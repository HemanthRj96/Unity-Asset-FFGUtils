using System;
using UnityEngine;


public static class TransformHelpers
{
    /// <summary>
    /// Makes the given game objects children of the transform.
    /// </summary>
    /// <param name="children">Game objects to make children.</param>
    public static void AddChildren(this Transform transform, GameObject[] children)
    {
        Array.ForEach(children, child => child.transform.parent = transform);
    }

    /// <summary>
    /// Destroys children of this tranform
    /// </summary>
    public static void DestroyChildren(this Transform transform)
    {
        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);
    }

    /// <summary>
    /// Destroys the children of this transform immediately
    /// </summary>
    public static void DestroyChildrenImmediate(this Transform transform)
    {
        foreach (Transform child in transform)
            GameObject.DestroyImmediate(child.gameObject);
    }

    /// <summary>
    /// Resets the transform
    /// </summary>
    public static void Reset(this Transform transform)
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Sets the x component of the transform's position.
    /// </summary>
    /// <param name="x">Value of x.</param>
    public static void SetX(this Transform transform, float x)
    {
        Vector3 v = transform.position;
        v.x = x;
        transform.position = v;
    }

    /// <summary>
    /// Sets the y component of the transform's position.
    /// </summary>
    /// <param name="y">Value of y.</param>
    public static void SetY(this Transform transform, float y)
    {
        Vector3 v = transform.position;
        v.y = y;
        transform.position = v;
    }

    /// <summary>
    /// Sets the z component of the transform's position.
    /// </summary>
    /// <param name="z">Value of z.</param>
    public static void SetZ(this Transform transform, float z)
    {
        Vector3 v = transform.position;
        v.z = z;
        transform.position = v;
    }

    /// <summary>
    /// Sets the x component of the transform's localposition
    /// </summary>
    /// <param name="x">Value of x</param>
    public static void SetLocalX(this Transform transform, float x)
    {
        Vector3 v = transform.localPosition;
        v.x = x;
        transform.localPosition = v;
    }

    /// <summary>
    /// Sets the y component of the transform's localposition
    /// </summary>
    /// <param name="y">Value of y</param>
    public static void SetLocalY(this Transform transform, float y)
    {
        Vector3 v = transform.localPosition;
        v.y = y;
        transform.localPosition = v;
    }

    /// <summary>
    /// Sets the z component of the transform's localposition
    /// </summary>
    /// <param name="z">Value of z</param>
    public static void SetLocalZ(this Transform t, float z)
    {
        Vector3 v = t.localPosition;
        v.z = z;
        t.localPosition = v;
    }
}
