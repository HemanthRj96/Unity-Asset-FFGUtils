using System;
using UnityEngine;

namespace FickleFrames
{
    /// <summary>
    /// Base class of state, inherit this class to create custom states
    /// </summary>
    public class State : ScriptableObject, IState
    {
        #region Internals

        [Header("-Base State Settings-")]
        [SerializeField] 
        private EStateUpdateMode updateMode = EStateUpdateMode.Update;

        protected Action _stateUpdate = delegate { };
        protected Action _stateFixedUpdate = delegate { };

        #endregion Internals

        public Action onStateUpdate { get { return _stateUpdate; } }
        public Action onStateFixedUpdate { get { return _stateFixedUpdate; } }


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


        /// <summary>
        /// Returns string corresponding to a state
        /// </summary>
        public virtual string GetState() 
        { 
            return default; 
        }


        /// <summary>
        /// State update logic
        /// </summary>
        public virtual void StateUpdate() 
        {
        
        }
    }
}