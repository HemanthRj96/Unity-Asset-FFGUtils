using System;
using System.Collections.Generic;
using UnityEngine;


namespace FickleFrames.Systems
{
    /// <summary>
    /// Manager class used to register/deregister/execute custom actions
    /// </summary>
    public static class ActionManager
    {
        #region Internal

        //*********************************************Private Fields****************************************************
        private static Dictionary<string, Action<IActionParameters>> s_actionCollection = new Dictionary<string, Action<IActionParameters>>();
        private static Dictionary<string, ActionComponent> s_componentCollection = new Dictionary<string, ActionComponent>();

        #endregion Internal

        #region Public Methods

        //*********************************************Public Methods****************************************************
        /// <summary>
        /// Extension method to register an action component
        /// </summary>
        /// <param name="actionName">Name of the action component</param>
        public static void RegisterActionComponent(this ActionComponent component, string actionName)
        {
            if (s_componentCollection.ContainsKey(actionName))
                return;
            s_componentCollection.Add(actionName, component);
        }


        /// <summary>
        /// Extension method to remove action component
        /// </summary>
        /// <param name="actionName">Name of the action component</param>
        public static void DeregisterActionComponent(this ActionComponent component, string actionName)
        {
            if (s_componentCollection.ContainsKey(actionName))
                s_componentCollection.Remove(actionName);
        }


        /// <summary>
        /// Method to register an action
        /// </summary>
        /// <param name="multipleSubscription">Set this as true if you want to subsribe multiple actions under same tag</param>
        public static void RegisterAction(Action<IActionParameters> targetAction, string actionName, bool multipleSubscription = false)
        {
            if (s_actionCollection.ContainsKey(actionName))
            {
                if (multipleSubscription)
                    s_actionCollection[actionName] += targetAction;
                return;
            }
            else
                s_actionCollection.Add(actionName, targetAction);
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
            if (s_actionCollection.ContainsKey(actionName))
            {
                if (s_actionCollection[actionName] == null)
                    s_actionCollection.Remove(actionName);
                cachedAction = s_actionCollection[actionName];
            }

            // Construct data if there's any data available otherwise flush the cached data
            if (data != null || source != null)
                cachedAction.Invoke(new ActionParameters(data, source));
            else
                cachedAction.Invoke(null);
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


        /// <summary>
        /// Function that returns an action component during runtime
        /// </summary>
        /// <param name="actionName">Target action component name</param>
        public static ActionComponent FetchActionComponent(string actionName)
        {
            if (s_componentCollection.ContainsKey(actionName))
                return s_componentCollection[actionName];
            return null;
        }

        #endregion Public Methods
    }
}
