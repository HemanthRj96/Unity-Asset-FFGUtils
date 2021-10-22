using FickleFrames.Controllers.Internals;
using System;
using UnityEngine;


namespace FickleFrames.Controllers
{
    /// <summary>
    /// Base class of state, inherit this class to create custom states
    /// </summary>
    public abstract class State : ScriptableObject, IState
    {
        #region Private Fields

        [Header("-Base State Settings-")]
        [SerializeField] private EStateUpdateMode updateMode = EStateUpdateMode.Update;

        private Action _stateUpdate = delegate { };
        private Action _stateFixedUpdate = delegate { };

        #endregion Private Fields

        #region Public Properties

        public Action onStateUpdate { get { return _stateUpdate; } }
        public Action onStateFixedUpdate { get { return _stateFixedUpdate; } }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Change the way how the state will be updated
        /// </summary>
        private void OnEnable()
        {
            if (updateMode == EStateUpdateMode.Update)
                _stateUpdate = StateUpdate;
            else
                _stateFixedUpdate = StateUpdate;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Returns string corresponding to a state
        /// </summary>
        public abstract string GetState();


        /// <summary>
        /// State update logic
        /// </summary>
        public abstract void StateUpdate();

        #endregion Public Methods
    }
}

