using UnityEngine;


namespace FFG.Systems.Internal
{
    public class QuatTimeline
    {
        private AnimationCurve _x;
        private AnimationCurve _y;
        private AnimationCurve _z;
        private AnimationCurve _w;

        public void Add(Quaternion angle)
        {
            float time = TransformRecorder.GetCurrentTime();
            _x.AddKey(time, angle.x);
            _y.AddKey(time, angle.y);
            _z.AddKey(time, angle.z);
            _w.AddKey(time, angle.w);
        }

        public Quaternion Get(float time)
        {
            return new Quaternion(_x.Evaluate(time), _y.Evaluate(time), _z.Evaluate(time), _w.Evaluate(time));
        }
    }
}