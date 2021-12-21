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
    public sealed class StateController : MonoBehaviour
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
        private bool _canUse = true;
        private Action<string> _onStateChangeEvent = delegate { };
        private Action _stateAwakeInvoker = delegate { };
        private Action _stateStartInvoker = delegate { };

        /*.............................................Private Methods......................................................*/


        /// <summary>
        /// Initializes and invokes StateAwake in correct order
        /// </summary>
        private void Awake()
        {
            bootstrapper();
            awakers();
        }


        /// <summary>
        /// Invoke StateStart in correct order
        /// </summary>
        private void Start()
        {
            starters();
        }


        /// <summary>
        /// Call base method before everything
        /// </summary>
        private void Update()
        {
            if (!_canUse)
                return;
            getActiveState()?.OnStateUpdate();
        }


        /// <summary>
        /// Call base method before everything
        /// </summary>
        private void FixedUpdate()
        {
            if (!_canUse)
                return;
            getActiveState()?.OnStateFixedUpdate();
        }


        /// <summary>
        /// Bootstraps default data
        /// </summary>
        private void bootstrapper()
        {
            if (_data == null || _input == null || String.IsNullOrEmpty(_defaultStateName))
            {
                Debug.LogWarning("Missing data please check the inspector of this gameObject!!");
                _canUse = false;
                return;
            }
            else
            {
                _data.ParentController = this;
                _input.ParentController = this;
            }

            foreach (StateContainer s in _states)
                if (s.StateName != "" && s.State != null)
                {
                    _stateLookup.Add(s.StateName, s.State);
                    s.State.ParentController = this;
                    s.State.SharedData = _data;
                    _stateAwakeInvoker += s.State.StateAwake;
                    _stateStartInvoker += s.State.StateStart;
                }

            _currentStateName = _defaultStateName;

            _data.StateAwake();
            _input.StateAwake();
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
        /// Helper method to call StateAwake in correct order
        /// </summary>
        private void awakers()
        {
            _data.StateAwake();
            _input.StateAwake();
            _stateAwakeInvoker();
        }


        /// <summary>
        /// Helper method to call StateStart in correct order
        /// </summary>
        private void starters()
        {
            _data.StateStart();
            _input.StateStart();
            _stateStartInvoker();
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Use this method to attach an action which invokes eerytime when state changes
        /// </summary>
        /// <param name="targetEvent">The target event</param>
        public void SubscribeToStateChangeEvent(Action<string> targetEvent)
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