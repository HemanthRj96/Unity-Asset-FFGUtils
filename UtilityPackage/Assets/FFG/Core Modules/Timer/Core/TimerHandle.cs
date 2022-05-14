using System;
using UnityEngine;


namespace FFG
{
    /// <summary>
    /// Handle to a timer used to get overall progress, pause, resume, add and deduct time from the timer
    /// </summary>
    public class TimerHandle
    {
        #region Constructors


        public TimerHandle(out Action tick)
        {
            _callback = null;
            _duration = -1;
            _elapsedTime = -0.02f;
            _previousTime = Time.time;
            _canTick = true;

            tick = this.tick;
        }

        public TimerHandle(Action callback, float duration, bool isLap, out Action tick)
        {
            _callback = callback;
            _duration = duration;
            _elapsedTime = -0.02f;
            _previousTime = Time.time;
            _canTick = true;
            _isLap = isLap;

            tick = this.tick;
        }


        #endregion
        #region Fields & Properties


        private Action _callback;
        private float _duration;
        private float _elapsedTime;
        private float _previousTime;
        private bool _canTick;
        private bool _isLap = false;


        #endregion
        #region Private Methods


        private void tick()
        {
            if (!_canTick)
                return;

            _elapsedTime += Time.deltaTime;

            if (_duration == -1)
                return;
            else if (_elapsedTime >= _duration)
            {
                if (_isLap)
                {
                    float err = (Time.time - _previousTime) - _duration;
                    _elapsedTime = err;
                    _previousTime = Time.time - err;
                    _callback();
                }
                else
                {
                    _canTick = false;
                    _callback();
                }
            }
        }


        #endregion
        #region Public Methods


        /// <summary>
        /// Returns the actual elapsed time
        /// </summary>
        public float GetElapsedTime() => _elapsedTime;

        /// <summary>
        /// Returns the progress from 0 to 1 where 0 being incomplete and 1 being complete
        /// </summary>
        public float GetProgress() => Mathf.Clamp01(_elapsedTime / _duration);

        /// <summary>
        /// Call this method to pause timer
        /// </summary>
        public void PauseTimer() => _canTick = false;

        /// <summary>
        /// Call this method to resume timer
        /// </summary>
        public void ResumeTimer() => _canTick = true; 


        #endregion
    }
}