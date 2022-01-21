using FFG.Controllers.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FFG.Controllers
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

        /*.............................................Private Fields.......................................................*/

        private Dictionary<string, IState> _stateLookup = new Dictionary<string, IState>();

        private string _activeState = null;

        private Action<string> _onStateChangeEvent = delegate { };

        private Action _stateAwakeInvoker = delegate { };
        private Action _stateStartInvoker = delegate { };

        /*.............................................Private Methods......................................................*/


        /// <summary>
        /// Initializes and invokes StateAwake in correct order
        /// </summary>
        private void Awake() => bootstrapper();


        /// <summary>
        /// Invoke StateStart in correct order
        /// </summary>
        private void Start() => starters();


        /// <summary>
        /// Call base method before everything
        /// </summary>
        private void Update() => getState()?.OnStateUpdate();


        /// <summary>
        /// Call base method before everything
        /// </summary>
        private void FixedUpdate() => getState()?.OnStateFixedUpdate();


        /// <summary>
        /// Bootstraps default data
        /// </summary>
        private void bootstrapper()
        {
            if (_data == null || _input == null)
            {
                Debug.LogWarning("Missing values SharedData/SyncInput, check inspector!! Initializing with default values!!");
                _data = ScriptableObject.CreateInstance<StateSharedData>();
                _input = ScriptableObject.CreateInstance<StateSyncInput>();
            }

            _data.ParentController = this;
            _input.ParentController = this;

            foreach (StateContainer s in _states)
                if (s.StateName != "" && s.State != null)
                {
                    _stateLookup.Add(s.StateName, s.State);

                    s.State.ParentController = this;
                    s.State.SharedData = _data;

                    _stateAwakeInvoker += s.State.StateAwake;
                    _stateStartInvoker += s.State.StateStart;
                }

            _data.StateAwake();
            _input.StateAwake();

            awakers();
        }


        /// <summary>
        /// Helper method returns an IState object and calls onStateChangeEvent
        /// </summary>
        private IState getState()
        {
            string newState = _input.InputUpdate();

            if (string.IsNullOrEmpty(newState) || !_stateLookup.ContainsKey(newState))
                return null;

            if (newState != _activeState)
                _onStateChangeEvent.Invoke(newState);

            _activeState = newState;
            
            return _stateLookup[_activeState];
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
        public StateContainer[] GetStates()
        {
            List<StateContainer> stateContainers = new List<StateContainer>();

            foreach (var temp in _stateLookup.ToList())
            {
                stateContainers.Add(new StateContainer
                {
                    StateName = temp.Key,
                    State = temp.Value as State
                });
            }
            return stateContainers.ToArray();
        }


        /// <summary>
        /// Method to change state synchronized input on runtime
        /// </summary>
        /// <param name="newInput">Target new sync input</param>
        public void ChangeSynchronizedInput(StateSyncInput newInput)
        {
            if (newInput == null)
                return;
            else if (newInput.ParentController == null)
            {
                newInput.ParentController = this;
                _input = newInput;
                newInput.StateAwake();
                newInput.StateStart();
            }
            else if (newInput.ParentController != null && newInput.ParentController == this)
                _input = newInput;
            else
                return;
        }
    }
}