using System;
using UnityEngine;

namespace FickleFrames.StateMachine
{
    /// <summary>
    /// Update this class only if you're adding custom states
    /// </summary>
    public class StateController
    {
        public StateController(SettingsContainer settings)
        {
            this.settings = settings;
            initializeStates();
        }

        // State settings
        private SettingsContainer settings;
        // Cached state
        private I_State activeState;


        //##ALL THE CUSTOM STATE OBJECTS SHOULD BE ADDED HERE##
        private RunState runState;
        private IdleState idleState;
        private WalkState walkState;
        private JumpState jumpState;
        //##ALL THE CUSTOM STATE OBJECTS SHOULD BE ADDED HERE##

        /// <summary>
        /// Returns the active state
        /// </summary>
        public I_State getActiveState()
        {
            string stateType = activeState.GetType().ToString();
            updateActiveState();

            switch (stateType)
            {
                //##IF YOU'RE ADDING A NEW CUSTOM STATE THEN YOU HAVE TO ADD IT HERE##
                case "RunState":
                    return runState;
                case "IdleState":
                    return idleState;
                case "WalkState":
                    return walkState;
                case "JumpState":
                    return jumpState;
                //##IF YOU'RE ADDING A NEW CUSTOM STATE THEN YOU HAVE TO ADD IT HERE##
                default:
                    return null;
            }
        }

        /// <summary>
        /// This method runs upon constructor invoke
        /// </summary>
        private void initializeStates()
        {
            //##ALL THE CUSTOM STATE OBJECTS SHOULD ALSO BE CONSTRUCTED WITH SETTINGS FROM STATESETTINGS##
            runState = new RunState(settings.runSettings);
            idleState = new IdleState(settings.idleSettings);
            walkState = new WalkState(settings.walkSettings);
            jumpState = new JumpState(settings.jumpSettings);
            //##ALL THE CUSTOM STATE OBJECTS SHOULD ALSO BE CONSTRUCTED WITH SETTINGS FROM STATESETTINGS##

            //##SET THE DEFAULT ACTIVE STATE HERE##
            activeState = idleState;
            //##SET THE DEFAULT ACTIVE STATE HERE##
        }

        /// <summary>
        /// Method to update the active state
        /// </summary>
        private void updateActiveState() => activeState = activeState.getState();
    }
}