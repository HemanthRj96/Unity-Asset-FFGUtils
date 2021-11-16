using FickleFrameGames.Systems;
using FickleFrameGames.Systems.Internal;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(ActionSettings))]
public class ActionSettingsDrawer : PropertyDrawer
{
    float newHeight = 0;
    readonly float singleLine = 22;
    string _actionNameCache;
    EActionExecutionMode _onActionBeginCache;

    SerializedProperty _actionName;
    SerializedProperty _onActionBegin;
    SerializedProperty _shouldRegister;
    SerializedProperty _actionDelay;
    SerializedProperty _onActionEnd;
    SerializedProperty _nextActionName;
    SerializedProperty _nextAction;
    SerializedProperty _destroyDelay;
    SerializedProperty _selection;

    enum Selection
    {
        Name,
        ObjectReference
    }
    Selection selection = Selection.Name;


    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(rect, label, property);

        var target = property.serializedObject.targetObject;
        var root = (ActionComponent)target;
        rect.height = 16;

        // Initialize serialized objects
        _actionName = getPropertyRelative(property, "ActionName");
        _onActionBegin = getPropertyRelative(property, "ActionExecutionMode");
        _shouldRegister = getPropertyRelative(property, "ShouldRegister");
        _actionDelay = getPropertyRelative(property, "ActionDelay");
        _onActionEnd = getPropertyRelative(property, "OnActionExecutionEnd");
        _nextActionName = getPropertyRelative(property, "NextActionName");
        _nextAction = getPropertyRelative(property, "NextAction");
        _destroyDelay = getPropertyRelative(property, "DestroyDelay");
        _selection = getPropertyRelative(property, "selection");

        nextLine(ref rect);
        heading(rect, "Action On Begin Settings");
        space(ref rect, 30);

        #region action name

        propertyField(rect, _actionName, "Name for this action", "This will be the name that will be used to add this component to ActionManager");
        _actionNameCache = _actionName.stringValue;
        if (_actionNameCache == "")
            _actionName.stringValue = root.gameObject.name;

        #endregion

        #region onActionBegin

        bool disable_onActionBegin = false;

        if (root.isChained == true)
        {
            disable_onActionBegin = true;
            _onActionBegin.enumValueIndex = (int)EActionExecutionMode.ExecuteExternally;
        }
        if (disable_onActionBegin)
        {
            nextLine(ref rect);
            EditorGUI.LabelField(rect, $"This component is chained to : ", $"{root.chainedComponent.name}");
        }

        _onActionBeginCache = (EActionExecutionMode)_onActionBegin.enumValueIndex;
        nextLine(ref rect);

        EditorGUI.BeginDisabledGroup(disable_onActionBegin);
        propertyField(rect, _onActionBegin, "Action Invoke Mode", "The behaviour of the action itself upon invoke");
        EditorGUI.EndDisabledGroup();

        #endregion

        #region shouldRegister

        nextLine(ref rect);
        bool _disableShouldRegister = true;

        switch (_onActionBeginCache)
        {
            case EActionExecutionMode.ExecuteOnStart:
                _disableShouldRegister = false;
                break;
            case EActionExecutionMode.ExecuteOnUpdate:
            case EActionExecutionMode.ExecuteOnFixedUpdate:
                _shouldRegister.boolValue = false;
                break;
            case EActionExecutionMode.ExecuteExternally:
                _shouldRegister.boolValue = true;
                break;
        }

        EditorGUI.BeginDisabledGroup(_disableShouldRegister);
        propertyField(rect, _shouldRegister, "Can invoke externally? ", "Set this as true if this action must be invoked externally");
        EditorGUI.EndDisabledGroup();

        #endregion

        #region actionDelay

        nextLine(ref rect);
        bool disableActionDelay = false;

        if (_onActionBeginCache == EActionExecutionMode.ExecuteOnFixedUpdate || _onActionBeginCache == EActionExecutionMode.ExecuteOnUpdate)
        {
            disableActionDelay = true;
            _actionDelay.floatValue = 0;
        }

        EditorGUI.BeginDisabledGroup(disableActionDelay);
        _actionDelay.floatValue = EditorGUI.Slider(rect, new GUIContent("Delay before current action", ""), _actionDelay.floatValue, 0, 120);
        EditorGUI.EndDisabledGroup();

        #endregion

