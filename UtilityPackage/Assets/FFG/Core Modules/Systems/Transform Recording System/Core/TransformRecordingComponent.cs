using FFG.Systems.Internal;
using System.Collections;
using UnityEngine;


namespace FFG.Systems
{
    public enum ERecorderStates
    {
        Active,
        Paused,
        Stopped
    }

    public class TransformRecordingComponent : MonoBehaviour
    {
        /*.............................................Serialized Fields....................................................*/

        [SerializeField]
        private bool _startRecorderOnAwake = false;

        [SerializeField]
        private float _recordRate = 24;

        [SerializeField]
        private KeyCode _startRecordKey;
        [SerializeField]
        private KeyCode _pauseRecordKey;
        [SerializeField]
        private KeyCode _stopRecordKey;
        [SerializeField]
        private KeyCode _replayKey;

        [SerializeField]
        private string _savePath;

        /*.............................................Private Fields.......................................................*/

        private float _startTime = 0;
        private float _endTime = 0;
        private float _pauseTime = -1;
        private float _totalPauseTime = 0;

        private AnalogTransform _transformData = null;
        private ERecorderStates _states = ERecorderStates.Stopped;

        private bool _isNewRecording = true;
        private bool _isReplaying = false;

        private Transform _anotherTransform = null;

        /*.............................................Private Methods......................................................*/

        /// <summary>
        /// Start coroutines if it should
        /// </summary>
        private void Awake()
        {
            _states = ERecorderStates.Paused;

            if (_startRecorderOnAwake)
                StartCoroutine(recorder());
        }

        /// <summary>
        /// Check for the inputs
        /// </summary>
        private void Update()
        {
            if (_isReplaying)
                return;

            if (Input.GetKeyDown(_startRecordKey))
            {
                if (_startRecorderOnAwake == false)
                    StartCoroutine(recorder());
                Record();
            }
            else if (Input.GetKeyDown(_pauseRecordKey))
                PauseRecord();
            else if (Input.GetKeyDown(_stopRecordKey))
                StopRecord();
            else if (Input.GetKeyDown(_replayKey))
                Replay();
        }

        /// <summary>
        /// Recorder function routine
        /// </summary>
        private IEnumerator recorder()
        {
            while (true)
            {
                yield return new WaitForSeconds(1 / _recordRate);

                if (_states == ERecorderStates.Active)
                {
                    if (_isNewRecording)
                    {
                        _isNewRecording = false;
                        _startTime = Time.time;
                        _transformData = new AnalogTransform();
                    }

                    if (_pauseTime != -1)
                    {
                        _totalPauseTime += Time.time - _pauseTime;
                        _pauseTime = -1;
                    }

                    if (_anotherTransform == null)
                        _transformData.Record(transform, Time.time - _startTime - _totalPauseTime);
                    else
                        _transformData.Record(_anotherTransform, Time.time - _startTime - _totalPauseTime);
                }
                else if (_states == ERecorderStates.Stopped)
                {
                    _isNewRecording = true;
                    _endTime = Time.time - _startTime - _totalPauseTime;
                    _transformData.SetTimeStamp(new Vector2(_startTime, _endTime));
                    AnalogTransformDataSaver.SaveData(_transformData, Application.dataPath + $"/{_savePath}/save.txt");
                    _transformData = null;
                    break;
                }
                else if (_states == ERecorderStates.Paused)
                {
                    if (!_isNewRecording && _pauseTime == -1)
                        _pauseTime = Time.time;
                    continue;
                }
            }
        }

        /// <summary>
        /// Replaying function routine
        /// </summary>
        private IEnumerator replayer()
        {
            float startTime = 0, endTime = 0, timer = 0;

            if (_states == ERecorderStates.Active)
                _states = ERecorderStates.Paused;

            if (_transformData == null)
            {
                _transformData = AnalogTransformDataSaver.GetData(Application.dataPath + $"/{_savePath}/save.txt");
                if (_transformData == null)
                {
                    startTime = -1;
                    endTime = -1;
                }
                else
                {
                    startTime = _transformData.GetTimeStamp().x;
                    endTime = _transformData.GetTimeStamp().y;
                }
            }
            else
            {
                int keyCount = _transformData.Position.GetKeyCount();

                if (keyCount == 0)
                    startTime = -1;
                else
                {
                    startTime = _startTime;
                    endTime = _transformData.Position.X.keys[keyCount - 1].time;
                }
            }

            if (startTime != -1)
            {
                _isReplaying = true;

                while (timer <= endTime)
                {
                    yield return new WaitForSeconds(1 / _recordRate);

                    if (_anotherTransform == null)
                        _transformData.UpdateTransform(transform, timer);
                    else
                        _transformData.UpdateTransform(_anotherTransform, timer);

                    timer += 1 / _recordRate;
                }
                _isReplaying = false;
            }

            yield return null;
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Call this method to begin the recording
        /// </summary>
        public void Record(Transform targetTransform = null)
        {
            if (targetTransform != null)
                _anotherTransform = targetTransform;
            _states = ERecorderStates.Active;
        }

        /// <summary>
        /// Call this method to pause the recording
        /// </summary>
        public void PauseRecord(Transform targetTransform = null)
        {
            if (targetTransform != null)
                _anotherTransform = targetTransform;
            _states = ERecorderStates.Paused;
        }

        /// <summary>
        /// Call this method to stop the recording
        /// </summary>
        public void StopRecord(Transform targetTransform = null)
        {
            if (targetTransform != null)
                _anotherTransform = targetTransform;
            _states = ERecorderStates.Stopped;
        }

        /// <summary>
        /// Call this method to replay the recording
        /// </summary>
        public void Replay(Transform targetTransform = null)
        {
            if (targetTransform != null)
                _anotherTransform = targetTransform;
            StartCoroutine(replayer());
        }
    }
}