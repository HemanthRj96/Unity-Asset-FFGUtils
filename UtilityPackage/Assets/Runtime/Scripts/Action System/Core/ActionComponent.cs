using FickleFrames.ActionSystem.Internal;
using System.Collections;
using UnityEngine;


namespace FickleFrames.ActionSystem
{
    public sealed class ActionComponent : MonoBehaviour
    {
        #region Editor
#if UNITY_EDITOR


        [SerializeField] private bool isChained = false;

#endif
        #endregion Editor

        #region Private Fields

        [SerializeField] private string actionName;
        [SerializeField] private ActionSlave slave;
        [SerializeField] private EOnActionBegin onActionBegin;
        [SerializeField] private bool shouldRegister;
        [SerializeField] private float actionDelay;
        [SerializeField] private EOnActionEnd onActionEnd;
        [SerializeField] private string nextActionName;
        [SerializeField] private ActionComponent nextAction;
        [SerializeField] private float destroyDelay;

        #endregion Private Fields

        #region Private Properties

        private IActionParameters passingParams { get; set; } = null;

        #endregion Private Properties

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
        /// Returns the slave controlled by this Component
        /// </summary>
        public ActionSlave GetSlave()
        {
            return slave;
        }

        #endregion Public Methods

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
            ActionManager.DeleteAction(actionName);
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

            slave.doAction(parameters);

            if (onActionEnd == EOnActionEnd.ExecuteAnotherAction)
                invokeNext();
            else if (onActionEnd == EOnActionEnd.DestroySelf)
            {
                ActionManager.DeleteAction(actionName);
                Destroy(gameObject, destroyDelay);
            }
        }


        /// <summary>
        /// Coroutine to invoke next action
        /// </summary>
        private void invokeNext()
        {
            if (nextAction != null)
                nextAction.invokeAction(passingParams);
            else
                ActionManager.ExecuteAction(nextActionName, passingParams.data, passingParams.source);
        }

        #endregion Private Methods
    }
}