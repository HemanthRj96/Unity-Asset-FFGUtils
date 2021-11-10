using System;
using System.Collections.Generic;
using UnityEngine;


namespace FickleFrames.Systems
{
    /// <summary>
    /// System used to register/deregister/execute custom actions
    /// </summary>
    public static class ActionSystem
    {
        #region Internal

        /*.............................................Private Fields.......................................................*/
        private static Dictionary<string, Action<IActionData>> s_actionCollection = new Dictionary<string, Action<IActionData>>();
        private static IActionData s_actionData = null;

        /*.............................................Private Methods......................................................*/
        /// <summary>
        /// Method to construct data
        /// </summary>
        /// <param name="actionData">Target data</param>
        /// <param name="gameObject">Target gameobject</param>
        private static void constructData(IActionData actionData) => s_actionData = actionData;

        #endregion Internal

        #region Public Methods

        /*.............................................Public Methods.......................................................*/
        /// <summary>
        /// Method to register an action
        /// </summary>
        /// <param name="multipleSubscription">Set this as true if you want to subsribe multiple actions under same tag</param>
        public static void RegisterAction(Action<IActionData> targetAction, string actionName, bool multipleSubscription = false)
        {
            if (s_actionCollection.ContainsKey(actionName))
            {
                if (multipleSubscription)
                    s_actionCollection[actionName] += targetAction;
                else
                    Debug.LogWarning($"Action already exists!! [Action Name = {actionName}] Set multipleSubscription as true " +
                        $"if this behaviour was intended");
                return;
            }
            else
                s_actionCollection.Add(actionName, targetAction);
        }


        /// <summary>
        /// Executes an action with a valid actionName
        /// </summary>
        /// <param name="data">Data to be passed</param>
        /// <param name="gameObject">Instigating GameObject(self)</param>
        public static void ExecuteAction(string actionName, object data = null, GameObject gameObject = null)
        {
            Action<IActionData> cachedAction = null;

            // Check if the tag exists
            if (s_actionCollection.ContainsKey(actionName))
            {
                if (s_actionCollection[actionName] == null)
                    s_actionCollection.Remove(actionName);
                cachedAction = s_actionCollection[actionName];
            }
            else
            {
                Debug.LogWarning($"Action Not Found!! [Action Name = {actionName}]");
                return;
            }

            // Construct data if there's any data available otherwise flush the cached data
            if (data != null || gameObject != null)
                constructData(new ActionData(data, gameObject));
            else
                s_actionData = null;

            cachedAction.Invoke(s_actionData);
        }


        /// <summary>
        /// Deregister action from actionManager
        /// </summary>
        /// <param name="actionName">Target action name</param>
        public static void DeregisterAction(string actionName)
        {
            if (s_actionCollection.ContainsKey(actionName))
            {
                s_actionCollection[actionName] = null;
                s_actionCollection.Remove(actionName);
            }
        }

        #endregion Public Methods
    }
}
