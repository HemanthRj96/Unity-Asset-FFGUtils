using System;
using System.Collections.Generic;
using UnityEngine;


namespace FickleFrameGames.Systems
{
    /// <summary>
    /// System used to register/deregister/execute custom actions
    /// </summary>
    public static class ActionSystem
    {
        /*.............................................Private Fields.......................................................*/

        private static Dictionary<string, Action<IActionData>> s_actionListener = new Dictionary<string, Action<IActionData>>();
        private static Dictionary<string, Action<IActionData>> s_actionBroadcaster = new Dictionary<string, Action<IActionData>>();

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Method to register an action
        /// </summary>
        /// <param name="multipleSubscription">Set this as true if you want to subsribe multiple actions under same tag</param>
        public static void CreateListener(Action<IActionData> listenerMethod, string listenerName, bool multipleSubscription = false)
        {
            if (s_actionListener.ContainsKey(listenerName))
            {
                if (multipleSubscription)
                    s_actionListener[listenerName] += listenerMethod;
                else
                    Debug.LogWarning($"Action already exists!! [Action Name = {listenerName}] Set multipleSubscription as true " +
                        $"if this behaviour was intended");
                return;
            }
            else
                s_actionListener.Add(listenerName, listenerMethod);
        }


        /// <summary>
        /// Deregister action from actionManager
        /// </summary>
        /// <param name="actionName">Target action name</param>
        public static void DeleteListener(string actionName)
        {
            if (s_actionListener.ContainsKey(actionName))
            {
                s_actionListener[actionName] = null;
                s_actionListener.Remove(actionName);
            }
        }


        /// <summary>
        /// Executes an action with a valid actionName
        /// </summary>
        /// <param name="data">Data to be passed</param>
        /// <param name="gameObject">Instigating GameObject(self)</param>
        public static void InvokeRemoteListener(string actionName, object data = null, GameObject gameObject = null)
        {

            if (!s_actionListener.ContainsKey(actionName) || s_actionListener[actionName] == null)
            {
                s_actionListener.TryRemove(actionName);
                return;
            }

            s_actionListener[actionName].Invoke(new ActionData(data, gameObject));
        }


        /// <summary>
        /// Method to register a broadcaster
        /// </summary>
        /// <param name="broadcasterName">Unique broadcaster name</param>
        public static void CreateBroadcaster(string broadcasterName)
        {
            if (!s_actionBroadcaster.ContainsKey(broadcasterName))
            {
                Action<IActionData> action = delegate { };
                s_actionBroadcaster.Add(broadcasterName, action);
            }
        }


        /// <summary>
        /// Method to delete broadcaster
        /// </summary>
        /// <param name="broadcasterName">Target broadcaster name</param>
        public static void DeleteBroadcaster(string broadcasterName)
        {
            if (s_actionBroadcaster.ContainsKey(broadcasterName))
            {
                s_actionBroadcaster[broadcasterName] = null;
                s_actionBroadcaster.Remove(broadcasterName);
            }
        }


        /// <summary>
        /// Method to subscribe to a broadcaster
        /// </summary>
        /// <param name="broadcasterName">Broadcaster name</param>
        public static void SubscribeToBroadcaster(string broadcasterName, Action<IActionData> listeningMethod)
        {
            if (s_actionBroadcaster.ContainsKey(broadcasterName))
                s_actionBroadcaster[broadcasterName] += listeningMethod;
        }


        /// <summary>
        /// Method to create a braodcast call to all listeners
        /// </summary>
        /// <param name="actionName">Broadcaster name</param>
        public static void InvokeBroadcastCall(string actionName, object data = null, GameObject source = null)
        {
            if (s_actionBroadcaster.ContainsKey(actionName))
                s_actionBroadcaster[actionName]?.Invoke(new ActionData(data, source));
        }
    }
}
