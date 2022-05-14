using System.Collections.Generic;
using UnityEngine;


public static class VectorHelpers
{
    /// <summary>
    /// Returns Vector2 with x and y components
    /// </summary>
    public static Vector2 XY(this Vector3 v)
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
    
    /// <summary>
    /// Returns the position closest to the given one.
    /// </summary>
    /// <param name="position">Target position</param>
    /// <param name="otherPositions">Other world positions.</param>
    public static Vector3 GetClosest(this Vector3 position, IEnumerable<Vector3> otherPositions)
    {
        var closest = Vector3.zero;
        var shortestDistance = Mathf.Infinity;

        foreach (var otherPosition in otherPositions)
        {
            var distance = (position - otherPosition).sqrMagnitude;

            if (distance < shortestDistance)
            {
                closest = otherPosition;
                shortestDistance = distance;
            }
        }
        return closest;
    }
}
