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
            space(5);
            heading("-Action Defaults-");
            space(5);


            // Action Name
            propertyField(getProperty("_actionName"), "Name for this action", "This will be the name that will be used to add this component to ActionManager");
            _actionName = getProperty("_actionName").stringValue;
            if (_actionName == "")
                getProperty("_actionName").stringValue = root.gameObject.name;


            // Action slave
            propertyField(getProperty("_actionSlave"), "Slave object", "This is a prefab or scene object which contains custom implementation, this can be modified during runtime as well");
            _actionSlave = (ActionSlave)getProperty("_actionSlave").objectReferenceValue;
            if(_actionSlave == null)
                info("Slave object cannot be null for this component to function properly!!", MessageType.Warning);


            space(10);
            heading("-Action Begin Settings-");
            space(5);


            // OnActionBegin
            bool onAB_shouldDeactivate = false;
            if (root.isChained)
            {
                onAB_shouldDeactivate = true;
                getProperty("_onActionBegin").enumValueIndex = 3;
            }
            EditorGUI.BeginDisabledGroup(onAB_shouldDeactivate);
            if(onAB_shouldDeactivate)
                GUILayout.Label($"This component is chained to {root.chainedComponent.name}");
            propertyField(getProperty("_onActionBegin"), "Action Invoke Mode", "The behaviour of the action itself upon invoke");
            EditorGUI.EndDisabledGroup();
            _onActionBegin = (EOnActionBegin)getProperty("_onActionBegin").enumValueIndex;


            // ShouldRegister
            _shouldRegister = _onActionBegin == EOnActionBegin.ExecuteExternally;
            EditorGUI.BeginDisabledGroup(_shouldRegister);
            if (_shouldRegister)
                getProperty("_shouldRegister").boolValue = _shouldRegister;
            propertyField(getProperty("_shouldRegister"), "Can invoke externally? ", "Set this as true if this action must be invoked externally");
            EditorGUI.EndDisabledGroup();


            // ActionDelay
            bool disableActionDelay = false;
            if (_onActionBegin == EOnActionBegin.ExecuteOnFixedUpdate || _onActionBegin == EOnActionBegin.ExecuteOnUpdate)
            {
                disableActionDelay = true;
                getProperty("_actionDelay").floatValue = 0;
            }
            EditorGUI.BeginDisabledGroup(disableActionDelay);
            propertySlider(getProperty("_actionDelay"), 0, 120, "Delay before current action");
            EditorGUI.EndDisabledGroup();


            space(10);
            heading("-Action End Settings-");
            space(5);


            // OnActionEnd
            propertyField(getProperty("_onActionEnd"), "Action completion routine", "Choose what this component has to do upon completion of current action");
            _onActionEnd = (EOnActionEnd)getProperty("_onActionEnd").enumValueIndex;


            // _nextAction
            if (_onActionEnd == EOnActionEnd.ExecuteAnotherAction)
            {
                string[] toolbars = new string[] { "Next action reference", "Next action name" };
                ActionComponent _cachedNextAction = (ActionComponent)getProperty("_nextAction").objectReferenceValue;
                ActionComponent _nextAction = null;

                space(5);
                selection = GUILayout.Toolbar(selection, toolbars);
                space(5);

                if (selection == 0)
                    propertyField(getProperty("_nextAction"), "Next action component", "Action component that has to be invoked upon completion of this action");
                else
                    propertyField(getProperty("_nextActionName"), "Next action name", "Name of any valid action registered to Action Manager");
                space(5);

                _nextAction = (ActionComponent)getProperty("_nextAction").objectReferenceValue;

                if (_nextAction == root)
                    getProperty("_nextAction").objectReferenceValue = null;
                else if (_nextAction != null)
                {
                    _nextAction.isChained = true;
                    _nextAction.chainedComponent = root;
                }
                else if (_nextAction == null && _cachedNextAction != null)
                    _cachedNextAction.isChained = false;


                if (getProperty("_nextActionName").stringValue == "" && getProperty("_nextAction").objectReferenceValue == null)
                    info("Both values cannot be null! Please initialize atleast one of them!!", MessageType.Error);
            }


            // DestroyDelay
            if (_onActionEnd == EOnActionEnd.DestroySelf)
                propertySlider(getProperty("_destroyDelay"), 0, 120, "Delay before destroy");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            InpectorUpdate();
            serializedObject.ApplyModifiedProperties();
        }
    }
}