using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace FFG
{
    /// <summary>
    /// Class used to create & invoke messages and pass data
    /// </summary>
    public static class GlobalEvents
    {
        // Fields & properties

        private static Dictionary<string, EventContainer> s_eventContainerLookup = new Dictionary<string, EventContainer>();

        // Public methods

        /// <summary>
        /// Returns true if the tag is unique
        /// </summary>
        private static bool containsTag(string eventTag)
        {
            return s_eventContainerLookup.ContainsKey(eventTag);
        }

        /// <summary>
        /// Helper method to create event
        /// </summary>
        private static void eventCreator
            (
                string eventTag,
                UnityAction noParamEvent = null,
                UnityAction<object> singleParamEvent = null,
                UnityAction<object, GameObject> doubleParamEvent = null
            )
        {
            EventContainer container = new EventContainer(eventTag);

            if (noParamEvent != null)
                container.AddListener(noParamEvent);
            else if (singleParamEvent != null)
                container.AddListener(singleParamEvent);
            else if (doubleParamEvent != null)
                container.AddListener(doubleParamEvent);
            s_eventContainerLookup.Add(eventTag, container);
        }

        /// <summary>
        /// Method to create a new event
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        public static void CreateEvent(string eventTag)
        {
            if (eventTag.IsValid() && !containsTag(eventTag))
                eventCreator(eventTag);
        }

        /// <summary>
        /// Method to create a new event with a default listener
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="targetEvent">Target listener with no parameters</param>
        public static void CreateEvent(string eventTag, UnityAction targetEvent)
        {
            if (eventTag.IsValid())
                if (containsTag(eventTag))
                    AddEventListener(eventTag, targetEvent);
                else
                    eventCreator(eventTag, noParamEvent: targetEvent);
        }

        /// <summary>
        /// Method to create a new event with a default listener
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="targetEvent">Target listener with single parameter</param>
        public static void CreateEvent(string eventTag, UnityAction<object> targetEvent)
        {
            if (eventTag.IsValid())
                if (containsTag(eventTag))
                    AddEventListener(eventTag, targetEvent);
                else
                    eventCreator(eventTag, singleParamEvent: targetEvent);
        }

        /// <summary>
        /// Method to create a new event with a default listener
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="targetEvent">Target listener with two parameters</param>
        public static void CreateEvent(string eventTag, UnityAction<object, GameObject> targetEvent)
        {
            if (eventTag.IsValid())
                if (containsTag(eventTag))
                    AddEventListener(eventTag, targetEvent);
                else
                    eventCreator(eventTag, doubleParamEvent: targetEvent);
        }

        /// <summary>
        /// Method to add a listener to an event
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="targetEvent">Target event with no parameters</param>
        public static void AddEventListener(string eventTag, UnityAction targetEvent)
        {
            if (eventTag.IsValid() && containsTag(eventTag))
                s_eventContainerLookup[eventTag].AddListener(targetEvent);
        }

        /// <summary>
        /// Method to add a listener to an event
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="targetEvent">Target event with one parameter</param>
        public static void AddEventListener(string eventTag, UnityAction<object> targetEvent)
        {
            if (eventTag.IsValid() && containsTag(eventTag))
                s_eventContainerLookup[eventTag].AddListener(targetEvent);
        }

        /// <summary>
        /// Method to add a listener to an event
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="targetEvent">Target event with two parameter</param>
        public static void AddEventListener(string eventTag, UnityAction<object, GameObject> targetEvent)
        {
            if (eventTag.IsValid() && containsTag(eventTag))
                s_eventContainerLookup[eventTag].AddListener(targetEvent);
        }

        /// <summary>
        /// Method to invoke an event
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        public static void InvokeEvent(string eventTag)
        {
            if (eventTag.IsValid() && containsTag(eventTag))
                s_eventContainerLookup[eventTag].Invoke();
        }

        /// <summary>
        /// Method to invoke an event with data
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="eventData">Event data</param>
        public static void InvokeEvent(string eventTag, object eventData)
        {
            if (eventTag.IsValid() && containsTag(eventTag))
                s_eventContainerLookup[eventTag].Invoke(eventData);
        }

        /// <summary>
        /// Method to invoke an event with data and instigator
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="eventData">Event data</param>
        /// <param name="eventInstigator"></param>
        public static void InvokeEvent(string eventTag, object eventData, GameObject eventInstigator)
        {
            if (eventTag.IsValid() && containsTag(eventTag))
                s_eventContainerLookup[eventTag].Invoke(eventData, eventInstigator);
        }

        /// <summary>
        /// Method to remove event listener
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="targetEvent">Listener to be removed</param>
        public static void RemoveEventListener(string eventTag, UnityAction targetEvent)
        {
            if (eventTag.IsValid() && containsTag(eventTag))
                s_eventContainerLookup[eventTag].RemoveListener(targetEvent);
        }

        /// <summary>
        /// Method to remove event listener
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="targetEvent">Listener to be removed</param>
        public static void RemoveEventListener(string eventTag, UnityAction<object> targetEvent)
        {
            if (eventTag.IsValid() && containsTag(eventTag))
                s_eventContainerLookup[eventTag].RemoveListener(targetEvent);
        }

        /// <summary>
        /// Method to remove event listener
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        /// <param name="targetEvent">Listener to be removed</param>
        public static void RemoveEventListener(string eventTag, UnityAction<object, GameObject> targetEvent)
        {
            if (eventTag.IsValid() && containsTag(eventTag))
                s_eventContainerLookup[eventTag].RemoveListener(targetEvent);
        }

        /// <summary>
        /// Method to destroy an event
        /// </summary>
        /// <param name="eventTag">Event tag</param>
        public static void DestroyEvent(string eventTag)
        {
            if (eventTag.IsValid() && containsTag(eventTag))
                s_eventContainerLookup.Remove(eventTag);
        }
    }
}
