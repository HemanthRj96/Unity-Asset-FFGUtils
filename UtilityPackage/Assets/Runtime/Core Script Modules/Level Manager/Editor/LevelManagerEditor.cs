using FickleFrames.Managers.Internal;
using System.Collections.Generic;
using System.IO;
using UnityEditor;


namespace FickleFrames.Managers.Editor_
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
    public class LevelManagerEditor : BaseEditor<LevelManager>
    {
        string controllerPath;

        private void InspectorUpdate()
        {
            space(10);
            heading("Level Controller Settings");
            space(2);

            // doNotDestroyOnLoad
            propertyField(getProperty("doNotDestroyOnLoad"), "Do Not Destroy On Load", "Set this as true if this asset has to persist through scenes");

            // levelControllerPath
            propertyField(getProperty("levelControllerPath"), "Level Controller Path", "File path where all level controllers are located");
            controllerPath = getProperty("levelControllerPath").stringValue;
            if (controllerPath == "")
            {
                space(5);
                info("This field cannot be empty", MessageType.Error);
                return;
            }
            else
            {
                if (!Directory.Exists(controllerPath))
                {
                    space(5);
                    EditorGUILayout.BeginHorizontal();
                    info("This directory do not exist!!");
                    if (button("Create new directory", 37.5f))
                    {
                        Directory.CreateDirectory(controllerPath);
                        AssetDatabase.Refresh();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    space(5);

                    EditorGUILayout.BeginHorizontal();
                    if (button("Truncate Database", 25))
                    {
                        getProperty("levelControllers").ClearArray();
                    }
                    if (button("Refresh Database", 25))
                    {
                        int index = 0;
                        getProperty("levelControllers").ClearArray();
                        foreach (string path in Directory.EnumerateFiles(controllerPath))
                        {
                            LevelController controller = AssetDatabase.LoadAssetAtPath<LevelController>(path);
                            if (controller != null)
                            {
                                getProperty("levelControllers").InsertArrayElementAtIndex(index);
                                getProperty("levelControllers").GetArrayElementAtIndex(index).objectReferenceValue = controller;
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (getProperty("levelControllers").arraySize == 0)
                        info($"No level controllers in {controllerPath}", MessageType.Warning);
                }
            }

            space(20);
            heading("Level Build Settings");

            // print the details of all level controllers
            List<EditorLevelData> levelDataCacheList = new List<EditorLevelData>();
            List<EditorBuildSettingsScene> scenesInBuild = new List<EditorBuildSettingsScene>();

            if (button("Clear Levels In Build", 25))
                EditorBuildSettings.scenes = new EditorBuildSettingsScene[] { };

            scenesInBuild.AddRange(EditorBuildSettings.scenes);

            // Get all levels from level controllers
            for (int i = 0; i < getProperty("levelControllers").arraySize; ++i)
            {
                LevelController cached = (LevelController)getProperty("levelControllers").GetArrayElementAtIndex(i).objectReferenceValue;
                if (cached != null)
                {
                    string levelName = cached.serializedScene == null ? null : cached.serializedScene.name;
                    levelDataCacheList.Add(new EditorLevelData(cached, levelName, cached.serializedScene));
                }
            }

            space(15);

            // Print all levels
            for (int index = 0; index < levelDataCacheList.Count; ++index)
            {
                var level = levelDataCacheList[index];
                var foundScene = scenesInBuild.Find(scene => Path.GetFileNameWithoutExtension(scene.path) == level.levelName);
                bool sceneFound = foundScene != null;

                EditorGUILayout.LabelField("Level Name : ", string.IsNullOrEmpty(level.levelName) ? "-" : level.levelName);
                EditorGUILayout.LabelField("Level Controller Name : ", level.controller.name);
                EditorGUILayout.LabelField("Level Load Mode : ", level.controller.loadMode.ToString());
                EditorGUILayout.LabelField("Level Physics Mode : ", level.controller.physicsMode.ToString());

                if (string.IsNullOrEmpty(level.levelName))
                    info("Level controllers not initialized properly!!", MessageType.Error);
                else if (sceneFound)
                {
                    if (button($"Remove {level.levelName} from build", 20))
                        scenesInBuild.Remove(scenesInBuild.Find(scene => Path.GetFileNameWithoutExtension(scene.path) == level.levelName));
                }
                else if (!string.IsNullOrEmpty(level.levelName))
                {
                    if (button($"Add {level.levelName} to build", 20))
                        scenesInBuild.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(level.sceneAsset), true));
                }

                space(15);
            }
            EditorBuildSettings.scenes = scenesInBuild.ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            InspectorUpdate();
            serializedObject.ApplyModifiedProperties();
        }
    }
}