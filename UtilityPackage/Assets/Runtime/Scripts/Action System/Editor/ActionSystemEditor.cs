using FickleFrames.Systems.Internal;
using UnityEditor;
using UnityEngine;


namespace FickleFrames.Systems.ActionSystemEditor_
{
    [CustomEditor(typeof(ActionComponent))]
    public class ActionSystemEditor : CustomInspector<ActionComponent>
    {
        string actionName;
        ActionSlave slave;
        EOnActionBegin onActionBegin;
        bool shouldRegister;
        EOnActionEnd onActionEnd;
        int selection = 0;

        private void InpectorUpdate()
        {
            space(5);
            heading("-Action Defaults-");
            space(5);


            // Action Name
            propertyField(getProperty("actionName"), "Name for this action", "This will be the name that will be used to add this component to ActionManager");
            actionName = getProperty("actionName").stringValue;
            if (actionName == "")
                getProperty("actionName").stringValue = root.gameObject.name;


            // Action slave
            propertyField(getProperty("actionSlave"), "Slave object", "This is a prefab or scene object which contains custom implementation, this can be modified during runtime as well");
            slave = (ActionSlave)getProperty("actionSlave").objectReferenceValue;
            if(slave == null)
                info("Slave object cannot be null for this component to function properly!!", MessageType.Warning);


            space(10);
            heading("-Action Begin Settings-");
            space(5);


            // OnActionBegin
            bool onAB_shouldDeactivate = false;
            if (root.isChained)
            {
                onAB_shouldDeactivate = true;
                getProperty("onActionBegin").enumValueIndex = 3;
            }
            EditorGUI.BeginDisabledGroup(onAB_shouldDeactivate);
            if(onAB_shouldDeactivate)
                GUILayout.Label($"This component is chained to {root.chainedComponent.name}");
            propertyField(getProperty("onActionBegin"), "Action Invoke Mode", "The behaviour of the action itself upon invoke");
            EditorGUI.EndDisabledGroup();
            onActionBegin = (EOnActionBegin)getProperty("onActionBegin").enumValueIndex;


            // ShouldRegister
            shouldRegister = onActionBegin == EOnActionBegin.ExecuteExternally;
            EditorGUI.BeginDisabledGroup(shouldRegister);
            if (shouldRegister)
                getProperty("shouldRegister").boolValue = shouldRegister;
            propertyField(getProperty("shouldRegister"), "Can invoke externally? ", "Set this as true if this action must be invoked externally");
            EditorGUI.EndDisabledGroup();


            // ActionDelay
            bool disableActionDelay = false;
            if (onActionBegin == EOnActionBegin.ExecuteOnFixedUpdate || onActionBegin == EOnActionBegin.ExecuteOnUpdate)
            {
                disableActionDelay = true;
                getProperty("actionDelay").floatValue = 0;
            }
            EditorGUI.BeginDisabledGroup(disableActionDelay);
            propertySlider(getProperty("actionDelay"), 0, 120, "Delay before current action");
            EditorGUI.EndDisabledGroup();


            space(10);
            heading("-Action End Settings-");
            space(5);


            // OnActionEnd
            propertyField(getProperty("onActionEnd"), "Action completion routine", "Choose what this component has to do upon completion of current action");
            onActionEnd = (EOnActionEnd)getProperty("onActionEnd").enumValueIndex;


            // nextAction
            if (onActionEnd == EOnActionEnd.ExecuteAnotherAction)
            {
                string[] toolbars = new string[] { "Next action reference", "Next action name" };
                ActionComponent cachedNextAction = (ActionComponent)getProperty("nextAction").objectReferenceValue;
                ActionComponent currentNextAction = null;

                space(5);
                selection = GUILayout.Toolbar(selection, toolbars);
                space(5);

                if (selection == 0)
                    propertyField(getProperty("nextAction"), "Next action component", "Action component that has to be invoked upon completion of this action");
                else
                    propertyField(getProperty("nextActionName"), "Next action name", "Name of any valid action registered to Action Manager");
                space(5);

                currentNextAction = (ActionComponent)getProperty("nextAction").objectReferenceValue;

                if (currentNextAction == root)
                    getProperty("nextAction").objectReferenceValue = null;
                else if (currentNextAction != null)
                {
                    currentNextAction.isChained = true;
                    currentNextAction.chainedComponent = root;
                }
                else if (currentNextAction == null && cachedNextAction != null)
                    cachedNextAction.isChained = false;


                if (getProperty("nextActionName").stringValue == "" && getProperty("nextAction").objectReferenceValue == null)
                    info("Both values cannot be null! Please initialize atleast one of them!!", MessageType.Error);
            }


            // DestroyDelay
            if (onActionEnd == EOnActionEnd.DestroySelf)
                propertySlider(getProperty("destroyDelay"), 0, 120, "Delay before destroy");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            InpectorUpdate();
            serializedObject.ApplyModifiedProperties();
        }
    }
}