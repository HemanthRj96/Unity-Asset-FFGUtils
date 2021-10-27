using UnityEngine;
using UnityEditor;
using System.IO;
using System;


namespace FickleFrames.Controllers.StateControllerEditor_
{
    [CustomEditor(typeof(StateController))]
    public class StateControllerEditor : BaseEditor<StateController>
    {
        string controllerFilepath;
        string suffix;
        bool validScript = false;


        bool validDirectory => Directory.Exists(controllerFilepath);
        SerializedProperty stateArray(int index) => getProperty("states").GetArrayElementAtIndex(index);


        private void InspectorUpdate()
        {
            space(5);
            stateControllerFilepath();
            loadControllersFromDirectory();

            space(5);
            scriptSuffix();

            space(10);
            stateArrayManipulation();

            space(10);
            printStateArray();
        }

        private void stateControllerFilepath()
        {
            // stateControllerFilepath
            propertyField(getProperty("stateControllerFilepath"), "Controller File Path", "Path where all controller scriptable objects are saved");
            controllerFilepath = getProperty("stateControllerFilepath").stringValue;

            if (controllerFilepath == "")
                info("Cannot create or load states statically", MessageType.Warning);
            else if (!validDirectory && button($"Create directory {controllerFilepath}", 25))
            {
                Directory.CreateDirectory(controllerFilepath);
                AssetDatabase.Refresh();
            }
        }
        
        private void loadControllersFromDirectory()
        {
            // Load controllers from directory
            if (validDirectory)
            {
                if (button("Load Controllers From Directory", 25))
                {
                    foreach (string path in Directory.GetFiles(controllerFilepath, "*.asset"))
                    {
                        State state = AssetDatabase.LoadAssetAtPath<State>(path);
                        int currentIndex = -1;

                        if (state != null)
                        {
                            // Find the empty element index
                            for (int i = 0; i < getProperty("states").arraySize; ++i)
                            {
                                string sn = stateArray(i).FindPropertyRelative("stateName").stringValue;
                                State s = (State)stateArray(i).FindPropertyRelative("state").objectReferenceValue;

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
                                currentIndex = getProperty("states").arraySize;
                                getProperty("states").InsertArrayElementAtIndex(currentIndex);
                            }

                            stateArray(currentIndex).FindPropertyRelative("stateName").stringValue = state.name;
                            stateArray(currentIndex).FindPropertyRelative("state").objectReferenceValue = state;
                            currentIndex = -1;
                        }
                    }
                }
            }
        }

        private void scriptSuffix()
        {
            // scriptSuffix
            if (validDirectory)
            {
                propertyField(getProperty("scriptSuffix"), "Script Suffix", "Suffix added to script files as a unique identifier");
                suffix = getProperty("scriptSuffix").stringValue;

                if (string.IsNullOrEmpty(suffix))
                {
                    info("Field empty! Cannot create code file from template!", MessageType.Warning);
                    validScript = false;
                }
                else if (Char.IsDigit(suffix[0]))
                {
                    info("Cannot begin with number!", MessageType.Error);
                    validScript = false;
                }
                else
                    validScript = true;
            }
        }

        private void stateArrayManipulation()
        {
            // state array manipulator
            int index = getProperty("states").arraySize;

            EditorGUILayout.BeginHorizontal();

            if (button("Add State"))
                getProperty("states").InsertArrayElementAtIndex(index);

            if (button("Remove State"))
                if (index > 0)
                    getProperty("states").DeleteArrayElementAtIndex(index - 1);

            EditorGUILayout.EndHorizontal();
        }

        private void printStateArray()
        {
            // print all elements inside stateArray
            for (int i = 0; i < getProperty("states").arraySize; ++i)
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    getProperty("states").DeleteArrayElementAtIndex(i);
                    continue;
                }
                propertyField(stateArray(i).FindPropertyRelative("stateName"), "", "");
                propertyField(stateArray(i).FindPropertyRelative("state"), "", "");

                EditorGUILayout.EndHorizontal();

                string stateName = stateArray(i).FindPropertyRelative("stateName").stringValue;
                State state = (State)stateArray(i).FindPropertyRelative("state").objectReferenceValue;

                // check if all conditions for script creation is met
                if (validDirectory && validScript && !string.IsNullOrEmpty(stateName) && state == null)
                {
                    createCodeFile(stateName);
                    createScriptableObject(i, stateName);
                }
                space(5);
            }
        }

        private void createCodeFile(string stateName)
        {
            var style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = Color.green;

            // create code file
            if (!File.Exists($"{controllerFilepath}/{stateName}_{suffix}.cs") && GUILayout.Button($"Create {stateName} from code template", style))
            {
                string templateContent;
                TextAsset templateFile = (TextAsset)EditorGUIUtility.Load("State Controller/script_template.txt");
                if (templateFile == null)
                    return;
                templateContent = templateFile.text;
                templateContent = templateContent.Replace($"_FILE_NAME_", $"{stateName}");
                templateContent = templateContent.Replace($"_GAMEOBJECT_NAME_", $"{root.gameObject.name}");
                templateContent = templateContent.Replace($"_STATE_NAME_", $"{stateName}");
                templateContent = templateContent.Replace($"_SCRIPT_NAME_", $"{stateName}_{suffix}");

                using (StreamWriter sw = new StreamWriter($"{controllerFilepath}/{stateName}_{suffix}.cs"))
                {
                    sw.Write(templateContent);
                }
                AssetDatabase.Refresh();
            }
        }

        private void createScriptableObject(int index, string stateName)
        {
            var style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = Color.cyan;
            // create scriptable object
            if (
                !EditorApplication.isCompiling &&
                File.Exists($"{controllerFilepath}/{stateName}_{suffix}.cs") &&
                !File.Exists($"{controllerFilepath}/{stateName}.asset") &&
                GUILayout.Button($"Construct {stateName} object from code file", style)
                )
            {
                State instance = (State)CreateInstance($"{stateName}_{suffix}");
                if (instance != null)
                {
                    AssetDatabase.CreateAsset(instance, $"{controllerFilepath}/{stateName}.asset");
                    State cachedState = AssetDatabase.LoadAssetAtPath<State>($"{ controllerFilepath}/{stateName}.asset");
                    if (cachedState != null)
                        stateArray(index).FindPropertyRelative("state").objectReferenceValue = cachedState;
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
