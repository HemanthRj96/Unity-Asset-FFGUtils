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
            Space(5);

            // print the details of all level controllers
            if (Button("Clear Levels In Build", 25))
                EditorBuildSettings.scenes = new EditorBuildSettingsScene[] { };

            Space(5);
            Heading("Level Controller Settings");
            Space(2);

            // doNotDestroyOnLoad
            PropertyField(GetProperty("doNotDestroyOnLoad"), "Do Not Destroy On Load", "Set this as true if this asset has to persist through scenes");

            // _levelControllerPath
            List<LevelController> levelControllerList = new List<LevelController>();
            PropertyField(GetProperty("_levelControllerPath"), "Level Controller Path", "File path where all level controllers are located");
            controllerPath = GetProperty("_levelControllerPath").stringValue;

            if (controllerPath == "")
            {
                Space(5);
                Info("This field cannot be empty", MessageType.Error);
                return;
            }

            if (!Directory.Exists(controllerPath))
            {
                Space(5);
                EditorGUILayout.BeginHorizontal();
                Info("This directory do not exist!!");
                if (Button("Create new directory", 37.5f))
                {
                    Directory.CreateDirectory(controllerPath);
                    AssetDatabase.Refresh();
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                Space(5);

                foreach (string path in Directory.EnumerateFiles(controllerPath))
                {
                    LevelController controller = AssetDatabase.LoadAssetAtPath<LevelController>(path);
                    if (controller != null)
                        levelControllerList.Add(controller);
                }

                EditorGUILayout.BeginHorizontal();
                if (Button("Truncate Database", 25))
                {
                    GetProperty("_levelControllers").ClearArray();
                    levelControllerList.Clear();
                }
                if (Button("Refresh Database", 25))
                {
                    foreach (LevelController controller in levelControllerList)
                    {
                        int foundIndex = -1;
                        for (int i = 0; i < GetProperty("_levelControllers").arraySize; ++i)
                        {
                            LevelController temp = (LevelController)GetProperty("_levelControllers").GetArrayElementAtIndex(i).objectReferenceValue;
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
                            int index = GetProperty("_levelControllers").arraySize;
                            GetProperty("_levelControllers").InsertArrayElementAtIndex(index);
                            GetProperty("_levelControllers").GetArrayElementAtIndex(index).objectReferenceValue = controller;
                        }
                        else
                            GetProperty("_levelControllers").GetArrayElementAtIndex(foundIndex).objectReferenceValue = controller;
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (levelControllerList.Count == 0)
                    Info($"No level controllers in {controllerPath}", MessageType.Warning);
            }

            // Print all levels
            if (GetProperty("_levelControllers").arraySize == 0)
                return;

            List<EditorBuildSettingsScene> scenesInBuild = new List<EditorBuildSettingsScene>();
            scenesInBuild.AddRange(EditorBuildSettings.scenes);

            Space(10);
            Heading("Loaded Level Controllers");
            Space(15);

            for (int index = 0; index < GetProperty("_levelControllers").arraySize; ++index)
            {
                LevelController controller = (LevelController)GetProperty("_levelControllers").GetArrayElementAtIndex(index).objectReferenceValue;
                if (controller == null)
                    continue;

                SceneAsset sceneAsset = controller.serializedScene;
                string levelName = sceneAsset == null ? null : sceneAsset.name;
                bool sceneFound = scenesInBuild.Find(x => Path.GetFileNameWithoutExtension(x.path) == levelName) != null;

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("X", GUILayout.Height(80), GUILayout.Width(20)))
                {
                    GetProperty("_levelControllers").GetArrayElementAtIndex(index).objectReferenceValue = null;
                    GetProperty("_levelControllers").DeleteArrayElementAtIndex(index);

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
                    Info("Level controllers not initialized properly!!", MessageType.Error);
                else if (sceneFound)
                {
                    if (Button($"Remove {levelName} from build", 20))
                        scenesInBuild.Remove(scenesInBuild.Find(scene => Path.GetFileNameWithoutExtension(scene.path) == levelName));
                }
                else if (!string.IsNullOrEmpty(levelName))
                {
                    if (Button($"Add {levelName} to build", 20))
                        scenesInBuild.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(sceneAsset), true));
                }

                Space(15);
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