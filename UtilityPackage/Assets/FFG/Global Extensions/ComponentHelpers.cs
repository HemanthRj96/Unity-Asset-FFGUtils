using UnityEngine;


public static class ComponentHelpers
{
    /// <summary>
    /// Returns the component if it exists otherwise returns a new component
    /// </summary>
    public static TComponentType AddComponent<TComponentType>(this Component component) where TComponentType : Component
    {
        return component.gameObject.AddComponent<TComponentType>();
    }

    /// <summary>
    /// Returns the component if it exists otherwise returns a new component
    /// </summary>
    public static TComponentType AddOrGetComponent<TComponentType>(this Component component) where TComponentType : Component
    {
        return component.gameObject.GetComponent<TComponentType>() ?? component.gameObject.AddComponent<TComponentType>();
    }

    /// <summary>
    /// Return true if the gameObject has the component attached to it
    /// </summary>
    public static bool HasComponent<TComponentType>(this GameObject component) where TComponentType : Component
    {
        return component.gameObject.GetComponent<TComponentType>() != null;
    }
}
