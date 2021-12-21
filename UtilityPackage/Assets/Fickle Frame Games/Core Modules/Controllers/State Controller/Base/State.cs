using FickleFrameGames.Controllers.Internals;
using System;
using UnityEngine;


namespace FickleFrameGames.Controllers
{
    /// <summary>
    /// Base class of state, inherit this class to create custom states
    /// </summary>
    public abstract class State : ScriptableObject, IState
    {
        /*.............................................Serialized Fields....................................................*/

        [Header("Native State Settings")]
        [SerializeField] private EStateUpdateMode _updateMode = EStateUpdateMode.Update;

        /*.............................................Private Fields.......................................................*/

        private Action _stateUpdate = delegate { };
        private Action _stateFixedUpdate = delegate { };
        private StateController _stateController = null;
        private StateSharedData _data = null;

        /*.............................................Properties...........................................................*/

        public StateController ParentController
        {
            get { return _stateController; }
            set { if (_stateController == null) _stateController = value; }
        }

        public StateSharedData SharedData
        {
            get { return _data; }
            set { if (_data == null) _data = value; }
        }

        public Action OnStateUpdate { get { return _stateUpdate; } }

        public Action OnStateFixedUpdate { get { return _stateFixedUpdate; } }

        /*.............................................Private Methods......................................................*/

        /// <summary>
        /// Change the way how the state will be updated
        /// </summary>
        private void OnEnable()
        {
            if (_updateMode == EStateUpdateMode.Update)
                _stateUpdate = StateUpdate;
            else
                _stateFixedUpdate = StateUpdate;
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Override this method to perform any operations upon awake
        /// </summary>
        public virtual void StateAwake() { }


        /// <summary>
        /// Override this method to perfom any operations upon start
        /// </summary>
        public virtual void StateStart() { }


        /// <summary>
        /// State update logic
        /// </summary>
        public abstract void StateUpdate();
    }
}

