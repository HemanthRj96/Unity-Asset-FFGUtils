using UnityEngine;


namespace FFG.TrasformRecorder.Internal
{
    /// <summary>
    /// Container class to store tranform in the form of a timeline
    /// </summary>
    public class AnalogTransform
    {
        public AnalogVector Position;
        public AnalogQuaternion Rotation;
        public AnalogVector Scale;
        public Vector2 TimeStamp;

        public AnalogTransform()
        {
            Position = new AnalogVector();
            Rotation = new AnalogQuaternion();
            Scale = new AnalogVector();
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Method to record transform
        /// </summary>
        /// <param name="transform">Target tranform</param>
        /// <param name="time">Target time</param>
        public void Record(Transform transform, float time)
        {
            Position.Add(transform.position, time);
            Rotation.Add(transform.rotation, time);
            Scale.Add(transform.localScale, time);
        }

        /// <summary>
        /// Call this method to change transform based on timeline
        /// </summary>
        /// <param name="transform">Target tranform to be updated</param>
        /// <param name="time">Target time in the timeline</param>
        public void UpdateTransform(Transform transform, float time)
        {
            transform.position = GetPosition(time);
            transform.rotation = GetRotation(time);
            transform.localScale = GetScale(time);
        }

        /// <summary>
        /// Return the position from the timeline
        /// </summary>
        /// <param name="time">Target time in the timeline</param>
        public Vector3 GetPosition(float time) => Position.Get(time);

        /// <summary>
        /// Return the rotation from the timeline
        /// </summary>
        /// <param name="time">Target time in the timeline</param>
        public Quaternion GetRotation(float time) => Rotation.Get(time);

        /// <summary>
        /// Returns the scale from the timeline
        /// </summary>
        /// <param name="time">Target time in the timeline</param>
        public Vector3 GetScale(float time) => Scale.Get(time);

        /// <summary>
        /// Method to set the time stamp for this recorded data
        /// </summary>
        /// <param name="timeStamp">Vector2 with 'x' as starting time and 'y' as ending time</param>
        public void SetTimeStamp(Vector2 timeStamp) => TimeStamp = timeStamp;

        /// <summary>
        /// Returns the time stamp of this recorded data as Vector2 where x is the start time and y is the end time
        /// </summary>
        public Vector2 GetTimeStamp() => TimeStamp;
    }
}