using FickleFrames.ActionSystem.Internal;
using FickleFrames.ActionSystem;
using UnityEditor;
using UnityEngine;
using System;

namespace FickleFrames.ActionSystem.Editor_
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


        private void update()
        {
            label("-Action Defaults-");
            space(5);
            // Action Name
            propertyField(getProperty("actionName"), "Name for this action", "This will be the name that will be used to add this component to ActionManager");
            actionName = getProperty("actionName").stringValue;
            if (actionName == "")
                getProperty("actionName").stringValue = gameobject.name;

            // Slave
            propertyField(getProperty("slave"), "Slave object", "This object will have all the code necessary to perform a custom action");
            slave = (ActionSlave)getProperty("slave").objectReferenceValue;
            if (slave == null)
            {
                info("Slave object cannot be null for this component to work properly!");
                return;
            }
            space(10);

            label("-Action Begin Settings-");
            space(5);
            // OnActionBegin
            propertyField(getProperty("onActionBegin"), "Action Invoke Mode", "The behaviour of the action itself upon invoke");
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

            label("-Action End Settings-");
            space(5);
            // OnActionEnd
            propertyField(getProperty("onActionEnd"), "Action completion routine", "Choose what this component has to do upon completion of current action");
            onActionEnd = (EOnActionEnd)getProperty("onActionEnd").enumValueIndex;

            // nextAction
            if (onActionEnd == EOnActionEnd.ExecuteAnotherAction)
            {
                space(5);
                string[] toolbars = new string[] { "Next action reference", "Next action name" };
                selection = GUILayout.Toolbar(selection, toolbars);
                space(5);
                if (selection == 0)
                    propertyField(getProperty("nextAction"), "Next action component", "Action component that has to be invoked upon completion of this action");
                else
                    propertyField(getProperty("nextActionName"), "Next action name", "Name of any valid action registered to Action Manager");
                space(5);
                if (getProperty("nextAction").stringValue == "" && getProperty("nextActionName").objectReferenceValue == null)
                    info("Both values cannot be null! Please initialize atleast one of them!!", MessageType.Error);
            }

            // DestroyDelay
            if (onActionEnd == EOnActionEnd.DestroySelf)
                propertySlider(getProperty("destroyDelay"), 0, 120, "Delay before destroy");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            update();
            serializedObject.ApplyModifiedProperties();
        }
    }
}