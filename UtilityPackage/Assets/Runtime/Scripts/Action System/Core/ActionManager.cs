using System;
using System.Collections.Generic;
using UnityEngine;

namespace FickleFrames
{
    /// <summary>
    /// Manager class used to call invoke a custom action or add a custom action
    /// </summary>
    public static class ActionManager
    {
        #region Internals


        private static ActionParameters cachedActionData = null;
        private static Dictionary<string, Action<IActionParameters>> actionDictionary = new Dictionary<string, Action<IActionParameters>>();
        private static Dictionary<string, ActionComponent> componentDictionary = new Dictionary<string, ActionComponent>();

        /// <summary>
        /// ActionParameter data cacher
        /// </summary>
        private static void ConstructData(ActionParameters data) => cachedActionData = data;


        #endregion Internals


        /// <summary>
        /// Extension method for component class
        /// </summary>
        public static void RegisterComponent(this ActionComponent component, string componentName)
        {
            componentDictionary.TryAdd(componentName, component);
        }


        /// <summary>
        /// Adds a custom action to be called later
        /// </summary>
        /// <param name="shouldSubscribe">Set this as true if you want to subsribe multiple actions under same tag</param>
        public static void RegisterAction(string tagID, Action<IActionParameters> targetAction, bool shouldSubscribe = false)
        {
            if (actionDictionary.ContainsKey(tagID) && shouldSubscribe)
                actionDictionary[tagID] += targetAction;
            else
                actionDictionary.Add(tagID, targetAction);
        }


        /// <summary>
        /// Executes an action with a valid tagID
        /// </summary>
        /// <param name="data">Data to be passed</param>
        /// <param name="source">Instigating GameObject(self)</param>
        public static void ExecuteAction(string tagID, object data = null, GameObject source = null)
        {
            Action<IActionParameters> cachedAction = null;

            // Check if the tag exists
            if (actionDictionary.ContainsKey(tagID))
            {
                if(actionDictionary[tagID] == null)
                {
                    actionDictionary.Remove(tagID);
                    return;
                }
                cachedAction = actionDictionary[tagID];
            }
            else
                return;

            // Construct data if there's any data available otherwise flush the cached data
            if (data != null || source != null)
                ConstructData(new ActionParameters(data, source));
            else
                cachedActionData = null;

            cachedAction.Invoke(cachedActionData);
        }


        /// <summary>
        /// Removes action
        /// </summary>
        public static void DeleteAction(string tagID)
        {
            if (actionDictionary.ContainsKey(tagID))
            {
                actionDictionary[tagID] = null;
                actionDictionary.Remove(tagID);
            }
        }
    }
}
