using System.Collections;
using UnityEngine;

namespace FickleFrames.Action
{
    /// <summary>
    /// This component should be attached to any GameObject that employs custom behaviours
    /// </summary>
    public class ActionComponent : MonoBehaviour
    {
        [Header("Custom Action Details")]
        [SerializeField]
        protected ActionDataStruct actionData;

        protected IActionParams actionParams { get; private set; }


        /// <summary>
        /// This method should be overrided inorder to provide custom code
        /// </summary>
        /// <param name="actionParams"></param>
        protected virtual void InternalExecute() { }

        /// <summary>
        /// This method is used to execute action with/without passing any parameters
        /// </summary>
        /// <param name="data">Optional data</param>
        public void Execute(IActionParams actionParams = null)
        {
            this.actionParams = actionParams;
            StartCoroutine(invokeRoutine());
        }

        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        protected void Awake()
        {
            if (actionData.onActionBegin == E_OnActionBegin.ExecuteExternally || actionData.shouldAddToRegistry)
                ActionManager.RegisterStaticAction(actionData.tagID, Execute);
        }

        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        protected void Start()
        {
            if (actionData.onActionBegin == E_OnActionBegin.ExecuteOnStart)
                InternalExecute();
        }

        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        protected void Update()
        {
            if (actionData.onActionBegin == E_OnActionBegin.ExecuteOnUpdate)
                InternalExecute();
        }

        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        protected void FixedUpdate()
        {
            if (actionData.onActionBegin == E_OnActionBegin.ExecuteOnFixedUpdate)
                InternalExecute();
        }

        #region Internal methods

        /// <summary>
        /// This method runs on completion of the data execution
        /// </summary>
        private void onExecutionEnd()
        {
            switch (actionData.onActionEnd)
            {
                case E_OnActionEnd.ExecuteAnotherAction:
                    StartCoroutine(invokeRoutine(true));
                    break;
                case E_OnActionEnd.RemoveFromRegistry:
                    if (actionData.onActionBegin == E_OnActionBegin.ExecuteExternally || actionData.shouldAddToRegistry)
                        ActionManager.DeleteAction(actionData.tagID);
                    break;
                case E_OnActionEnd.DeactivateSelf:
                    if (actionData.onActionBegin == E_OnActionBegin.ExecuteExternally || actionData.shouldAddToRegistry)
                        ActionManager.DeleteAction(actionData.tagID);
                    gameObject.SetActive(false);
                    break;
                case E_OnActionEnd.DestroySelf:
                    if (actionData.onActionBegin == E_OnActionBegin.ExecuteExternally || actionData.shouldAddToRegistry)
                        ActionManager.DeleteAction(actionData.tagID);
                    Destroy(gameObject, actionData.delayBeforeDestroy);
                    break;
                case E_OnActionEnd.DoNothing:
                default:
                    break;
            }
        }

        /// <summary>
        /// Coroutine for adding delay before next action execution
        /// </summary>
        /// <returns></returns>
        private IEnumerator invokeRoutine(bool selectFlag = false)
        {
            if (selectFlag == false)
            {
                yield return new WaitForSecondsRealtime(actionData.delayBeforeCurrentAction);
                InternalExecute();
                onExecutionEnd();
            }
            else if (selectFlag == true)
            {
                yield return new WaitForSecondsRealtime(actionData.delayBeforeNextAction);
                if (actionData.targetCustomAction != null)
                    actionData.targetCustomAction.InternalExecute();
                else
                    ActionManager.ExecuteAction(actionData.targetTagID);
            }
        }

        #endregion
    }
}