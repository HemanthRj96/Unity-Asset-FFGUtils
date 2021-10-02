using UnityEngine;
using FickleFrames;
using UnityEditor;

[CustomEditor(typeof(ActionComponent))]
public class Editor_ActionComponent : Editor
{
    ActionComponent component = null;
    ActionComponent cachedComponent = null;
    GameObject gameObject;

    EOnActionBegin onActionBegin;
    EOnActionEnd onActionEnd;
    int selection = 0;
    string[] toolbars = { "Next action object", "Next action name" };
    bool isSlaveAvailable = false;
    bool isNameAvailable = false;


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        propertyUpdate();
        serializedObject.ApplyModifiedProperties();
    }


    /// <summary>
    /// All updates happen here
    /// </summary>
    private void propertyUpdate()
    {
        loadValues();

        GUILayout.Space(10);

        property_actionName();
        property_slave();

        if (!isSlaveAvailable || !isNameAvailable)
            return;

        GUILayout.Space(20);

        property_onActionBegin();
        property_shouldAddToRegistry();
        property_delayBeforeCurrentAction();

        GUILayout.Space(30);

        property_OnActionEnd();
        property_nextCustomAction();
    }


    /// <summary>
    /// Loads default values
    /// </summary>
    private void loadValues()
    {
        if (component == null)
            component = (ActionComponent)target;
        gameObject = component.gameObject;
    }


    private void property_actionName()
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField
            (
                serializedObject.FindProperty("actionData").FindPropertyRelative("actionName"),
                new GUIContent
                (
                    "Name for this action",
                    "The same name will be used while registering to ActionManager"
                )
            );
        if (GUILayout.Button("Default"))
            serializedObject.FindProperty("actionData").FindPropertyRelative("actionName").stringValue = gameObject.name;

        GUILayout.EndHorizontal();

        if (serializedObject.FindProperty("actionData").FindPropertyRelative("actionName").stringValue == "")
        {
            isNameAvailable = false;
            EditorGUILayout.HelpBox("Set action name", MessageType.Error);
        }
        else
            isNameAvailable = true;
    }


    private void property_slave()
    {
        EditorGUILayout.PropertyField
            (
                serializedObject.FindProperty("actionData").FindPropertyRelative("slave"),
                new GUIContent
                (
                    "Action slave"
                )
            );

        ActionSlave slave = (ActionSlave)serializedObject.FindProperty("actionData").FindPropertyRelative("slave").objectReferenceValue;

        if (slave != null)
        {
            isSlaveAvailable = true;
            slave.component = component;
        }
        else
        {
            EditorGUILayout.HelpBox("Action slave object is made by inheriting ActionSlave class", MessageType.Info);
            isSlaveAvailable = false;
        }
    }


    private void property_onActionBegin()
    {
        if (component.isChained)
            serializedObject.FindProperty("actionData").FindPropertyRelative("onActionBegin").enumValueIndex = (int)EOnActionBegin.ExecuteExternally;

        EditorGUI.BeginDisabledGroup(component.isChained);

        EditorGUILayout.PropertyField
            (
                serializedObject.FindProperty("actionData").FindPropertyRelative("onActionBegin"),
                new GUIContent
                (
                    "On Action Begin",
                    "How this action component should work when it's triggered"
                 )
            );
        onActionBegin = (EOnActionBegin)serializedObject.FindProperty("actionData").FindPropertyRelative("onActionBegin").enumValueIndex;

        EditorGUI.EndDisabledGroup();
    }


    private void property_shouldAddToRegistry()
    {
        bool shouldDisable = false;

        if (onActionBegin == EOnActionBegin.ExecuteExternally)
        {
            serializedObject.FindProperty("actionData").FindPropertyRelative("shouldAddToRegistry").boolValue = true;
            shouldDisable = true;
        }

        EditorGUI.BeginDisabledGroup(shouldDisable);

        EditorGUILayout.PropertyField
            (
                serializedObject.FindProperty("actionData").FindPropertyRelative("shouldAddToRegistry"),
                new GUIContent
                (
                    "Should register to ActionManager?",
                    "Set this as true if you need to execute action on this component externally"
                )
            );

        EditorGUI.EndDisabledGroup();
    }


    private void property_delayBeforeCurrentAction()
    {
        if (onActionBegin == EOnActionBegin.ExecuteOnFixedUpdate || onActionBegin == EOnActionBegin.ExecuteOnUpdate)
            return;
        serializedObject.FindProperty("actionData").FindPropertyRelative("delayBeforeCurrentAction").floatValue
            = EditorGUILayout.Slider
                (
                    "Delay before current action",
                    serializedObject.FindProperty("actionData").FindPropertyRelative("delayBeforeCurrentAction").floatValue,
                    0,
                    1000
                );
    }


    private void property_OnActionEnd()
    {
        EditorGUILayout.PropertyField
            (
                serializedObject.FindProperty("actionData").FindPropertyRelative("onActionEnd"),
                new GUIContent
                (
                    "On Action End",
                    "What this action component should do upon completion"
                 )
            );
        onActionEnd = (EOnActionEnd)serializedObject.FindProperty("actionData").FindPropertyRelative("onActionEnd").enumValueIndex;
    }


    private void property_nextCustomAction()
    {
        if (onActionEnd == EOnActionEnd.DoNothing)
        {
            if (cachedComponent != null)
                cachedComponent.isChained = false;
            serializedObject.FindProperty("actionData").FindPropertyRelative("delayBeforeDestroy").floatValue = 0;
            serializedObject.FindProperty("actionData").FindPropertyRelative("nextCustomAction").objectReferenceValue = null;
            serializedObject.FindProperty("actionData").FindPropertyRelative("nextCustomActionName").stringValue = "";
            return;
        }

        if (onActionEnd == EOnActionEnd.DestroySelf)
        {
            GUILayout.Space(2);

            serializedObject.FindProperty("actionData").FindPropertyRelative("delayBeforeDestroy").floatValue
                = EditorGUILayout.Slider
                    (
                        "Delay before destroy",
                        serializedObject.FindProperty("actionData").FindPropertyRelative("delayBeforeDestroy").floatValue,
                        0,
                        1000
                    );
        }
        else if (onActionEnd == EOnActionEnd.ExecuteAnotherAction)
        {
            GUILayout.Space(10);
            selection = GUILayout.Toolbar(selection, toolbars);
            GUILayout.Space(10);

            
            if (toolbars[selection] == "Next action object")
            {
                EditorGUILayout.PropertyField
                    (
                        serializedObject.FindProperty("actionData").FindPropertyRelative("nextCustomAction"),
                        new GUIContent
                        (
                            "Action component reference",
                            "Provide with the name of the action you want to call after execution of current action"
                        )
                    );
            }
            else if (toolbars[selection] == "Next action name")
            {
                EditorGUILayout.PropertyField
                    (
                        serializedObject.FindProperty("actionData").FindPropertyRelative("nextCustomActionName"),
                        new GUIContent
                        (
                            "Next action name",
                            "Provide with the name of the action you want to call after execution of current action"
                        )
                    );
            }

            string actionName = serializedObject.FindProperty("actionData").FindPropertyRelative("nextCustomActionName").stringValue;
            ActionComponent actionComponent = (ActionComponent)serializedObject.FindProperty("actionData").FindPropertyRelative("nextCustomAction").objectReferenceValue;

            if (actionComponent != null)
            {
                cachedComponent = actionComponent;
                actionComponent.isChained = true;
            }
            else if (cachedComponent != null)
                cachedComponent.isChained = false;

            if (actionName == "" && actionComponent == null)
                EditorGUILayout.HelpBox("Both action name and action component cannot be null", MessageType.Error);
        }
    }
}
