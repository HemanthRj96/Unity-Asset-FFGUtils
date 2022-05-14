using UnityEngine;


public static class GameObjectHelpers
{
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

    /// <summary>
    /// Returns the position of a gameobject
    /// </summary>
    public static Vector3 GetPosition(this GameObject gameObject)
    {
        return gameObject.transform.position;
    }

    /// <summary>
    /// Returns the position of a gameobject
    /// </summary>
    public static Vector3 GetLocalScale(this GameObject gameObject)
    {
        return gameObject.transform.localScale;
    }

    /// <summary>
    /// Returns the rotation of a gameobject
    /// </summary>
    public static Quaternion GetRotation(this GameObject gameObject)
    {
        return gameObject.transform.rotation;
    }

    /// <summary>
    /// Sets the position of a gameobject
    /// </summary>
    /// <param name="position">Target position</param>
    public static void SetPosition(this GameObject gameObject, Vector3 position)
    {
        gameObject.transform.position = position;
    }

    /// <summary>
    /// Sets the local scale of a gameobject
    /// </summary>
    /// <param name="scale">Target scale</param>
    public static void SetLocalScale(this GameObject gameObject, Vector3 scale)
    {
        gameObject.transform.localScale = scale;
    }

    /// <summary>
    /// Sets the rotation of a gameobject
    /// </summary>
    /// <param name="eaulerAngles">Target rotation</param>
    public static void SetRotation(this GameObject gameObject, Vector3 eaulerAngles)
    {
        gameObject.transform.rotation = Quaternion.Euler(eaulerAngles);
    }

    /// <summary>
    /// Clones this gameobject, careful when you clone self since it can lead to infinite cloning
    /// </summary>
    public static GameObject CloneThis(this GameObject gameObject)
    {
        var go = GameObject.Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
        go.transform.position = gameObject.transform.position;
        go.transform.rotation = gameObject.transform.rotation;
        go.transform.localScale = gameObject.transform.localScale;
        return go;
    }

    /// <summary>
    /// Returns distance between this gameObject and other
    /// </summary>
    /// <param name="other">Target gameObject</param>
    public static float DistanceTo(this GameObject gameObject, GameObject other)
    {
        return Vector3.Distance(gameObject.transform.position, other.transform.position);
    }

    /// <summary>
    /// Returns the distance between this gameObject and the target point
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="targetPoint"></param>
    public static float DistanceTo(this GameObject gameObject, Vector3 targetPoint)
    {
        return Vector3.Distance(gameObject.transform.position, targetPoint);
    }
}
