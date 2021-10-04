using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelController))]
public class Editor_LevelController : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        propertyUpdate();
        serializedObject.ApplyModifiedProperties();
    }

    private void propertyUpdate()
    {
        property_levelName();

        property_sceneLoadParams();
    }


    private void property_levelName()
    {
        EditorGUILayout.PropertyField
            (
                serializedObject.FindProperty("serializedScene"),
                new GUIContent
                    (
                        "Target scene",
                        "The scene this controller will handle"
                    )
            );
        if (serializedObject.FindProperty("serializedScene").objectReferenceValue != null)
        {
            string levelName = serializedObject.FindProperty("serializedScene").objectReferenceValue.name;
            serializedObject.FindProperty("levelName").stringValue = levelName;
            serializedObject.FindProperty("isControllerReady").boolValue = true;
        }
        else
        {
            EditorGUILayout.HelpBox("This value cannot be null! Initialize it with scene controlled by this SceneController", MessageType.Error);
            serializedObject.FindProperty("levelName").stringValue = null;
            serializedObject.FindProperty("isControllerReady").boolValue = false;
        }
    }


    private void property_sceneLoadParams()
    {

        if (serializedObject.FindProperty("levelName").stringValue == "")
            return;

        EditorGUILayout.PropertyField
            (
                serializedObject.FindProperty("loadMode"),
                new GUIContent
                    (
                        "Scene load mode",
                        "The way the scene has to be loaded"
                    )
            );
        EditorGUILayout.PropertyField
            (
                serializedObject.FindProperty("physicsMode"),
                new GUIContent
                    (
                        "Physics mode",
                        "Provides options for 2D and 3D local physics"
                    )
            );
    }
}
