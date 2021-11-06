using FickleFrames.Systems.Internal;
using System.Collections;
using UnityEngine;


namespace FickleFrames.Systems
{
    /// <summary>
    /// Component class which should be attached to gameObjects that provides multiple ways of running custom codes and 
    /// linking to another action component creating a chain of infinite number of custom actions
    /// </summary>
    public class ActionComponent : MonoBehaviour
    {
        #region Editor
#if UNITY_EDITOR
        public bool isChained = false;
        public ActionComponent chainedComponent = null;
#endif
        #endregion Editor

        #region Internal

        /*.............................................Serialized Fields....................................................*/
#pragma warning disable 0649
        [SerializeField] private string _actionName;
        [SerializeField] private ActionSlave _actionSlave;
        [SerializeField] private EOnActionBegin _onActionBegin;
        [SerializeField] private bool _shouldRegister;
        [SerializeField] private float _actionDelay;
        [SerializeField] private EOnActionEnd _onActionEnd;
        [SerializeField] private string _nextActionName;
        [SerializeField] private ActionComponent _nextAction;
        [SerializeField] private float _destroyDelay;
#pragma warning restore 0649


        /*.............................................Properties...........................................................*/
        private IActionParameters _passingParams { get; set; } = null;


        #region Private Methods

        /*.............................................Private Methods......................................................*/
        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void Awake()
        {
            bootstrapper();
        }


        /// <summary>
        /// Delete action from the ActionManager
        /// </summary>
        private void OnDestroy()
        {
            this.DeregisterActionComponent(_actionName);
            ActionManager.DeregisterAction(_actionName);
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void Start()
        {
            if (_onActionBegin == EOnActionBegin.ExecuteOnStart)
                invokeAction();
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void Update()
        {
            if (_onActionBegin == EOnActionBegin.ExecuteOnUpdate)
                invokeAction();
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void FixedUpdate()
        {
            if (_onActionBegin == EOnActionBegin.ExecuteOnFixedUpdate)
                invokeAction();
        }


        /// <summary>
        /// Bootstraps action data and initializes everything properly
        /// </summary>
        private void bootstrapper()
        {
            this.RegisterActionComponent(_actionName);
            if (_onActionBegin == EOnActionBegin.ExecuteExternally || _shouldRegister)
                ActionManager.RegisterAction(invokeAction, _actionName);
        }


        /// <summary>
        /// This action invokes internal and next action
        /// </summary>
        private void invokeAction(IActionParameters actionParameters = null)
        {
            StartCoroutine(invokeSelf(actionParameters));
        }


        /// <summary>
        /// Coroutine to invoke this gameObject's action
        /// </summary>
        private IEnumerator invokeSelf(IActionParameters parameters)
        {
            if (_actionDelay > 0)
                yield return new WaitForSeconds(_actionDelay);

            _actionSlave?.DoAction(parameters);

            if (_onActionEnd == EOnActionEnd.ExecuteAnotherAction)
                invokeNext();
            else if (_onActionEnd == EOnActionEnd.DestroySelf)
            {
                ActionManager.DeregisterAction(_actionName);
                Destroy(gameObject, _destroyDelay);
            }
        }


        /// <summary>
        /// Coroutine to invoke next action
        /// </summary>
        private void invokeNext()
        {
            if (_passingParams == null)
                _passingParams = new ActionParameters();
            if (_nextAction != null)
                _nextAction.invokeAction(_passingParams);
            else
                ActionManager.ExecuteAction(_nextActionName, _passingParams.Data, _passingParams.Source);
        }

        #endregion Private Methods

        #endregion Internal

        #region Public Methods

        /*.............................................Public Methods.......................................................*/
        /// <summary>
        /// Call this method to set the data that has to passed to a chained action
        /// </summary>
        /// <param name="data">Data to be passed</param>
        public void SetPassingParameters(object data = null, GameObject source = null)
        {
            _passingParams = new ActionParameters(data, source);
        }


        /// <summary>
        /// Use this method to change slaves
        /// </summary>
        /// <param name="newSlave">New slave object</param>
        public void UpdateSlave(ActionSlave newSlave)
        {
            _actionSlave = newSlave;
        }

        #endregion Public
    }
}