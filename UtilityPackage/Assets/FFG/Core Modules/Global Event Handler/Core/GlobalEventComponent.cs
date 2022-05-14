using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace FFG
{
    /// <summary>
    /// Component is sealed, to use it attach it to a GameObject
    /// </summary>
    public sealed class GlobalEventComponent : MonoBehaviour
    {
        // Internal class

        internal class EventInvokeHelpers
        {
            // Constructor

            public EventInvokeHelpers(string eventTag, EventContainer eventContainer)
            {
                _eventTag = eventTag;
                _eventContainer = eventContainer;
            }


            // Fields & properties

            private string _eventTag = null;
            private EventContainer _eventContainer = null;


            // Public methods

            public string EventTag() => _eventTag;
            public void NoParamEvent() => _eventContainer.Invoke();
            public void SingleParamEvent(object eventData) => _eventContainer.Invoke(eventData);
            public void DoubleParamEvent(object eventData, GameObject eventInstigator) => _eventContainer.Invoke(eventData, eventInstigator);
        }


        // Fields & properties

        [SerializeField]
        private List<EventContainer> _eventContainer = new List<EventContainer>();

        private List<EventInvokeHelpers> _invokeHelpers = new List<EventInvokeHelpers>();

        // Private methods

        private void Awake() => init();

        /// <summary>
        /// Initializing on load
        /// </summary>
        private void init()
        {
            EventInvokeHelpers helper = null;

            foreach (var ec in _eventContainer)
            {
                helper = new EventInvokeHelpers(ec.EventTag, ec);

                GlobalEvents.CreateEvent(helper.EventTag());
                GlobalEvents.AddEventListener(helper.EventTag(), helper.NoParamEvent);
                GlobalEvents.AddEventListener(helper.EventTag(), helper.SingleParamEvent);
                GlobalEvents.AddEventListener(helper.EventTag(), helper.DoubleParamEvent);

                _invokeHelpers.Add(helper);
            }
        }

        /// <summary>
        /// Unsubscribe from all the events
        /// </summary>
        private void OnDestroy()
        {
            foreach (var helper in _invokeHelpers)
            {
                GlobalEvents.RemoveEventListener(helper.EventTag(), helper.NoParamEvent);
                GlobalEvents.RemoveEventListener(helper.EventTag(), helper.SingleParamEvent);
                GlobalEvents.RemoveEventListener(helper.EventTag(), helper.DoubleParamEvent);
            }
        }
    }
}