using System;
using UnityEngine;

namespace FickleFrames.StateMachine
{
    /// <summary>
    /// This class should be attached as a component to the target player that has to controlled 
    /// using inputs
    /// </summary>
    public class StateManager : MonoBehaviour
    {
        [Header("Custom State Data")]
        public StateSettingsContainer stateSettings = null;
        [SerializeField]
        private E_StateUpdateMode updateMode;
        [SerializeField]
        private bool canUpdate = true;

        private Action callOnUpdate = delegate { };
        private Action callOnFixedUpdate = delegate { };
        private StateController controller;
        private I_State cachedState = null;

        /// <summary>
        /// To resume the state update
        /// </summary>
        public void ResumeStateUpdate() { canUpdate = true; }

        /// <summary>
        /// To stop state update
        /// </summary>
        public void StopStateUpdate() { canUpdate = false; }


        private void Awake()
        {
            // Check if the settings are initialized
            if (stateSettings == null)
            {
                Debug.LogError("STATE SETTINGS ARE NOT INITIALIZED!!");
                return;
            }
            // Initialize the controller with state settings
            controller = new StateController(stateSettings.settings);
            // Set the type of state update 
            if (updateMode == E_StateUpdateMode.Update)
                callOnUpdate = invokeHelper;
            else
                callOnFixedUpdate = invokeHelper;
        }

        private void Update()
        {
            if (canUpdate && cachedState != null)
            {
                cachedState = controller.getActiveState();
                callOnUpdate.Invoke();
            }
        }

        private void FixedUpdate()
        {
            if (canUpdate && cachedState != null)
                callOnFixedUpdate.Invoke();
        }

        /// <summary>
        /// Helper method
        /// </summary>
        private void invokeHelper() => cachedState.updateState();
    }
}
