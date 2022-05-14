using UnityEngine;


public static class CameraHelpers
{
    /// <summary>
    /// Returns the orthographic camera bounds in worldspace
    /// </summary>
    public static Vector4 GetOrthoCameraBoundsInWorldSpace(this Camera camera)
    {
        float v = camera.orthographicSize;
        float h = v * Screen.width / Screen.height;

        return new Vector4(-h, -v, h, v);
    }

    /// <summary>
    /// Get the horizontal FOV from the stereo camera
    /// </summary>
    public static float GetHorizontalFieldOfViewRadians(this Camera camera)
    {
        return 2f * Mathf.Atan(Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad * 0.5f) * camera.aspect);
    }
}
