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
#pragma warning disable 0649,0414
        [SerializeField] private string stateControllerFilepath;
#pragma warning restore 0649, 0414
#endif
        #endregion Editor

        /*.............................................Serialized Fields....................................................*/

        [SerializeField] private StateContainer[] _states;
        [SerializeField] private string _defaultStateName = "";

        /*.............................................Private Fields.......................................................*/

        private Dictionary<string, IState> _stateLookup = new Dictionary<string, IState>();
        private string _currentStateName = "";
        private Action<string> _onStateChangeEvent = delegate { };

        /*.............................................Private Methods......................................................*/

        private void Awake()
        {
            bootstrapper();
        }


        /// <summary>
        /// Bootstraps default data
        /// </summary>
        private void bootstrapper()
        {
            foreach (StateContainer state in _states)
                if (state.StateName != "" && state.State != null)
                    _stateLookup.Add(state.StateName, state.State);
        }


        /// <summary>
        /// Helper method returns an IState object and calls onStateChangeEvent
        /// </summary>
        private IState getActiveState()
        {
            if (_stateLookup.Count == 0)
                return null;

            // This shouldn't happen
            if (_defaultStateName == "")
                _defaultStateName = _stateLookup.Keys.ToList()[0];
            if (_currentStateName == "")
                _currentStateName = _defaultStateName;

            string newStateName = _stateLookup[_currentStateName]?.GetState();

            if (_currentStateName != newStateName)
                _onStateChangeEvent(newStateName);

            _currentStateName = newStateName;

            return _stateLookup[_currentStateName];
        }


        /// <summary>
        /// Call base method before everything
        /// </summary>
        private void Update()
        {
            NewUpdate();
            getActiveState()?.OnStateUpdate();
        }


        /// <summary>
        /// Call base method before everything
        /// </summary>
        private void FixedUpdate()
        {
            NewFixedUpdate();
            getActiveState()?.OnStateFixedUpdate();
        }

        /*.............................................Protected Methods....................................................*/

        /// <summary>
        /// This method must be implemented since there's no other reason for inheriting this component
        /// </summary>
        protected virtual void NewUpdate() { }

        /// <summary>
        /// This method must be implemented since there's no other reason for inheriting this component
        /// </summary>
        protected virtual void NewFixedUpdate() { }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Use this method to attach and subscribe to stateChangeEvent
        /// </summary>
        /// <param name="targetEvent">The target event</param>
        public void AttachStateChangeEvent(Action<string> targetEvent)
        {
            if (targetEvent != null)
                _onStateChangeEvent += targetEvent;
        }
    }
}