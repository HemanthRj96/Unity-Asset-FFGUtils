using UnityEngine;


namespace FFG.Systems.Internal
{
    /// <summary>
    /// Container class to store rotation in the form of a timeline
    /// </summary>
    public class AnalogQuaternion
    {
        public AnimationCurve X;
        public AnimationCurve Y;
        public AnimationCurve Z;
        public AnimationCurve W;

        public AnalogQuaternion()
        {
            X = new AnimationCurve();
            Y = new AnimationCurve();
            Z = new AnimationCurve();
            W = new AnimationCurve();
        }

        /// <summary>
        /// Stores rotation in timeline
        /// </summary>
        /// <param name="rotation">Target rotation</param>
        /// <param name="time">Current time</param>
        public void Add(Quaternion rotation, float time)
        {
            X.AddKey(time, rotation.x);
            Y.AddKey(time, rotation.y);
            Z.AddKey(time, rotation.z);
            W.AddKey(time, rotation.w);
        }

        /// <summary>
        /// Returns the total numberof keys
        /// </summary>
        public int GetKeyCount() => X.length;

        /// <summary>
        /// Returns the quaternion from the timeline
        /// </summary>
        /// <param name="time">Target time in the timeline</param>
        public Quaternion Get(float time) => new Quaternion(X.Evaluate(time), Y.Evaluate(time), Z.Evaluate(time), W.Evaluate(time));
    }
}