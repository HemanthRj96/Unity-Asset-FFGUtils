using System.Collections.Generic;
using System.IO;
using UnityEditor;


namespace FickleFrames.Managers.LevelManagerEditor_
{
    struct EditorLevelData
    {
        public EditorLevelData(LevelController controller, string levelName = default, SceneAsset sceneAsset = null)
        {
            this.controller = controller;
            this.levelName = levelName;
            this.sceneAsset = sceneAsset;
        }

        public LevelController controller;
        public string levelName;
        public SceneAsset sceneAsset;
    }

    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : CustomInspector<LevelManager>
    {
        string levelControllerPath;

        private void InspectorUpdate()
        {
            space(10);
            heading("Level Controller Settings");
            space(2);

            // levelControllerPath
            propertyField(getProperty("levelControllerPath"), "Level Controller Path", "File path where all level controllers are located");
            levelControllerPath = getProperty("levelControllerPath").stringValue;
            if (levelControllerPath == "")
            {
                space(5);
                info("This field cannot be empty", MessageType.Error);
                return;
            }
            else
            {
                if (!Directory.Exists(levelControllerPath))
                {
                    space(5);
                    EditorGUILayout.BeginHorizontal();
                    info("This directory do not exist!!");
                    if (button("Create new directory", 37.5f))
                    {
                        Directory.CreateDirectory(levelControllerPath);
                        AssetDatabase.Refresh();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    space(5);
                    if (button("Refresh Database", 25))
                    {
                        int index = 0;
                        getProperty("levelControllers").ClearArray();
                        foreach (string path in Directory.EnumerateFiles(levelControllerPath))
                        {
                            LevelController controller = AssetDatabase.LoadAssetAtPath<LevelController>(path);
                            if (controller != null)
                            {
                                getProperty("levelControllers").InsertArrayElementAtIndex(index);
                                getProperty("levelControllers").GetArrayElementAtIndex(index).objectReferenceValue = controller;
                            }
                        }
                    }
                    if (getProperty("levelControllers").arraySize == 0)
                        info($"No level controllers in {levelControllerPath}", MessageType.Warning);
                }
            }


            // print the details of all level controllers
            List<EditorLevelData> levels = new List<EditorLevelData>();
            List<EditorBuildSettingsScene> includedScenes = new List<EditorBuildSettingsScene>();

            space(20);
            heading("Level Build Settings");

            if (button("Clear Levels In Build", 25))
                EditorBuildSettings.scenes = new EditorBuildSettingsScene[] { };

            includedScenes.AddRange(EditorBuildSettings.scenes);
            // Get all levels from level controllers
            for (int i = 0; i < getProperty("levelControllers").arraySize; ++i)
            {
                LevelController cached = (LevelController)getProperty("levelControllers").GetArrayElementAtIndex(i).objectReferenceValue;
                if (cached != null)
                {
                    string levelName = cached.serializedScene == null ? null : cached.serializedScene.name;
                    levels.Add(new EditorLevelData(cached, levelName, cached.serializedScene));
                }
            }

            space(15);

            // Print all levels
            foreach (EditorLevelData level in levels)
            {
                var foundScene = includedScenes.Find(scene => Path.GetFileNameWithoutExtension(scene.path) == level.levelName);
                bool sceneFound = foundScene != null;

                EditorGUILayout.LabelField("Level Name : ", level.levelName);
                EditorGUILayout.LabelField("Level Controller Name : ", level.controller.name);
                EditorGUILayout.LabelField("Level Load Mode : ", level.controller.loadMode.ToString());
                EditorGUILayout.LabelField("Level Physics Mode : ", level.controller.physicsMode.ToString());

                if (string.IsNullOrEmpty(level.levelName))
                    info("Level controllers not initialized properly!!", MessageType.Error);
                else if (sceneFound)
                {
                    if (button($"Remove {level.levelName} from build", 20))
                        includedScenes.Remove(includedScenes.Find(scene => Path.GetFileNameWithoutExtension(scene.path) == level.levelName));
                }
                else if (!string.IsNullOrEmpty(level.levelName))
                {
                    if (button($"Add {level.levelName} to build", 20))
                        includedScenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(level.sceneAsset), true));
                }

                space(15);
            }
            EditorBuildSettings.scenes = includedScenes.ToArray();
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            InspectorUpdate();
            serializedObject.ApplyModifiedProperties();
        }
    }
}