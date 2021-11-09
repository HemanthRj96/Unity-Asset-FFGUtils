using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;


namespace FickleFrames.Controllers.StateControllerEditor_
{
    [CustomEditor(typeof(StateController))]
    public class StateControllerEditor : BaseEditor<StateController>
    {
        string controllerFilepath;
        string suffix;
        bool validScript = false;

        bool validDirectory => Directory.Exists(controllerFilepath);
        SerializedProperty stateArray(int index) => GetProperty("_states").GetArrayElementAtIndex(index);


        private void InspectorUpdate()
        {
            Space(5);

            #region stateControllerFilepath

            // stateControllerFilepath
            PropertyField(GetProperty("stateControllerFilepath"), "Controller File Path", "Path where all controller scriptable objects are saved");
            controllerFilepath = GetProperty("stateControllerFilepath").stringValue;
            if (controllerFilepath == "")
                Info("Cannot create or load _states statically", MessageType.Warning);
            else if (!validDirectory && Button($"Create directory {controllerFilepath}", 25))
            {
                Directory.CreateDirectory(controllerFilepath);
                AssetDatabase.Refresh();
            }

            #endregion

            #region load controllers from directory

            // Load controllers from directory
            if (validDirectory)
            {
                if (Button("Load Controllers From Directory", 25))
                {
                    foreach (string path in Directory.GetFiles(controllerFilepath, "*.asset"))
                    {
                        State state = AssetDatabase.LoadAssetAtPath<State>(path);
                        int currentIndex = -1;

                        if (state != null)
                        {
                            // Find the empty element index
                            for (int i = 0; i < GetProperty("_states").arraySize; ++i)
                            {
                                string sn = stateArray(i).FindPropertyRelative("StateName").stringValue;
                                State s = (State)stateArray(i).FindPropertyRelative("State").objectReferenceValue;

                                if (string.IsNullOrEmpty(sn) && s == null)
                                {
                                    currentIndex = i;
                                    break;
                                }
                                else if (sn == state.name || s == state)
                                {
                                    currentIndex = -2;
                                    break;
                                }
                            }

                            if (currentIndex == -2)
                            {
                                currentIndex = -1;
                                continue;
                            }

                            if (currentIndex == -1)
                            {
                                currentIndex = GetProperty("_states").arraySize;
                                GetProperty("_states").InsertArrayElementAtIndex(currentIndex);
                            }
                            stateArray(currentIndex).FindPropertyRelative("StateName").stringValue = state.name;
                            stateArray(currentIndex).FindPropertyRelative("State").objectReferenceValue = state;
                            currentIndex = -1;
                        }
                    }
                }
            }

            #endregion

            Space(5);

            #region scriptSuffix

            // scriptSuffix
            if (validDirectory)
            {
                PropertyField(GetProperty("scriptSuffix"), "Script Suffix", "Suffix added to script files as a unique identifier");
                suffix = GetProperty("scriptSuffix").stringValue;
                if (string.IsNullOrEmpty(suffix))
                {
                    Info("Field empty! Cannot create code file from template!", MessageType.Warning);
                    validScript = false;
                }
                else if (Char.IsDigit(suffix[0]))
                {
                    Info("Cannot begin with number!", MessageType.Error);
                    validScript = false;
                }
                else
                    validScript = true;
            }

            #endregion

            Space(10);

            #region state array manipulator

            // state array manipulator
            int index = GetProperty("_states").arraySize;

            EditorGUILayout.BeginHorizontal();

            if (Button("Add State"))
                GetProperty("_states").InsertArrayElementAtIndex(index);

            if (Button("Remove State"))
                if (index > 0)
                {
                    GetProperty("_states").GetArrayElementAtIndex(index - 1).objectReferenceValue = null;
                    GetProperty("_states").DeleteArrayElementAtIndex(index - 1);
                }

            EditorGUILayout.EndHorizontal();

            #endregion

            #region defaultState

            Space(5);

            // defaultState
            List<string> entries = new List<string>();
            for (int i = 0; i < GetProperty("_states").arraySize; ++i)
            {
                State state = (State)stateArray(i).FindPropertyRelative("State").objectReferenceValue;
                string stateName = stateArray(i).FindPropertyRelative("StateName").stringValue;
                if (state != null && stateName != null)
                    entries.Add(stateName);
            }

            string defaultState = GetProperty("_defaultStateName").stringValue;
            int selection = Mathf.Max(0, entries.FindIndex(x => x == defaultState));

            if (entries.Count > 0)
                GetProperty("_defaultStateName").stringValue = entries[DropdownList("Default State : ", selection, entries.ToArray())];
            else
                GetProperty("_defaultStateName").stringValue = "";

            #endregion

            Space(10);

            #region print elements inside stateArray

            // print all elements inside stateArray
            for (int i = 0; i < GetProperty("_states").arraySize; ++i)
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    GetProperty("_states").GetArrayElementAtIndex(i).objectReferenceValue = null;
                    GetProperty("_states").DeleteArrayElementAtIndex(i);
                    continue;
                }
                PropertyField(stateArray(i).FindPropertyRelative("StateName"), "", "");
                PropertyField(stateArray(i).FindPropertyRelative("State"), "", "");

