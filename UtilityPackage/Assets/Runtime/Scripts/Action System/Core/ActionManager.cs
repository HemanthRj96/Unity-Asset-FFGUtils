using System;
using System.Collections.Generic;
using UnityEngine;


namespace FickleFrames.ActionSystem
{
    /// <summary>
    /// Manager class used to register/deregister/execute custom actions
    /// </summary>
    public static class ActionManager
    {
        #region Private Fields

        private static Dictionary<string, Action<IActionParameters>> actionCollection = new Dictionary<string, Action<IActionParameters>>();
        private static Dictionary<string, ActionComponent> componentCollection = new Dictionary<string, ActionComponent>();

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Extension method to register an action component
        /// </summary>
        /// <param name="actionName">Name of the action component</param>
        public static void RegisterActionComponent(this ActionComponent component, string actionName)
        {
            if (componentCollection.ContainsKey(actionName))
                return;
            componentCollection.Add(actionName, component);
        }


        /// <summary>
        /// Extension method to remove action component
        /// </summary>
        /// <param name="actionName">Name of the action component</param>
        public static void DeregisterActionComponent(this ActionComponent component, string actionName)
        {
            if (componentCollection.ContainsKey(actionName))
                componentCollection.Remove(actionName);
        }


        /// <summary>
        /// Method to register an action
        /// </summary>
        /// <param name="shouldSubscribe">Set this as true if you want to subsribe multiple actions under same tag</param>
        public static void RegisterAction(Action<IActionParameters> targetAction, string actionName, bool shouldSubscribe = false)
        {
            if (actionCollection.ContainsKey(actionName) && shouldSubscribe)
                actionCollection[actionName] += targetAction;
            else
                actionCollection.Add(actionName, targetAction);
        }


        /// <summary>
        /// Executes an action with a valid actionName
        /// </summary>
        /// <param name="data">Data to be passed</param>
        /// <param name="source">Instigating GameObject(self)</param>
        public static void ExecuteAction(string actionName, object data = null, GameObject source = null)
        {
            Action<IActionParameters> cachedAction = null;

            // Check if the tag exists
            if (actionCollection.ContainsKey(actionName))
            {
                if (actionCollection[actionName] == null)
                {
                    actionCollection.Remove(actionName);
                    return;
                }
                cachedAction = actionCollection[actionName];
            }
            else
                return;

            // Construct data if there's any data available otherwise flush the cached data
            if (data != null || source != null)
                cachedAction.Invoke(new ActionParameters(data, source));
            else
                cachedAction.Invoke(null);
        }


        /// <summary>
        /// Removes action
        /// </summary>
        public static void DeregisterAction(string actionName)
        {
            if (actionCollection.ContainsKey(actionName))
            {
                actionCollection[actionName] = null;
                actionCollection.Remove(actionName);
            }
        }


        /// <summary>
        /// Function that returns an action component during runtime
        /// </summary>
        /// <param name="actionName">Name of the action component</param>
        public static ActionComponent FetchActionComponent(string actionName)
        {
            if (componentCollection.ContainsKey(actionName))
                return componentCollection[actionName];
            return null;
        }

        #endregion Public Methods
    }
}
