using FickleFrames.Systems.Internal;
using UnityEditor;
using UnityEngine;


namespace FickleFrames.Systems.Editor_
{
    [CustomEditor(typeof(ActionComponent))]
    public class ActionComponentEditor : BaseEditor<ActionComponent>
    {
        string _actionName;
        ActionSlave _actionSlave;
        EOnActionBegin _onActionBegin;
        bool _shouldRegister;
        EOnActionEnd _onActionEnd;
        int selection = 0;

        private void InpectorUpdate()
        {
            Space(5);
            Heading("-Action Defaults-");
            Space(5);


            // Action Name
            PropertyField(GetProperty("_actionName"), "Name for this action", "This will be the name that will be used to add this component to ActionManager");
            _actionName = GetProperty("_actionName").stringValue;
            if (_actionName == "")
                GetProperty("_actionName").stringValue = Root.gameObject.name;


            // Action slave
            PropertyField(GetProperty("_actionSlave"), "Slave object", "This is a prefab or scene object which contains custom implementation, this can be modified during runtime as well");
            _actionSlave = (ActionSlave)GetProperty("_actionSlave").objectReferenceValue;
            if(_actionSlave == null)
                Info("Slave object cannot be null for this component to function properly!!", MessageType.Warning);


            Space(10);
            Heading("-Action Begin Settings-");
            Space(5);


            // OnActionBegin
            bool onAB_shouldDeactivate = false;
            if (Root.isChained)
            {
                onAB_shouldDeactivate = true;
                GetProperty("_onActionBegin").enumValueIndex = 3;
            }
            EditorGUI.BeginDisabledGroup(onAB_shouldDeactivate);
            if(onAB_shouldDeactivate)
                GUILayout.Label($"This component is chained to {Root.chainedComponent.name}");
            PropertyField(GetProperty("_onActionBegin"), "Action Invoke Mode", "The behaviour of the action itself upon invoke");
            EditorGUI.EndDisabledGroup();
            _onActionBegin = (EOnActionBegin)GetProperty("_onActionBegin").enumValueIndex;


            // ShouldRegister
            _shouldRegister = _onActionBegin == EOnActionBegin.ExecuteExternally;
            EditorGUI.BeginDisabledGroup(_shouldRegister);
            if (_shouldRegister)
                GetProperty("_shouldRegister").boolValue = _shouldRegister;
            PropertyField(GetProperty("_shouldRegister"), "Can invoke externally? ", "Set this as true if this action must be invoked externally");
            EditorGUI.EndDisabledGroup();


            // ActionDelay
            bool disableActionDelay = false;
            if (_onActionBegin == EOnActionBegin.ExecuteOnFixedUpdate || _onActionBegin == EOnActionBegin.ExecuteOnUpdate)
            {
                disableActionDelay = true;
                GetProperty("_actionDelay").floatValue = 0;
            }
            EditorGUI.BeginDisabledGroup(disableActionDelay);
            PropertySlider(GetProperty("_actionDelay"), 0, 120, "Delay before current action");
            EditorGUI.EndDisabledGroup();


            Space(10);
            Heading("-Action End Settings-");
            Space(5);


            // OnActionEnd
            PropertyField(GetProperty("_onActionEnd"), "Action completion routine", "Choose what this component has to do upon completion of current action");
            _onActionEnd = (EOnActionEnd)GetProperty("_onActionEnd").enumValueIndex;


            // _nextAction
            if (_onActionEnd == EOnActionEnd.ExecuteAnotherAction)
            {
                string[] toolbars = new string[] { "Next action reference", "Next action name" };
                ActionComponent _cachedNextAction = (ActionComponent)GetProperty("_nextAction").objectReferenceValue;
                ActionComponent _nextAction = null;

                Space(5);
                selection = GUILayout.Toolbar(selection, toolbars);
                Space(5);

                if (selection == 0)
                    PropertyField(GetProperty("_nextAction"), "Next action component", "Action component that has to be invoked upon completion of this action");
                else
                    PropertyField(GetProperty("_nextActionName"), "Next action name", "Name of any valid action registered to Action Manager");
                Space(5);

                _nextAction = (ActionComponent)GetProperty("_nextAction").objectReferenceValue;

                if (_nextAction == Root)
                    GetProperty("_nextAction").objectReferenceValue = null;
                else if (_nextAction != null)
                {
                    _nextAction.isChained = true;
                    _nextAction.chainedComponent = Root;
                }
                else if (_nextAction == null && _cachedNextAction != null)
                    _cachedNextAction.isChained = false;


                if (GetProperty("_nextActionName").stringValue == "" && GetProperty("_nextAction").objectReferenceValue == null)
                    Info("Both values cannot be null! Please initialize atleast one of them!!", MessageType.Error);
            }


            // DestroyDelay
            if (_onActionEnd == EOnActionEnd.DestroySelf)
                PropertySlider(GetProperty("_destroyDelay"), 0, 120, "Delay before destroy");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            InpectorUpdate();
            serializedObject.ApplyModifiedProperties();
        }
    }
}