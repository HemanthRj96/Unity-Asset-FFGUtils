using System;
using System.Collections.Generic;
using UnityEngine;


public static class ExtensionMethods
{

    #region Collection Extensions


    /// <summary>
    /// Prints all the values inside an array
    /// </summary>
    public static void PrintValues<TType>(this TType[] arr)
    {
        for (int i = 0; i < arr.Length; ++i)
            Debug.Log($"Item #{i} : {arr[i]}");
    }


    /// <summary>
    /// Prints all the value inside a list
    /// </summary>
    public static void PrintValues<TType>(this List<TType> list)
    {
        for (int i = 0; i < list.Count; ++i)
            Debug.Log($"Item #{i} : {list[i]}");
    }


    /// <summary>
    /// Removes an item if it exists
    /// </summary>
    public static void TryRemove<TType>(this List<TType> list, TType item)
    {
        if (list.Contains(item))
            list.Remove(item);
    }


    /// <summary>
    /// Adds an item if it doesn't exist
    /// </summary>
    public static void TryAdd<TType>(this List<TType> list, TType item)
    {
        if (!list.Contains(item))
            list.Add(item);
    }


    /// <summary>
    /// Shuffles elements in a list
    /// </summary>
    public static void Shuffle<TType>(this List<TType> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            TType value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    /// <summary>
    /// Prints all the values inside the dictionary
    /// </summary>
    public static void PrintValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
        foreach (var temp in dictionary)
            Debug.Log($"Key : {temp.Key}, Value : {temp.Value}");
    }


    /// <summary>
    /// Removes an item in dictionary if it exists
    /// </summary>
    public static void TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
    {
        if (dictionary.ContainsKey(key))
            dictionary.Remove(key);
    }


    /// <summary>
    /// Adds key and value if it doesn't exist
    /// </summary>
    public static void TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (!dictionary.ContainsKey(key))
            dictionary.Add(key, value);
    }


    /// <summary>
    /// Returns the value if it exists or null
    /// </summary>
    public static TValue GetOrNull<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : class
    {
        if (dictionary.ContainsKey(key))
            return dictionary[key];
        return null;
    }


    #endregion Colleciton Extensions


    #region RigidBody Extensions


    /// <summary>
    /// Changes the direction of velocty
    /// </summary>
    public static void ChangeDirection(this Rigidbody2D rb, Vector3 direction)
    {
        rb.velocity = direction * rb.velocity.magnitude;
    }


    /// <summary>
    /// Changes the velocity without changing the direction
    /// </summary>
    /// <param name="magnitude">Target velocity</param>
    public static void NormalizeVelocity(this Rigidbody rb, float magnitude = 1)
    {
        rb.velocity = rb.velocity.normalized * magnitude;
    }


    #endregion RigidBody Extensions


    #region GameObject Extensions


    /// <summary>
    /// Attaches a component to the given component's game object.
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
        return component.GetComponent<TComponentType>() ?? component.AddComponent<TComponentType>();
    }


    /// <summary>
    /// Checks whether a component's game object has a component of type T attached.
    /// </summary>
    public static bool HasComponent<TComponentType>(this Component component) where TComponentType : Component
    {
        return component.GetComponent<TComponentType>() != null;
    }


    /// <summary>
    /// Returns the component if it exists otherwise returns a new component
    /// </summary>
    public static TComponentType AddOrGetComponent<TComponentType>(this GameObject gameObject) where TComponentType : Component
    {
        return gameObject.GetComponent<TComponentType>() ?? gameObject.AddComponent<TComponentType>();
    }


    /// <summary>
    /// Return true if the gameObject has the component attached to it
    /// </summary>
    public static bool HasComponent<TComponentType>(this GameObject gameObject) where TComponentType : Component
    {
        return gameObject.GetComponent<TComponentType>() != null;
    }


    #endregion GameObject Extensions


    #region Transform Extensions


    /// <summary>
    /// Makes the given game objects children of the transform.
    /// </summary>
    /// <param name="children">Game objects to make children.</param>
    public static void AddChildren(this Transform transform, GameObject[] children)
    {
        Array.ForEach(children, child => child.transform.parent = transform);
    }


    /// <summary>
    /// Makes the game objects of given components children of the transform.
    /// </summary>
    /// <param name="children">Components of game objects to make children.</param>
    public static void AddChildren(this Transform transform, Component[] children)
    {
        Array.ForEach(children, child => child.transform.parent = transform);
    }


    #endregion Transform Extensions


    #region Vector Extensions


    /// <summary>
    /// Returns Vector2 with x and y components
    /// </summary>
    public static Vector2 xy(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }


    /// <summary>
    /// Sets the X axis value
    /// </summary>
    /// <param name="x">Target X axis value</param>
    public static Vector3 SetX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }


    /// <summary>
    /// Sets the Y axis value
    /// </summary>
    /// <param name="y">Target Y axis value</param>
    public static Vector3 SetY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }


    /// <summary>
    /// Sets the Z axis value
    /// </summary>
    /// <param name="z">Target Z axis value</param>
    public static Vector3 SetZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }


    /// <summary>
    /// Sets the X axis value
    /// </summary>
    /// <param name="x">Target X axis value</param>
    public static Vector2 SetX(this Vector2 v, float x)
    {
        return new Vector2(x, v.y);
    }


    /// <summary>
    /// Sets the Y axis value
    /// </summary>
    /// <param name="y">Target Y axis value</param>
    public static Vector2 SetY(this Vector2 v, float y)
    {
        return new Vector2(v.x, y);
    }


    /// <summary>
    /// Sets the Z axis value
    /// </summary>
    /// <param name="z">Target Z axis value</param>
    public static Vector3 SetZ(this Vector2 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }


    #endregion Vector Extensions

}
