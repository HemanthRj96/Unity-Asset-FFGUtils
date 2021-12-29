using UnityEngine;


namespace FFG.Systems.Internal
{
    public class TransformRecordData
    {
        public TransformRecordData(Transform targetTransform)
        {
            _targetTransform = targetTransform;
            _position = new VectTimeline();
            _rotation = new QuatTimeline();
            _scale = new VectTimeline();
        }

        private VectTimeline _position;
        private QuatTimeline _rotation;
        private VectTimeline _scale;
        private Transform _targetTransform;
        private Vector2 _timeStamp;

        public void Record()
        {
            _position.Add(_targetTransform.position);
            _rotation.Add(_targetTransform.rotation);
            _scale.Add(_targetTransform.localScale);
        }

        public void UpdateTransform(float time)
        {
            _targetTransform.position = GetPosition(time);
            _targetTransform.rotation = GetRotation(time);
            _targetTransform.localScale = GetScale(time);
        }

        public Vector3 GetPosition(float time) => _position.Get(time);

        public Quaternion GetRotation(float time) => _rotation.Get(time);

        public Vector3 GetScale(float time) => _scale.Get(time);

        public void SetTimeStamp(Vector2 timeStamp) => _timeStamp = timeStamp;

        public Vector2 GetTimeStamp() => _timeStamp;
    }
}