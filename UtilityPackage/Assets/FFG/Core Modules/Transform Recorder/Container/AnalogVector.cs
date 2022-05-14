using UnityEngine;


namespace FFG.TrasformRecorder.Internal
{
    /// <summary>
    /// Container class to store vector in the form of a timeline
    /// </summary>
    public class AnalogVector
    {
        public AnimationCurve X;
        public AnimationCurve Y;
        public AnimationCurve Z;

        public AnalogVector()
        {
            X = new AnimationCurve();
            Y = new AnimationCurve();
            Z = new AnimationCurve();
        }

        /// <summary>
        /// Stores a vector in the timeline
        /// </summary>
        /// <param name="vector">Target vector</param>
        /// <param name="time">Current time</param>
        public void Add(Vector3 vector, float time)
        {
            X.AddKey(time, vector.x);
            Y.AddKey(time, vector.y);
            Z.AddKey(time, vector.z);
        }

        /// <summary>
        /// Returns the total number of keys in the timeline
        /// </summary>
        public int GetKeyCount() => X.length;

        /// <summary>
        /// Returns the Vector from the timeline
        /// </summary>
        /// <param name="time">Target time in the timeline</param>
        public Vector3 Get(float time) => new Vector3(X.Evaluate(time), Y.Evaluate(time), Z.Evaluate(time));
    }
}