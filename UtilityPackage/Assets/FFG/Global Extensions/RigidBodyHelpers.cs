using UnityEngine;


public static class RigidBodyHelpers
{
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
}
