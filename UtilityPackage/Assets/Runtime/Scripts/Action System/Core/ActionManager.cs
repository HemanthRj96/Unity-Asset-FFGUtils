using System;
using System.Collections.Generic;
using UnityEngine;


namespace FickleFrames.ActionSystem
{
    public static class ActionManager
    {
        #region Private Fields

        private static Dictionary<string, Action<IActionParameters>> actionDictionary = new Dictionary<string, Action<IActionParameters>>();

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Extension method for all the actions with IActionParameters
        /// </summary>
        /// <param name="shouldSubscribe">Set this as true if you want to subsribe multiple actions under same tag</param>
        public static void RegisterAction(Action<IActionParameters> targetAction, string tagID, bool shouldSubscribe = false)
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
                if (actionDictionary[tagID] == null)
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
                cachedAction.Invoke(new ActionParameters(data, source));
            else
                cachedAction.Invoke(null);
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

        #endregion Public Methods
    }
}
