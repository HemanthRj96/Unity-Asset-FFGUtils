using FickleFrames.Systems.Internal;
using UnityEngine;


namespace FickleFrames.Systems
{
    /// <summary>
    /// Component class which should be attached to gameObjects that provides multiple ways of running custom codes and 
    /// linking to another action component creating a chain of infinite number of custom actions
    /// </summary>
    public abstract class ActionComponent : MonoBehaviour
    {
        #region Editor
#if UNITY_EDITOR
        [HideInInspector] public bool isChained = false;
        [HideInInspector] public ActionComponent chainedComponent = null;
#endif
        #endregion Editor

        #region Internal

        /*.............................................Serialized Fields....................................................*/
#pragma warning disable 0649
        [SerializeField] private ActionSettings _actionSettings;
#pragma warning restore 0649

        /*.............................................Private Fields.......................................................*/
        private float timer = 0;
        private bool timerSet = false;

        /*.............................................Private Properties...................................................*/
        protected IActionData _outgoingData { get; set; } = new ActionData();
        protected IActionData _cachedData { get; set; } = new ActionData();


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
        /// Bootstraps action data and initializes everything properly
        /// </summary>
        private void bootstrapper()
        {
            if (_actionSettings.ActionExecutionMode == EActionExecutionMode.ExecuteExternally || _actionSettings.ShouldRegister)
                ActionSystem.RegisterAction(commitDelayedAction, _actionSettings.ActionName);
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void Start()
        {
            if (_actionSettings.ActionExecutionMode == EActionExecutionMode.ExecuteOnStart)
                commitDelayedAction();
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void Update()
        {
            if (_actionSettings.ActionExecutionMode == EActionExecutionMode.ExecuteOnUpdate)
                commitAction();
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        private void FixedUpdate()
        {
            if (_actionSettings.ActionExecutionMode == EActionExecutionMode.ExecuteOnFixedUpdate)
                commitAction();
        }


        /// <summary>
        /// Delete action from the ActionManager
        /// </summary>
        private void OnDestroy()
        {
            ActionSystem.DeregisterAction(_actionSettings.ActionName);
        }


        /// <summary>
        /// Method to invoke action with delay
        /// </summary>
        /// <param name="actionData"></param>
        private void commitDelayedAction(IActionData actionData = null)
        {
            _cachedData = actionData;
            Invoke(nameof(commitAction), _actionSettings.ActionDelay);
        }


        /// <summary>
        /// Method to invoke action without any delay
        /// </summary>
        private void commitAction()
        {
            doAction(_cachedData);

            if (_actionSettings.OnActionExecutionEnd == EOnActionExecutionEnd.ExecuteAnotherAction)
            {
                if (_actionSettings.NextAction != null)
                    _actionSettings.NextAction.commitDelayedAction(_outgoingData);
                else
                    ActionSystem.ExecuteAction(_actionSettings.NextActionName, _outgoingData.Data, _outgoingData.Source);
            }
            else if (_actionSettings.OnActionExecutionEnd == EOnActionExecutionEnd.DestroySelf)
            {
                ActionSystem.DeregisterAction(_actionSettings.ActionName);
                Destroy(gameObject, _actionSettings.DestroyDelay);
            }
        }

        #endregion Private Methods

        #endregion Internal

        #region Protected Methods

        /*.............................................Protected Methods....................................................*/
        /// <summary>
        /// Call this method to set the data that has to passed to a chained action
        /// </summary>
        /// <param name="data">Data to be passed</param>
        protected void constructOutgoingData(object data = null, GameObject source = null)
        {
            _outgoingData = new ActionData(data, gameObject);
        }

        /// <summary>
        /// Function must be implemented that runs all the custom logic for this action component
        /// </summary>
        /// <param name="parameters">Parameters passed</param>
        protected abstract void doAction(IActionData parameters);

        #endregion Protected Methods
    }
}