                EditorGUILayout.EndHorizontal();

                string StateName = stateArray(i).FindPropertyRelative("StateName").stringValue;
                State state = (State)stateArray(i).FindPropertyRelative("State").objectReferenceValue;

                // check if all conditions for script creation is met
                if (validDirectory && validScript && !string.IsNullOrEmpty(StateName) && state == null)
                {
                    createCodeFile(StateName);
                    createScriptableObject(i, StateName);
                }
                Space(5);
            }

            #endregion
        }

        private void createCodeFile(string StateName)
        {
            var style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = Color.green;

            // create code file
            if (!File.Exists($"{controllerFilepath}/{StateName}_{suffix}.cs") && GUILayout.Button($"Create {StateName} from code template", style))
            {
                string templateContent;
                TextAsset templateFile = (TextAsset)EditorGUIUtility.Load("State Controller/script_template.txt");
                if (templateFile == null)
                    return;
                templateContent = templateFile.text;
                templateContent = templateContent.Replace($"_FILE_NAME_", $"{StateName}");
                templateContent = templateContent.Replace($"_GAMEOBJECT_NAME_", $"{Root.gameObject.name}");
                templateContent = templateContent.Replace($"_STATE_NAME_", $"{StateName}");
                templateContent = templateContent.Replace($"_SCRIPT_NAME_", $"{StateName}_{suffix}");

                using (StreamWriter sw = new StreamWriter($"{controllerFilepath}/{StateName}_{suffix}.cs"))
                {
                    sw.Write(templateContent);
                }
                AssetDatabase.Refresh();
            }
        }

        private void createScriptableObject(int index, string StateName)
        {
            var style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = Color.cyan;
            // create scriptable object
            if (
                !EditorApplication.isCompiling &&
                File.Exists($"{controllerFilepath}/{StateName}_{suffix}.cs") &&
                !File.Exists($"{controllerFilepath}/{StateName}.asset") &&
                GUILayout.Button($"Construct {StateName} object from code file", style)
                )
            {
                State instance = (State)CreateInstance($"{StateName}_{suffix}");
                if (instance != null)
                {
                    AssetDatabase.CreateAsset(instance, $"{controllerFilepath}/{StateName}.asset");
                    State cachedState = AssetDatabase.LoadAssetAtPath<State>($"{ controllerFilepath}/{StateName}.asset");
                    if (cachedState != null)
                        stateArray(index).FindPropertyRelative("State").objectReferenceValue = cachedState;
                }
                else
                {
                    Debug.LogError("ASSET CREATION FAILED!!");
                }
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            InspectorUpdate();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
