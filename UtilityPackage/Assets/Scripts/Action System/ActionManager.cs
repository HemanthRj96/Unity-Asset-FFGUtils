using System;
using System.Collections.Generic;
using UnityEngine;

namespace FickleFrames.Action
{
    /// <summary>
    /// This class is used to register custom behaviours without using action component and
    /// execute a custom action
    /// </summary>
    public static class ActionManager
    {
        /// <summary>
        /// Data container which is used to parse any kind of data
        /// </summary>
        private class ActionParams : IActionParams
        {
            public ActionParams(object data = null, GameObject source = null)
            {
                this.data = data;
                this.source = source;
            }

            public object data { get; set; }
            public GameObject source { get; set; }
        }

        private static ActionParams cachedActionData = null;

        private static Dictionary<string, Action<IActionParams>> dynamicActionDictionary = new Dictionary<string, Action<IActionParams>>();
        private static Dictionary<string, Action<IActionParams>> staticActionDictionary = new Dictionary<string, Action<IActionParams>>();


        /// <summary>
        /// This method should only be used if you're not using CustomAction class
        /// </summary>
        /// <param name="tagID">Target tag ID for the custom action</param>
        /// <param name="targetAction">Target action</param>
        public static void RegisterDynamicAction(string tagID, Action<IActionParams> targetAction)
        {
            if (dynamicActionDictionary.ContainsKey(tagID))
                dynamicActionDictionary[tagID] += targetAction;
            else
                dynamicActionDictionary.Add(tagID, targetAction);
        }

        /// <summary>
        /// This method is exclusively used by CustomAction class so that it can be triggered dynamically
        /// </summary>
        /// <param name="tagID">Target tag ID for the custom action</param>
        /// <param name="targetAction">Target action</param>
        public static void RegisterStaticAction(string tagID, Action<IActionParams> targetAction)
        {
            if (staticActionDictionary.ContainsKey(tagID))
                staticActionDictionary[tagID] += targetAction;
            else
                staticActionDictionary.Add(tagID, targetAction);
        }

        /// <summary>
        /// Call this method to trigger an action event
        /// </summary>
        /// <param name="tagID">Tag ID for the target event</param>
        /// <param name="data">Data to be passed which is optional</param>
        /// <param name="source">Target source GameObject</param>
        public static void ExecuteAction(string tagID, object data = null, GameObject source = null)
        {
            Action<IActionParams> cachedAction = null;

            // Check if the tag exists inside both dictionaries

            if (dynamicActionDictionary.ContainsKey(tagID))
                cachedAction = dynamicActionDictionary[tagID];
            else if (staticActionDictionary.ContainsKey(tagID))
                cachedAction = staticActionDictionary[tagID];
            else
                return;

            // Construct data if there's any data available otherwise flush the cached data
            if (data != null || source != null)
                ConstructData(new ActionParams(data, source));
            else
                FlushData();

            cachedAction.Invoke(cachedActionData);
        }

        public static void DeleteAction(string tagID)
        {
            Debug.Log($"Delete requested {tagID}");
            if (dynamicActionDictionary.ContainsKey(tagID))
                dynamicActionDictionary.Remove(tagID);
            else if (staticActionDictionary.ContainsKey(tagID))
                staticActionDictionary.Remove(tagID);
            else
                Debug.LogWarning($"The key {tagID} do not exist in the current context");
        }

        /// <summary>
        /// Call this method to cache the ActionData
        /// </summary>
        /// <param name="data">Target data</param>
        private static void ConstructData(ActionParams data) => cachedActionData = data;

        /// <summary>
        /// This method flushes the cached ActionData
        /// </summary>
        private static void FlushData() => cachedActionData = null;
    }
}
