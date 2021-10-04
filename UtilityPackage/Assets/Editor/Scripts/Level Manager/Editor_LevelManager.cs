using FickleFrames;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(LevelManager))]
public class Editor_LevelManager : Editor
{

    // Static objects
    static LevelManager manager = null;
    static List<LevelController> controllers = new List<LevelController>();

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        propertyUpdate();
        serializedObject.ApplyModifiedProperties();
    }

    private void propertyUpdate()
    {
        loadValues();

        property_levelControllerPath();
    }

    private void loadValues()
    {
        if (manager == null)
            manager = (LevelManager)target;
    }

    private void property_levelControllerPath()
    {
        EditorGUILayout.PropertyField
            (
                serializedObject.FindProperty("levelControllerPath"),
                new GUIContent
                    (
                        "Controller Path",
                        "The path where all the level controller objects are stored"
                    )
            );

        if (serializedObject.FindProperty("levelControllerPath").stringValue == "")
            EditorGUILayout.HelpBox("Level controller path should be initialized for this manager to work!", MessageType.Error);
        else
        {
            string controllerPath = serializedObject.FindProperty("levelControllerPath").stringValue;

            if (!Directory.Exists(controllerPath) || Directory.GetFiles(controllerPath, "*.asset").Length == 0)
            {
                EditorGUILayout.HelpBox("Invalid directory!!", MessageType.Error);
                return;
            }
            if (Directory.GetFiles(controllerPath, "*.asset").ToList().Find(x => AssetDatabase.LoadAssetAtPath<LevelController>(x) != null) == default)
            {
                EditorGUILayout.HelpBox("Invalid directory!! No controllers", MessageType.Error);
                return;
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Level Controllers Found", EditorStyles.boldLabel);
            bool shouldRefresh = GUILayout.Button("Force Update");

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);

            if (controllers.Count != Directory.GetFiles(controllerPath, "*.asset").Length || shouldRefresh)
            {
                int index = 0;
                controllers.Clear();
                serializedObject.FindProperty("levelsData").ClearArray();

                foreach (string path in Directory.EnumerateFiles(controllerPath, "*.asset"))
                {
                    LevelController controller = AssetDatabase.LoadAssetAtPath<LevelController>(path);

                    if (controller != null && controller.IsControllerUsable())
                    {
                        controllers.TryAdd(controller);
                        serializedObject.FindProperty("levelsData").InsertArrayElementAtIndex(index);
                        serializedObject.FindProperty("levelsData").GetArrayElementAtIndex(index).FindPropertyRelative("levelName").stringValue = controller.levelName;
                        serializedObject.FindProperty("levelsData").GetArrayElementAtIndex(index).FindPropertyRelative("controller").objectReferenceValue = controller;
                        index++;
                    }
                }
            }

            if (controllers.Count > 0)
            {
                foreach (LevelController controller in controllers)
                {
                    EditorGUILayout.LabelField("Controller name : ", controller.name);
                    EditorGUILayout.LabelField("Level being controlled : ", controller.levelName);
                    EditorGUILayout.LabelField("Load mode : ", controller.loadMode.ToString());
                    EditorGUILayout.Space(5);
                }
            }
        }
    }
}
