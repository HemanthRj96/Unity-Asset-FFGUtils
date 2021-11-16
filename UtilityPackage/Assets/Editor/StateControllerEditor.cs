using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;
using FickleFrames.Controllers;


[CustomEditor(typeof(StateController))]
public class StateControllerEditor : BaseEditor<StateController>
{
    string controllerFilepath;
    ReorderableList list;
    SerializedProperty _states;

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
                    FickleFrames.Controllers.State state = AssetDatabase.LoadAssetAtPath<FickleFrames.Controllers.State>(path);
                    int currentIndex = -1;

                    if (state != null)
                    {
                        // Find the empty element index
                        for (int i = 0; i < GetProperty("_states").arraySize; ++i)
                        {
                            string sn = stateArray(i).FindPropertyRelative("StateName").stringValue;
                            FickleFrames.Controllers.State s = (FickleFrames.Controllers.State)stateArray(i).FindPropertyRelative("State").objectReferenceValue;

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

        Space(10);

        #region defaultState

        Space(5);

        // defaultState
        bool flag = false;
        string _defaultState = default;
        for (int i = GetProperty("_states").arraySize - 1; i >= 0; --i)
        {
            FickleFrames.Controllers.State state = (FickleFrames.Controllers.State)stateArray(i).FindPropertyRelative("State").objectReferenceValue;
            string stateName = stateArray(i).FindPropertyRelative("StateName").stringValue;
            if (state != null && stateName != null)
            {
                flag = true;
                _defaultState = stateName;
            }
        }

        GetProperty("_defaultStateName").stringValue = flag ? _defaultState : "";
        EditorGUI.BeginDisabledGroup(true);
        PropertyField
            (
                GetProperty("_defaultStateName"),
                "Default State Name : ", "This is the entry point or the default state this controller would be in upon update, " +
                "the default state is basically the first valid entry in the state controller list"
            );
        EditorGUI.EndDisabledGroup();

        #endregion

        Space(10);

        #region print elements inside stateArray

        // print all elements inside stateArray
        list.DoLayoutList();

        #endregion
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        InspectorUpdate();
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        _states = GetProperty("_states");
        list = new ReorderableList(serializedObject, _states, true, true, true, true);
        list.drawHeaderCallback = drawHeader;
        list.drawElementCallback = drawElement;
    }


    private void drawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "State Controllers");
    }

    private void drawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty elem = list.serializedProperty.GetArrayElementAtIndex(index);

        EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "State Name : ");
        EditorGUI.PropertyField
            (
                new Rect(rect.x + 77.5f, rect.y, 100, EditorGUIUtility.singleLineHeight),
                elem.FindPropertyRelative("StateName"),
                GUIContent.none
            );

        EditorGUI.LabelField(new Rect(rect.x + 180, rect.y, 100, EditorGUIUtility.singleLineHeight), "State Object : ");
        EditorGUI.PropertyField
            (
                new Rect(rect.x + 260, rect.y, 175, EditorGUIUtility.singleLineHeight),
                elem.FindPropertyRelative("State"),
                GUIContent.none
            );
    }

}
