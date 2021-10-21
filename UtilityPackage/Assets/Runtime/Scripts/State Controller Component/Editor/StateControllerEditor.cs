using UnityEngine;
using UnityEditor;
using System.IO;

namespace FickleFrames.Controllers.StateControllerEditor_
{
    [CustomEditor(typeof(StateController))]
    public class StateControllerEditor : CustomInspector<StateController>
    {
        string templateContent;
        string controllerFilepath;
        bool validDirectory => Directory.Exists(controllerFilepath);

        private void InspectorUpdate()
        {
            // stateControllerFilepath
            propertyField(getProperty("stateControllerFilepath"), "Controller File Path", "Path where all controller scriptable objects are saved");
            controllerFilepath = getProperty("stateControllerFilepath").stringValue;

            if (controllerFilepath == "")
                info(@"Cannot create or load states statically", MessageType.Warning);
            else if (!validDirectory && button($"Create directory {controllerFilepath}", 25))
                Directory.CreateDirectory(controllerFilepath);



            // Load controllers from directory
            if (validDirectory)
            {
                if (button("Load Controllers From Directory", 25))
                {
                    int arraySize = getProperty("stateNames").arraySize;
                    int j = 0;
                    foreach (string path in Directory.GetFiles(controllerFilepath, "*.asset"))
                    {
                        State state = AssetDatabase.LoadAssetAtPath<State>(path);
                        if (state != null)
                        {
                        repeat:

                            if (j > arraySize)
                            {
                                getProperty("stateNames").InsertArrayElementAtIndex(j);
                                getProperty("states").InsertArrayElementAtIndex(j);
                            }
                            string sn = getProperty("stateNames").GetArrayElementAtIndex(j).stringValue;
                            State s = (State)getProperty("states").GetArrayElementAtIndex(j).objectReferenceValue;

                            if (string.IsNullOrEmpty(sn) && s == null)
                            {
                                getProperty("stateNames").GetArrayElementAtIndex(j).stringValue = state.name;
                                getProperty("states").GetArrayElementAtIndex(j).objectReferenceValue = state;
                                ++j;
                            }
                            else
                            {
                                ++j;
                                goto repeat;
                            }
                        }
                    }
                }
            }


            //stateNames and states
            space(10);
            int index = getProperty("stateNames").arraySize;

            EditorGUILayout.BeginHorizontal();
            if (button("Add State"))
            {
                getProperty("stateNames").InsertArrayElementAtIndex(index);
                getProperty("states").InsertArrayElementAtIndex(index);
            }
            if (button("Remove State"))
            {
                if (index > 0)
                {
                    getProperty("stateNames").DeleteArrayElementAtIndex(index - 1);
                    getProperty("states").DeleteArrayElementAtIndex(index - 1);
                }
            }
            EditorGUILayout.EndHorizontal();


            for (int i = 0; i < getProperty("stateNames").arraySize; ++i)
            {
                GUILayout.Label($"State #{i + 1} :");
                EditorGUILayout.BeginHorizontal();
                propertyField(getProperty("stateNames").GetArrayElementAtIndex(i), "", "");
                propertyField(getProperty("states").GetArrayElementAtIndex(i), "", "");
                EditorGUILayout.EndHorizontal();

                string stateName = getProperty("stateNames").GetArrayElementAtIndex(i).stringValue;
                State state = (State)getProperty("states").GetArrayElementAtIndex(i).objectReferenceValue;

                if (validDirectory && !string.IsNullOrEmpty(stateName) && state == null)
                {
                    if (button($"Create {stateName} from code template"))
                    {
                        // Create shit
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