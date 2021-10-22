using FickleFrames.Controllers.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FickleFrames.Controllers
{
    /// <summary>
    /// This class should be attached as a component to the target player that has to controlled 
    /// using inputs
    /// </summary>
    public class StateController : MonoBehaviour
    {
        #region Editor
#if UNITY_EDITOR
        [SerializeField] private string stateControllerFilepath;
        [SerializeField] private string scriptSuffix;
#endif
        #endregion Editor

        #region Private Fields

        [SerializeField] private StateContainer[] states;
        [SerializeField] private string defaultStateName = "";

        private Dictionary<string, IState> stateLookup = new Dictionary<string, IState>();
        private string currentStateName = "";
        private Action<string> onStateChangeEvent = delegate { };

        #endregion Private Fields

        #region Private Methods

        private void Awake()
        {
            bootstrapper();
        }


        /// <summary>
        /// Bootstraps default data
        /// </summary>
        private void bootstrapper()
        {
            foreach (StateContainer state in states)
                if (state.stateName != "" && state.state != null)
                    stateLookup.Add(state.stateName, state.state);
        }


        /// <summary>
        /// Helper method returns an IState object and calls onStateChangeEvent
        /// </summary>
        private IState getActiveState()
        {
            if (stateLookup.Count == 0)
                return null;

            // This shouldn't happen
            if (defaultStateName == "")
                defaultStateName = stateLookup.Keys.ToList()[0];
            if (currentStateName == "")
                currentStateName = defaultStateName;

            string newStateName = stateLookup[currentStateName]?.GetState();

            if (currentStateName != newStateName)
                onStateChangeEvent(newStateName);

            currentStateName = newStateName;

            return stateLookup[currentStateName];
        }


        /// <summary>
        /// Call base method before everything
        /// </summary>
        private void Update()
        {
            _Update();
            getActiveState()?.onStateUpdate();
        }


        /// <summary>
        /// Call base method before everything
        /// </summary>
        private void FixedUpdate()
        {
            _FixedUpdate();
            getActiveState()?.onStateFixedUpdate();
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// This method must be implemented since there's no other reason for inheriting this component
        /// </summary>
        protected virtual void _Update() { }

        /// <summary>
        /// This method must be implemented since there's no other reason for inheriting this component
        /// </summary>
        protected virtual void _FixedUpdate() { }

        #endregion Protected Methods

        #region Public Methods

        /// <summary>
        /// Use this method to attach and subscribe to stateChangeEvent
        /// </summary>
        /// <param name="targetEvent">The target event</param>
        public void AttachStateChangeEvent(Action<string> targetEvent)
        {
            if (targetEvent != null)
                onStateChangeEvent += targetEvent;
        }

        #endregion Public Methods
    }
}