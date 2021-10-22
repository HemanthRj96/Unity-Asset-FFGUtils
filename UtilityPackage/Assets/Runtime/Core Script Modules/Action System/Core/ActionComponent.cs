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

        #region Private Fields
#pragma warning disable 0649

        [SerializeField] private string actionName;
        [SerializeField] private ActionSlave actionSlave;
        [SerializeField] private EOnActionBegin onActionBegin;
        [SerializeField] private bool shouldRegister;
        [SerializeField] private float actionDelay;
        [SerializeField] private EOnActionEnd onActionEnd;
        [SerializeField] private string nextActionName;
        [SerializeField] private ActionComponent nextAction;
        [SerializeField] private float destroyDelay;

#pragma warning restore 0649
        #endregion Private Fields

        #region Private Properties

        private IActionParameters passingParams { get; set; } = null;

        #endregion Private Properties

        #region Private Methods

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
            this.DeregisterActionComponent(actionName);
            ActionManager.DeregisterAction(actionName);
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void Start()
        {
            if (onActionBegin == EOnActionBegin.ExecuteOnStart)
                invokeAction();
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void Update()
        {
            if (onActionBegin == EOnActionBegin.ExecuteOnUpdate)
                invokeAction();
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void FixedUpdate()
        {
            if (onActionBegin == EOnActionBegin.ExecuteOnFixedUpdate)
                invokeAction();
        }


        /// <summary>
        /// Bootstraps action data and initializes everything properly
        /// </summary>
        private void bootstrapper()
        {
            this.RegisterActionComponent(actionName);
            if (onActionBegin == EOnActionBegin.ExecuteExternally || shouldRegister)
                ActionManager.RegisterAction(invokeAction, actionName);
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
            if (actionDelay > 0)
                yield return new WaitForSeconds(actionDelay);

            actionSlave?.doAction(parameters);

            if (onActionEnd == EOnActionEnd.ExecuteAnotherAction)
                invokeNext();
            else if (onActionEnd == EOnActionEnd.DestroySelf)
            {
                ActionManager.DeregisterAction(actionName);
                Destroy(gameObject, destroyDelay);
            }
        }


        /// <summary>
        /// Coroutine to invoke next action
        /// </summary>
        private void invokeNext()
        {
            if (passingParams == null)
                passingParams = new ActionParameters();
            if (nextAction != null)
                nextAction.invokeAction(passingParams);
            else
                ActionManager.ExecuteAction(nextActionName, passingParams.data, passingParams.source);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Call this method to set the data that has to passed to a chained action
        /// </summary>
        /// <param name="data">Data to be passed</param>
        public void SetPassingParameters(object data = null, GameObject source = null)
        {
            passingParams = new ActionParameters(data, source);
        }


        /// <summary>
        /// Use this method to change slaves
        /// </summary>
        /// <param name="newSlave">New slave object</param>
        public void UpdateSlave(ActionSlave newSlave)
        {
            actionSlave = newSlave;
        }

        #endregion Protected Methods
    }
}