using System;
using System.Collections.Generic;
using UnityEngine;

namespace FickleFrames
{
    /// <summary>
    /// This class should be attached as a component to the target player that has to controlled 
    /// using inputs
    /// </summary>
    public class StateControllerComponent : MonoBehaviour
    {
        #region Internal


        [Header("-State Controller Settings-")]

        [Tooltip("Add the default state i.e., by default active state as the first entry to the list")]
        [SerializeField]
        private List<StateContainer> states = new List<StateContainer>();

        private string defaultStateName = "";
        private string currentStateName = "";
        private IState cachedState = null;
        private Dictionary<string, IState> stateLookup = new Dictionary<string, IState>();
        private Action<string> onStateChangeEvent = delegate { };
        private bool finishedBootstrap = false;

        private bool isReady => finishedBootstrap;

        private void Awake()
        {
            bootstrapper();
        }


        /// <summary>
        /// Bootstraps default data
        /// </summary>
        private void bootstrapper()
        {
            // Check if states are initialized, if yes load lookups
            if (states.Count == 0)
            {
                Debug.LogError("NO STATES AVAILABLE!!");
                return;
            }
            // Initialize the default state
            defaultStateName = states[0].stateName;

            // Initialize stateLookup
            foreach (var state in states)
                stateLookup.Add(state.stateName, state.state);

            // Load default state
            cachedState = stateLookup[defaultStateName];
            finishedBootstrap = true;
        }


        /// <summary>
        /// Helper method returns an IState object and calls onStateChangeEvent
        /// </summary>
        private IState getActiveState()
        {
            string newStateName;
            // Change current state to default state if current state is empty
            if (currentStateName == "")
                currentStateName = defaultStateName;

            // Get the new state from stateLookup
            newStateName = stateLookup[currentStateName].GetState();

            // Call onStateChangeEvent if currentState is not equal to new state
            if (currentStateName != newStateName)
                onStateChangeEvent(newStateName);

            // Assign currentState with newState
            currentStateName = newStateName;

            return stateLookup[currentStateName];
        }


        #endregion Internal


        /// <summary>
        /// Call base method before everything
        /// </summary>
        protected void Update()
        {
            if (!isReady)
                return;
            cachedState = getActiveState();
            cachedState?.onStateUpdate();
        }


        /// <summary>
        /// Call base method before everything
        /// </summary>
        protected void FixedUpdate()
        {
            if (!isReady)
                return;
            cachedState?.onStateFixedUpdate();
        }


        /// <summary>
        /// Returns the current state
        /// </summary>
        public string GetCurrentState()
        {
            return currentStateName;
        }


        /// <summary>
        /// Use this method to attach and subscribe to stateChangeEvent
        /// </summary>
        /// <param name="targetEvent">The target event</param>
        public void AttachStateChangeEvent(Action<string> targetEvent)
        {
            if (targetEvent != null)
                onStateChangeEvent += targetEvent;
        }
    }
}