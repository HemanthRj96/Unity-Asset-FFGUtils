using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class EventContainer
{
    public EventContainer()
    {
        _noParameterEvent = new UnityEvent();
        _singleParameterEvent = new UnityEvent<object>();
        _doubleParameterEvent = new UnityEvent<object, GameObject>();
    }

    public EventContainer(string eventTag)
    {
        _eventTag = eventTag;
        _noParameterEvent = new UnityEvent();
        _singleParameterEvent = new UnityEvent<object>();
        _doubleParameterEvent = new UnityEvent<object, GameObject>();
    }


    // Fields & properties

    [SerializeField]
    private string _eventTag;
    [SerializeField]
    private UnityEvent _noParameterEvent = null;
    [SerializeField]
    private UnityEvent<object> _singleParameterEvent = null;
    [SerializeField]
    private UnityEvent<object, GameObject> _doubleParameterEvent = null;

    public string EventTag => _eventTag;


    // Public methods

    public void AddListener(UnityAction listener) => _noParameterEvent.AddListener(listener);
    public void AddListener(UnityAction<object> listener) => _singleParameterEvent.AddListener(listener);
    public void AddListener(UnityAction<object, GameObject> listener) => _doubleParameterEvent.AddListener(listener);

    public void RemoveListener(UnityAction listener) => _noParameterEvent.RemoveListener(listener);
    public void RemoveListener(UnityAction<object> listener) => _singleParameterEvent.RemoveListener(listener);
    public void RemoveListener(UnityAction<object, GameObject> listener) => _doubleParameterEvent.RemoveListener(listener);

    public void Invoke() => _noParameterEvent?.Invoke();
    public void Invoke(object data) => _singleParameterEvent?.Invoke(data);
    public void Invoke(object data, GameObject instigator) => _doubleParameterEvent?.Invoke(data, instigator);
}