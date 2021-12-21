using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;
using FickleFrameGames.Controllers;


[CustomEditor(typeof(StateController))]
public class StateControllerEditor : BaseEditor<StateController>
{
    string controllerFilepath;
    ReorderableList list;

    bool validDirectory => Directory.Exists(controllerFilepath);

    private void InspectorUpdate()
    {
        SerializedProperty stateControllerFilepath = GetProperty("_stateControllerFilepath");
        SerializedProperty states = GetProperty("_states");
        SerializedProperty stateArray(int index) => states.GetArrayElementAtIndex(index);
        SerializedProperty defaultState = GetProperty("_defaultStateName");
        SerializedProperty stateSharedData = GetProperty("_data");
        SerializedProperty stateSyncInput = GetProperty("_input");
        bool dataSet = false;
        bool inputSet = false;

        Space(5);

        #region stateControllerFilepath

        // stateControllerFilepath
        PropertyField(stateControllerFilepath, "Controller File Path", "Path where all state controller objects for this StateController component are saved");
        controllerFilepath = stateControllerFilepath.stringValue;

        if (controllerFilepath == "")
            Info("Invalid filepath, please provide a valid filepath", MessageType.Warning);
        else if (!validDirectory && Button($"Create directory {controllerFilepath}", 25))
        {
            Directory.CreateDirectory(controllerFilepath);
            AssetDatabase.Refresh();
        }

        if (!validDirectory)
            return;

        #endregion

        Space(8);

        #region StateSyncData and StateSyncInput

        dataSet = stateSharedData.objectReferenceValue != null;
        inputSet = stateSyncInput.objectReferenceValue != null;

        if (!dataSet || !inputSet)
        {
            foreach (string path in Directory.GetFiles(controllerFilepath, "*.asset"))
            {
                StateSharedData data;
                StateSyncInput input;

                if (!dataSet)
                {
                    data = AssetDatabase.LoadAssetAtPath<StateSharedData>(path);
                    if (data != null)
                    {
                        stateSharedData.objectReferenceValue = data;
                        dataSet = true;
                    }
                }
                if(!inputSet)
                {
                    input = AssetDatabase.LoadAssetAtPath<StateSyncInput>(path);
                    if(input != null)
                    {
                        stateSyncInput.objectReferenceValue = input;
                        inputSet = true;
                    }
                }
                if (inputSet && dataSet)
                    break;
            }
        }

        if (!dataSet || !inputSet)
        {
            if (inputSet == false)
                Info(
                        "StateSyncInput is not set! To resolve this error create a StateSyncInput scriptable object inside " +
                        $"the current Filepath ({controllerFilepath}) that is if you have already created a custom " +
                        "StateSyncInput script. If not then create a new script from Context Menu",
                        MessageType.Error
                    );
            if(dataSet == false)
                Info(
                        "StateSharedData is not set! To resolve this error create a StateSharedData scriptable object inside " +
                        $"the current Filepath ({controllerFilepath}) that is if you have already created a custom " +
                        "StateSharedData script. If not then create a new script from Context Menu",
                        MessageType.Error
                    );
            return;
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
                    var state = AssetDatabase.LoadAssetAtPath<FickleFrameGames.Controllers.State>(path);
                    int currentIndex = -1;

                    if (state != null)
                    {
                        // Find the empty element index
                        for (int i = 0; i < states.arraySize; ++i)
                        {
                            string sn = stateArray(i).FindPropertyRelative("StateName").stringValue;
                            FickleFrameGames.Controllers.State s =
                                (FickleFrameGames.Controllers.State)stateArray(i).FindPropertyRelative("State").objectReferenceValue;

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
                            currentIndex = states.arraySize;
                            states.InsertArrayElementAtIndex(currentIndex);
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
        string firstArrElem = null;
        for (int i = states.arraySize - 1; i >= 0; --i)
        {
            var state = (FickleFrameGames.Controllers.State)stateArray(i).FindPropertyRelative("State").objectReferenceValue;
            string stateName = stateArray(i).FindPropertyRelative("StateName").stringValue;

            if (state != null && stateName != null)
            {
                flag = true;
                firstArrElem = stateName;
            }
        }

        defaultState.stringValue = flag ? firstArrElem : "";
        EditorGUI.BeginDisabledGroup(true);
        PropertyField
            (
                defaultState,
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
        var states = GetProperty("_states");
        list = new ReorderableList(serializedObject, states, true, true, true, true);
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
