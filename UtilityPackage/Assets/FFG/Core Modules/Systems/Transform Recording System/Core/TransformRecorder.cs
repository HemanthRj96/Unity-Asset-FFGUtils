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

    public class TransformRecorder : MonoBehaviour
    {
        [SerializeField]
        private float _recordRate = 60;
        [SerializeField]
        private TransformRecordDataBank _recordingSaver = null;
        [SerializeField]
        private GameObject _targetGameObject = null;

        private static float s_startTime = 0;
        private static float s_endTime = 0;

        private TransformRecordData _transformData = null;

        private ERecorderStates _states = ERecorderStates.Stopped;

        private bool _recorderValidity = false;
        private bool _canRecord = false;


        private void Awake()
        {
            if (_recordingSaver == null || _targetGameObject == null)
                Debug.LogError("Saving system do not exist please create and intialize for the recordings to be saved");
            else
            {
                _recorderValidity = true;
                _states = ERecorderStates.Paused;
                StartCoroutine(recorder());
                
            }
        }

        private IEnumerator recorder()
        {
            while (true)
            {
                if (_states == ERecorderStates.Active)
                {
                    yield return new WaitForSeconds(1 / _recordRate);
                    _transformData.Record();
                }
                else if (_states == ERecorderStates.Stopped)
                    break;
                else
                    continue;
            }
        }


        // Public methods

        /// <summary>
        /// Returns the current time
        /// </summary>
        public static float GetCurrentTime() => Time.time - s_startTime;

        /// <summary>
        /// Method to set target recorder subject at runtime
        /// </summary>
        /// <param name="gameObject">Target gameObject to be recorded</param>
        public void SetTargetObject(GameObject gameObject)
        {
            _targetGameObject = gameObject;
        }

        public void SetTargetRecorderSaver(TransformRecordDataBank saver)
        {

        }

    }
}