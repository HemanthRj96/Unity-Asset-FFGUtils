using UnityEngine;
using UnityEditor;
using System.IO;

namespace FickleFrames.Controllers.StateControllerEditor_
{
    [CustomEditor(typeof(StateController))]
    public class StateControllerEditor : CustomInspector<StateController>
    {
        string controllerFilepath;
        string scriptSuffix;
        bool validDirectory => Directory.Exists(controllerFilepath);
        bool validScript => !string.IsNullOrEmpty(scriptSuffix);
        SerializedProperty stateArray(int index) => getProperty("states").GetArrayElementAtIndex(index);

        private void InspectorUpdate()
        {
            space(5);

            // stateControllerFilepath
            propertyField(getProperty("stateControllerFilepath"), "Controller File Path", "Path where all controller scriptable objects are saved");
            controllerFilepath = getProperty("stateControllerFilepath").stringValue;

            if (controllerFilepath == "")
                info("Cannot create or load states statically", MessageType.Warning);
            else if (!validDirectory && button($"Create directory {controllerFilepath}", 25))
                Directory.CreateDirectory(controllerFilepath);


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

            space(5);

            // scriptSuffix
            if (validDirectory)
            {
                propertyField(getProperty("scriptSuffix"), "Script Suffix", "Suffix added to script files as a unique identifier");
                scriptSuffix = getProperty("scriptSuffix").stringValue;
            }
            if (string.IsNullOrEmpty(scriptSuffix))
                info("Field empty! Cannot create code file from template!", MessageType.Warning);


            // state array manipulator
            space(10);
            int index = getProperty("states").arraySize;

            EditorGUILayout.BeginHorizontal();

            if (button("Add State"))
                getProperty("states").InsertArrayElementAtIndex(index);

            if (button("Remove State"))
                if (index > 0)
                    getProperty("states").DeleteArrayElementAtIndex(index - 1);

            EditorGUILayout.EndHorizontal();


            // print all elements inside stateArray
            for (int i = 0; i < getProperty("states").arraySize; ++i)
            {
                GUILayout.Label($"State #{i + 1} :");
                EditorGUILayout.BeginHorizontal();

                propertyField(stateArray(i).FindPropertyRelative("stateName"), "", "");
                propertyField(stateArray(i).FindPropertyRelative("state"), "", "");
                if(GUILayout.Button("X", GUILayout.Width(20)))
                {
                    getProperty("states").DeleteArrayElementAtIndex(i);
                    continue;
                }

                EditorGUILayout.EndHorizontal();

                string stateName = stateArray(i).FindPropertyRelative("stateName").stringValue;
                State state = (State)stateArray(i).FindPropertyRelative("state").objectReferenceValue;

                // check if all conditions for script creation is met
                if (validDirectory && validScript && !string.IsNullOrEmpty(stateName) && state == null)
                {
                    // create code file
                    if (!File.Exists($"{controllerFilepath}/{stateName}_{scriptSuffix}.cs") && button($"Create {stateName} from code template"))
                    {
                        string templateContent;
                        string templateFilepath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName().Substring(Directory.GetCurrentDirectory().Length + 1);
                        templateFilepath = templateFilepath.Replace("StateControllerEditor.cs", "script_template.txt");
                        TextAsset templateFile = AssetDatabase.LoadAssetAtPath<TextAsset>(templateFilepath);

                        templateContent = templateFile.text;
                        templateContent = templateContent.Replace($"_FILE_NAME_", $"{stateName}");
                        templateContent = templateContent.Replace($"_GAMEOBJECT_NAME_", $"{root.gameObject.name}");
                        templateContent = templateContent.Replace($"_STATE_NAME_", $"{stateName}");
                        templateContent = templateContent.Replace($"_SCRIPT_NAME_", $"{stateName}_{scriptSuffix}");

                        using (StreamWriter sw = new StreamWriter($"{controllerFilepath}/{stateName}_{scriptSuffix}.cs"))
                        {
                            sw.Write(templateContent);
                        }
                        AssetDatabase.Refresh();
                    }

                    // create scriptable object
                    if (
                        !EditorApplication.isCompiling &&
                        File.Exists($"{controllerFilepath}/{stateName}_{scriptSuffix}.cs") &&
                        !File.Exists($"{controllerFilepath}/{stateName}.asset") &&
                        button($"Construct {stateName} object from code file")
                        )
                    {
                        State instance = (State)CreateInstance($"{stateName}_{scriptSuffix}");
                        if (instance != null)
                        {
                            AssetDatabase.CreateAsset(instance, $"{controllerFilepath}/{stateName}.asset");
                            State cachedState = AssetDatabase.LoadAssetAtPath<State>($"{ controllerFilepath}/{stateName}.asset");
                            if (cachedState != null)
                                stateArray(i).FindPropertyRelative("state").objectReferenceValue = cachedState;
                        }
                        else
                        {
                            Debug.LogError("ASSET CREATION FAILED!!");
                        }
                    }
                }
                space(7.5f);
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
