using FickleFrames.Managers.Internal;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace FickleFrames.Managers.Editor_
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : BaseEditor<LevelManager>
    {
        string controllerPath;

        private void InspectorUpdate()
        {
            space(5);

            // print the details of all level controllers
            if (button("Clear Levels In Build", 25))
                EditorBuildSettings.scenes = new EditorBuildSettingsScene[] { };

            space(5);
            heading("Level Controller Settings");
            space(2);

            // doNotDestroyOnLoad
            propertyField(getProperty("doNotDestroyOnLoad"), "Do Not Destroy On Load", "Set this as true if this asset has to persist through scenes");

            // _levelControllerPath
            List<LevelController> levelControllerList = new List<LevelController>();
            propertyField(getProperty("_levelControllerPath"), "Level Controller Path", "File path where all level controllers are located");
            controllerPath = getProperty("_levelControllerPath").stringValue;

            if (controllerPath == "")
            {
                space(5);
                info("This field cannot be empty", MessageType.Error);
                return;
            }

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

                foreach (string path in Directory.EnumerateFiles(controllerPath))
                {
                    LevelController controller = AssetDatabase.LoadAssetAtPath<LevelController>(path);
                    if (controller != null)
                        levelControllerList.Add(controller);
                }

                EditorGUILayout.BeginHorizontal();
                if (button("Truncate Database", 25))
                {
                    getProperty("_levelControllers").ClearArray();
                    levelControllerList.Clear();
                }
                if (button("Refresh Database", 25))
                {
                    foreach (LevelController controller in levelControllerList)
                    {
                        int foundIndex = -1;
                        for (int i = 0; i < getProperty("_levelControllers").arraySize; ++i)
                        {
                            LevelController temp = (LevelController)getProperty("_levelControllers").GetArrayElementAtIndex(i).objectReferenceValue;
                            if (temp == null)
                            {
                                foundIndex = i;
                                break;
                            }
                            if (temp.Equals(controller))
                            {
                                foundIndex = -2;
                                break;
                            }
                        }

                        if (foundIndex == -2)
                            continue;
                        else if (foundIndex == -1)
                        {
                            int index = getProperty("_levelControllers").arraySize;
                            getProperty("_levelControllers").InsertArrayElementAtIndex(index);
                            getProperty("_levelControllers").GetArrayElementAtIndex(index).objectReferenceValue = controller;
                        }
                        else
                            getProperty("_levelControllers").GetArrayElementAtIndex(foundIndex).objectReferenceValue = controller;
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (levelControllerList.Count == 0)
                    info($"No level controllers in {controllerPath}", MessageType.Warning);
            }

            // Print all levels
            if (getProperty("_levelControllers").arraySize == 0)
                return;

            List<EditorBuildSettingsScene> scenesInBuild = new List<EditorBuildSettingsScene>();
            scenesInBuild.AddRange(EditorBuildSettings.scenes);

            space(10);
            heading("Loaded Level Controllers");
            space(15);

            for (int index = 0; index < getProperty("_levelControllers").arraySize; ++index)
            {
                LevelController controller = (LevelController)getProperty("_levelControllers").GetArrayElementAtIndex(index).objectReferenceValue;
                if (controller == null)
                    continue;

                SceneAsset sceneAsset = controller.serializedScene;
                string levelName = sceneAsset == null ? null : sceneAsset.name;
                bool sceneFound = scenesInBuild.Find(x => Path.GetFileNameWithoutExtension(x.path) == levelName) != null;

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("X", GUILayout.Height(80), GUILayout.Width(20)))
                {
                    getProperty("_levelControllers").GetArrayElementAtIndex(index).objectReferenceValue = null;
                    getProperty("_levelControllers").DeleteArrayElementAtIndex(index);

                    if (sceneFound)
                        scenesInBuild.Remove(scenesInBuild.Find(scene => Path.GetFileNameWithoutExtension(scene.path) == levelName));
                    continue;
                }
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Level Name : ", string.IsNullOrEmpty(levelName) ? "-" : levelName);
                EditorGUILayout.LabelField("Level Controller Name : ", controller.name);
                EditorGUILayout.LabelField("Level Load Mode : ", controller.LoadMode.ToString());
                EditorGUILayout.LabelField("Level Physics Mode : ", controller.PhysicsMode.ToString());
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();

                if (string.IsNullOrEmpty(levelName))
                    info("Level controllers not initialized properly!!", MessageType.Error);
                else if (sceneFound)
                {
                    if (button($"Remove {levelName} from build", 20))
                        scenesInBuild.Remove(scenesInBuild.Find(scene => Path.GetFileNameWithoutExtension(scene.path) == levelName));
                }
                else if (!string.IsNullOrEmpty(levelName))
                {
                    if (button($"Add {levelName} to build", 20))
                        scenesInBuild.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(sceneAsset), true));
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