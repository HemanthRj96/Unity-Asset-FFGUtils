using FFG.Managers;
using FFG.Managers.Internal;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : BaseEditor<LevelManager>
{
    string controllerPath;
    static LevelManager Instance = null;

    SerializedProperty subManagerPath;
    SerializedProperty subManagers;

    public override void InspectorUpdate()
    {
        subManagerPath = GetProperty("_levelControllerPath");
        subManagers = GetProperty("_subManagers");

        if (Instance == null)
            Instance = Root;
        else if (!Instance.Equals(Root))
        {
            Info("Only a single instance of level manager is allowed, this manager will be removed on awake call", MessageType.Error);
            subManagers.ClearArray();
            return;
        }

        Space(5);

        // print the details of all level controllers
        if (Button("Clear Levels In Build", 25))
            EditorBuildSettings.scenes = new EditorBuildSettingsScene[] { };

        Space(5);
        Heading("Sub Level Manager Settings");
        Space(2);

        // _levelControllerPath
        List<SubLevelManager> levelControllerList = new List<SubLevelManager>();

        PropertyField(subManagerPath, "Sub Level Manager Path", "File path where all sub level managers are located");
        controllerPath = subManagerPath.stringValue;

        if (controllerPath == "")
        {
            Space(5);

            Info("This field cannot be empty", MessageType.Error);
            subManagers.ClearArray();
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
            subManagers.ClearArray();

            EditorGUILayout.EndHorizontal();
        }
        else
        {
            Space(5);

            foreach (string path in Directory.EnumerateFiles(controllerPath))
            {
                SubLevelManager controller = AssetDatabase.LoadAssetAtPath<SubLevelManager>(path);
                if (controller != null)
                    levelControllerList.Add(controller);
            }

            EditorGUILayout.BeginHorizontal();

            if (Button("Truncate Database", 25))
            {
                subManagers.ClearArray();
                levelControllerList.Clear();
            }
            if (Button("Refresh Database", 25))
            {
                foreach (SubLevelManager controller in levelControllerList)
                {
                    int foundIndex = -1;
                    for (int i = 0; i < subManagers.arraySize; ++i)
                    {
                        SubLevelManager temp = (SubLevelManager)subManagers.GetArrayElementAtIndex(i).objectReferenceValue;
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
                        int index = subManagers.arraySize;
                        subManagers.InsertArrayElementAtIndex(index);
                        subManagers.GetArrayElementAtIndex(index).objectReferenceValue = controller;
                    }
                    else
                        subManagers.GetArrayElementAtIndex(foundIndex).objectReferenceValue = controller;
                }
            }

            EditorGUILayout.EndHorizontal();

            if (levelControllerList.Count == 0)
                Info($"No sub level managers in {controllerPath}", MessageType.Warning);
        }


        // Print all levels
        if (subManagers.arraySize == 0)
            return;

        List<EditorBuildSettingsScene> scenesInBuild = new List<EditorBuildSettingsScene>();
        scenesInBuild.AddRange(EditorBuildSettings.scenes);

        Space(10);
        Heading("Loaded Sub Level Manaagers");
        Space(15);

        for (int index = 0; index < subManagers.arraySize; ++index)
        {
            SubLevelManager controller = (SubLevelManager)subManagers.GetArrayElementAtIndex(index).objectReferenceValue;

            if (controller == null)
                continue;

            SceneAsset sceneAsset = controller.SerializedScene;
            string levelName = sceneAsset == null ? null : sceneAsset.name;
            bool sceneFound = scenesInBuild.Find(x => Path.GetFileNameWithoutExtension(x.path) == levelName) != null;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("X", GUILayout.Height(60), GUILayout.Width(20)))
            {
                subManagers.GetArrayElementAtIndex(index).DeleteCommand();

                if (sceneFound)
                    scenesInBuild.Remove(scenesInBuild.Find(scene => Path.GetFileNameWithoutExtension(scene.path) == levelName));
                continue;
            }

            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Level Name : ", string.IsNullOrEmpty(levelName) ? "-" : levelName);
            EditorGUILayout.LabelField("Sub Level Manager Name : ", controller.name);
            EditorGUILayout.LabelField("Level Load Mode : ", controller.IsAdditive ? "Additive" : "Single");

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            if (string.IsNullOrEmpty(levelName))
                Info("Sub level managers not initialized properly!!", MessageType.Error);
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
}