        nextLine(ref rect);
        nextLine(ref rect);
        heading(rect, "Action On End Settings");
        space(ref rect, 8);

        #region onActionEnd

        nextLine(ref rect);
        propertyField(rect, _onActionEnd, "Action completion routine", "Choose what this component has to do upon completion of current action");
        EOnActionExecutionEnd _onActionEndCache = (EOnActionExecutionEnd)_onActionEnd.enumValueIndex;

        #endregion

        #region nextActionName

        if (_onActionEndCache == EOnActionExecutionEnd.ExecuteAnotherAction)
        {
            nextLine(ref rect);
            GUIStyle style = EditorStyles.popup;
            string _nextActionNameCache = default;

            ActionComponent _cachedNextAction = (ActionComponent)_nextAction.objectReferenceValue;
            ActionComponent _nextActionCache = null;

            selection = (Selection)EditorGUI.EnumPopup(rect, (Selection)_selection.intValue, style);
            _selection.intValue = (int)selection;

            nextLine(ref rect);
            switch (selection)
            {
                case Selection.Name:
                    propertyField(rect, _nextActionName, "Next action name", "Name of any valid action registered to Action Manager");
                    ActionComponent _cached = (ActionComponent)_nextAction.objectReferenceValue;
                    if (_cached != null)
                    {
                        _cached.isChained = false;
                        _cached.chainedComponent = null;
                    }
                    _nextAction.objectReferenceValue = null;
                    _nextActionNameCache = _nextActionName.stringValue;
                    break;
                case Selection.ObjectReference:
                    propertyField(rect, _nextAction, "Next action component", "Action component that has to be invoked upon completion of this action");
                    _nextActionName.stringValue = null;
                    _nextActionCache = (ActionComponent)_nextAction.objectReferenceValue;

                    if (root.Equals(_nextActionCache))
                        _nextAction.objectReferenceValue = null;
                    else if (_nextActionCache != null)
                    {
                        if (_nextActionCache.actionExecutionMode == EActionExecutionMode.ExecuteOnFixedUpdate || _nextActionCache.actionExecutionMode == EActionExecutionMode.ExecuteOnUpdate)
                        {
                            _nextActionCache.isChained = false;
                            _nextActionCache.chainedComponent = null;
                            _nextAction.objectReferenceValue = null;
                        }
                        else
                        {
                            _nextActionCache.isChained = true;
                            _nextActionCache.chainedComponent = root;
                        }
                    }
                    else if (_nextActionCache == null && _cachedNextAction != null)
                    {
                        _cachedNextAction.isChained = false;
                        _cachedNextAction.chainedComponent = null;
                    }
                    break;
            }
        }
        else if (_onActionEndCache == EOnActionExecutionEnd.DestroySelf)
        {
            ActionComponent _cached = (ActionComponent)_nextAction.objectReferenceValue;
            if (_cached != null)
            {
                _cached.isChained = false;
                _cached.chainedComponent = null;
            }
            _nextAction.objectReferenceValue = null;
            _nextActionName.stringValue = null;

            nextLine(ref rect);
            _destroyDelay.floatValue = EditorGUI.Slider(rect, new GUIContent("Delay before self destruction", ""), _destroyDelay.floatValue, 0, 120);
        }
        else
        {
            ActionComponent _cached = (ActionComponent)_nextAction.objectReferenceValue;
            if (_cached != null)
            {
                _cached.isChained = false;
                _cached.chainedComponent = null;
            }
            _nextAction.objectReferenceValue = null;
            _nextActionName.stringValue = null;
        }

        #endregion

        space(ref rect, 25);
        EditorGUI.LabelField(rect, "", GUI.skin.horizontalSlider);

        newHeight = rect.y + 5;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return newHeight;
    }

    private void propertyField(Rect rect, SerializedProperty property, string name = "", string tooltip = "")
    {
        EditorGUI.PropertyField(rect, property, new GUIContent(name, tooltip));
    }

    private SerializedProperty getPropertyRelative(SerializedProperty property, string name)
    {
        return property.FindPropertyRelative(name);
    }

    private void space(ref Rect rect, float amount)
    {
        rect.y += amount;
    }

    private void nextLine(ref Rect rect)
    {
        rect.y += singleLine;
    }

    private void heading(Rect rect, string label)
    {
        var style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        EditorGUI.LabelField(rect, new GUIContent(label), style);
    }
}
