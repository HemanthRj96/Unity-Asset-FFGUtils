using System;
using System.Collections.Generic;
using UnityEngine;


namespace FFG.Systems
{
    /// <summary>
    /// System used to register/deregister/execute custom actions
    /// </summary>
    public static class ActionSystem
    {
        /*.............................................Private Fields.......................................................*/

        private static Dictionary<string, Action<IActionMessage>> s_actionListener = new Dictionary<string, Action<IActionMessage>>();
        private static Dictionary<string, Action<IActionMessage>> s_actionBroadcaster = new Dictionary<string, Action<IActionMessage>>();

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Method to create a remote action that can be invoked using the remote action name 
        /// </summary>
        /// <param name="remoteAction">Target remote action</param>
        /// <param name="remoteActionName">Name for this remote action</param>
        /// <param name="multipleSubscription">Enable multiple remote listeners under the same listener name</param>
        public static void CreateRemoteAction(Action<IActionMessage> remoteAction, string remoteActionName, bool multipleSubscription = false)
        {
            if (s_actionListener.ContainsKey(remoteActionName))
            {
                if (multipleSubscription)
                    s_actionListener[remoteActionName] += remoteAction;
                else
                    Debug.LogWarning($"Action already exists!! [Action Name = {remoteActionName}] Set multipleSubscription as true " +
                        $"if this behaviour was intended");
                return;
            }
            else
                s_actionListener.Add(remoteActionName, remoteAction);
        }


        /// <summary>
        /// Method to delete a remote action
        /// </summary>
        /// <param name="remoteActionName">Target remote action to be removed</param>
        public static void DeleteRemoteAction(string remoteActionName)
        {
            if (s_actionListener.ContainsKey(remoteActionName))
            {
                s_actionListener[remoteActionName] = null;
                s_actionListener.Remove(remoteActionName);
            }
        }


        /// <summary>
        /// Invokes a remote action
        /// </summary>
        /// <param name="remoteActionName">Name of the remote action to be invoked</param>
        /// <param name="data">Data to be passed</param>
        /// <param name="gameObject">Instigating GameObject(self)</param>
        public static void InvokeRemoteAction(string remoteActionName, object data = null, GameObject gameObject = null)
        {

            if (!s_actionListener.ContainsKey(remoteActionName) || s_actionListener[remoteActionName] == null)
            {
                s_actionListener.TryRemove(remoteActionName);
                return;
            }

            s_actionListener[remoteActionName].Invoke(new ActionData(data, gameObject));
        }


        /// <summary>
        /// Method to register a broadcaster
        /// </summary>
        /// <param name="broadcasterName">Unique broadcaster name</param>
        public static void CreateBroadcasterAction(string broadcasterName)
        {
            if (!s_actionBroadcaster.ContainsKey(broadcasterName))
            {
                Action<IActionMessage> action = delegate { };
                s_actionBroadcaster.Add(broadcasterName, action);
            }
        }


        /// <summary>
        /// Method to delete broadcaster
        /// </summary>
        /// <param name="broadcasterName">Target broadcaster name</param>
        public static void DeleteBroadcasterAction(string broadcasterName)
        {
            if (s_actionBroadcaster.ContainsKey(broadcasterName))
            {
                s_actionBroadcaster[broadcasterName] = null;
                s_actionBroadcaster.Remove(broadcasterName);
            }
        }


        /// <summary>
        /// Method to subscribe to a broadcaster action
        /// </summary>
        /// <param name="broadcasterName">Target name of the action</param>
        /// <param name="listeningMethod">Target method to listen</param>
        public static void SubscribeToBroadcasterAction(string broadcasterName, Action<IActionMessage> listeningMethod)
        {
            if (s_actionBroadcaster.ContainsKey(broadcasterName))
                s_actionBroadcaster[broadcasterName] += listeningMethod;
        }


        /// <summary>
        /// Method to invoke a broadcast call
        /// </summary>
        /// <param name="actionName">Target name of the action</param>
        /// <param name="data">Optional data to be sent</param>
        /// <param name="source">Gameobject invoking the broadcast call</param>
        public static void InvokeBroadcastCall(string actionName, object data = null, GameObject source = null)
        {
            if (s_actionBroadcaster.ContainsKey(actionName))
                s_actionBroadcaster[actionName]?.Invoke(new ActionData(data, source));
        }
    }
}
