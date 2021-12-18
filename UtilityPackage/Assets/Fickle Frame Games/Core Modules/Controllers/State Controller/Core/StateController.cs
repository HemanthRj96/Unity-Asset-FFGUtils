using FickleFrameGames.Controllers.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FickleFrameGames.Controllers
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
        [SerializeField] private string _stateControllerFilepath;
#pragma warning restore 0649, 0414
#endif
        #endregion Editor

        /*.............................................Serialized Fields....................................................*/

        [SerializeField] private StateContainer[] _states = default;
        [SerializeField] private StateSharedData _data = null;
        [SerializeField] private StateSyncInput _input = null;
        [SerializeField] private string _defaultStateName = "";

        /*.............................................Private Fields.......................................................*/

        private Dictionary<string, IState> _stateLookup = new Dictionary<string, IState>();
        private string _currentStateName = "";
        private Action<string> _onStateChangeEvent = delegate { };
        private bool _canUse = false;

        /*.............................................Private Methods......................................................*/

        private void Awake() => bootstrapper();


        /// <summary>
        /// Bootstraps default data
        /// </summary>
        private void bootstrapper()
        {
            foreach (StateContainer state in _states)
                if (state.StateName != "" && state.State != null)
                {
                    _stateLookup.Add(state.StateName, state.State);
                    state.State.ParentController = this;
                    state.State.SharedData = _data;
                }
            if (_data == null || _input == null)
            {
                Debug.LogWarning("Missing data please check the inspector of this gameObject!!");
                _canUse = false;
            }
            _currentStateName = _defaultStateName;
        }


        /// <summary>
        /// Helper method returns an IState object and calls onStateChangeEvent
        /// </summary>
        private IState getActiveState()
        {
            string newStateName = _input.InputUpdate();

            if (_stateLookup.Count == 0 || !_stateLookup.ContainsKey(newStateName) || string.IsNullOrEmpty(_defaultStateName))
                return null;

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
            if (!_canUse)
                return;

            NewUpdate();
            getActiveState()?.OnStateUpdate();
        }


        /// <summary>
        /// Call base method before everything
        /// </summary>
        private void FixedUpdate()
        {
            if (!_canUse)
                return;

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


        /// <summary>
        /// Returns all the states controlled by this StateController
        /// </summary>
        public List<StateContainer> GetStates()
        {
            List<StateContainer> stateContainers = new List<StateContainer>();

            foreach(var temp in _stateLookup.ToList())
            {
                stateContainers.Add(new StateContainer 
                {
                    StateName = temp.Key,
                    State = temp.Value as State
                });
            }
            return stateContainers;
        }
    }
}