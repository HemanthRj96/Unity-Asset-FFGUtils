using System.Collections;
using UnityEngine;

namespace FickleFrames
{
    public sealed class ActionComponent : MonoBehaviour
    {
        #region Editor Only

#if UNITY_EDITOR

        public bool isChained = false;

#endif

        #endregion


        #region Internals


        [SerializeField] private ActionDataStruct actionData;

        private IActionParameters passingParams { get; set; } = null;


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
            ActionManager.DeleteAction(actionData.actionName);
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void Start()
        {
            if (actionData.onActionBegin == EOnActionBegin.ExecuteOnStart)
                invokeAction();
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void Update()
        {
            if (actionData.onActionBegin == EOnActionBegin.ExecuteOnUpdate)
                invokeAction();
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void FixedUpdate()
        {
            if (actionData.onActionBegin == EOnActionBegin.ExecuteOnFixedUpdate)
                invokeAction();
        }


        /// <summary>
        /// Bootstraps action data and initializes everything properly
        /// </summary>
        private void bootstrapper()
        {
            this.RegisterComponent(actionData.actionName);

            // Register action if necessary
            if (actionData.onActionBegin == EOnActionBegin.ExecuteExternally || actionData.shouldAddToRegistry)
                ActionManager.RegisterAction(actionData.actionName, invokeAction);
        }


        /// <summary>
        /// This action invokes internal and next action
        /// </summary>
        private void invokeAction(IActionParameters actionParameters = null)
        {
            // Invoke self
            StartCoroutine(invokeSelf(actionParameters));
        }


        /// <summary>
        /// Coroutine to invoke this gameObject's action
        /// </summary>
        private IEnumerator invokeSelf(IActionParameters parameters)
        {
            if (actionData.onActionBegin == EOnActionBegin.ExecuteOnStart || actionData.onActionBegin == EOnActionBegin.ExecuteExternally)
            {
                yield return new WaitForSeconds(actionData.delayBeforeCurrentAction);
                actionData.slave.doAction(parameters);
            }
            else
                actionData.slave.doAction(parameters);

            if (actionData.onActionEnd == EOnActionEnd.ExecuteAnotherAction)
                invokeNext(passingParams);
            else if (actionData.onActionEnd == EOnActionEnd.DestroySelf)
            {
                ActionManager.DeleteAction(actionData.actionName);
                Destroy(gameObject, actionData.delayBeforeDestroy);
            }
        }


        /// <summary>
        /// Coroutine to invoke next action
        /// </summary>
        private void invokeNext(IActionParameters parameters)
        {
            if (actionData.nextCustomAction != null)
                actionData.nextCustomAction.invokeAction(parameters);
            else
                ActionManager.ExecuteAction(actionData.nextCustomActionName, parameters.data, parameters.source);
        }


        #endregion Internals


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
            return actionData.slave;
        }
    }
}