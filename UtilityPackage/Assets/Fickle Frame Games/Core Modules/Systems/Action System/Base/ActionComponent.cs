using FickleFrameGames.Systems.Internal;
using UnityEngine;


namespace FickleFrameGames.Systems
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
        public EActionExecutionMode actionExecutionMode => _actionSettings.ActionExecutionMode;
#endif
        #endregion Editor

        /*.............................................Serialized Fields....................................................*/

#pragma warning disable 0649
        [SerializeField] private bool dontDestroyOnLoad = false;
        [SerializeField] private ActionSettings _actionSettings;
#pragma warning restore 0649

        /*.............................................Private Properties...................................................*/

        protected IActionData _outgoingData { get; set; } = new ActionData();
        protected IActionData _cachedData { get; set; } = new ActionData();

        /*.............................................Private Methods......................................................*/

        /// <summary>
        /// Bootstraps action data and initializes everything properly
        /// </summary>
        private void bootstrapper()
        {
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(this);
            if (_actionSettings.ActionExecutionMode == EActionExecutionMode.ExecuteExternally || _actionSettings.ShouldRegister)
                ActionSystem.CreateListener(commitDelayedAction, _actionSettings.ActionName);
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
                    ActionSystem.InvokeRemoteListener(_actionSettings.NextActionName, _outgoingData.Data, _outgoingData.Source);
            }
            else if (_actionSettings.OnActionExecutionEnd == EOnActionExecutionEnd.DestroySelf)
            {
                ActionSystem.DeleteListener(_actionSettings.ActionName);
                Destroy(gameObject, _actionSettings.DestroyDelay);
            }
        }

        /*.............................................Protected Methods....................................................*/

        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        protected virtual void Awake()
        {
            bootstrapper();
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        protected virtual void Start()
        {
            if (_actionSettings.ActionExecutionMode == EActionExecutionMode.ExecuteOnStart)
                commitDelayedAction();
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        protected virtual void Update()
        {
            if (_actionSettings.ActionExecutionMode == EActionExecutionMode.ExecuteOnUpdate)
                commitAction();
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (_actionSettings.ActionExecutionMode == EActionExecutionMode.ExecuteOnFixedUpdate)
                commitAction();
        }


        /// <summary>
        /// Call base method to prevent unknown behaviour
        /// </summary>
        protected virtual void OnDestroy()
        {
            ActionSystem.DeleteListener(_actionSettings.ActionName);
        }


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

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Method to override delay time of this action
        /// </summary>
        /// <param name="newDelay">New delay time</param>
        public void OverrideDelayTime(float newDelay)
        {
            newDelay = Mathf.Clamp(newDelay, 0, 120);
            _actionSettings.ActionDelay = newDelay;
        }


        /// <summary>
        /// Returns the current delay time of this action
        /// </summary>
        /// <returns>Delay time</returns>
        public float GetCurrentDelayTime()
        {
            return _actionSettings.ActionDelay;
        }
    }
}