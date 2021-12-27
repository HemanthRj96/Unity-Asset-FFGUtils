using UnityEngine;


namespace FFG.Systems.Internal
{
    public class VectTimeline
    {
        private AnimationCurve _x;
        private AnimationCurve _y;
        private AnimationCurve _z;

        public void Add(Vector3 vector)
        {
            float time = TransformRecorder.GetCurrentTime();

            _x.AddKey(time, vector.x);
            _y.AddKey(time, vector.y);
            _z.AddKey(time, vector.z);
        }

        public Vector3 Get(float time)
        {
            return new Vector3(_x.Evaluate(time), _y.Evaluate(time), _z.Evaluate(time));
        }
    }
}