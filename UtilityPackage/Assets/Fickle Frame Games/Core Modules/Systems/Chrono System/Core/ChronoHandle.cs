using System;
using UnityEngine;


namespace FickleFrameGames.Systems
{
    public class ChronoHandle
    {
        /*.............................................Constructor..........................................................*/

        public ChronoHandle(Action callback, float duration, ETimerMode mode)
        {
            _callback = delegate { };
            _callback += callback;
            _duration = Mathf.Max(0.002f, duration);
            _mode = mode;
            _elapsedTime = -0.02f;
            _previousTime = Time.time;
            _canTick = true;
        }

        /*.............................................Private Fields.......................................................*/

        private event Action _callback;
        private ETimerMode _mode;

        private float _duration;
        private float _elapsedTime;
        private float _previousTime;

        private bool _canTick;

        /*.............................................Properties...........................................................*/

        /// <summary>
        /// Don't read this prperty if you don't know what you're doing
        /// </summary>
        public Action Tick { get { return tick; } }

        /*.............................................Private Methods......................................................*/

        /// <summary>
        /// Ticking method where elapsed time is added every frame
        /// </summary>
        private void tick()
        {
            if (!_canTick)
                return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _duration)
            {
                switch (_mode)
                {
                    case ETimerMode.Repeating:
                        {
                            float err = (Time.time - _previousTime) - _duration;
                            _elapsedTime = err;
                            _callback();
                            _previousTime = Time.time - err;
                            return;
                        }
                    case ETimerMode.Single:
                        _canTick = false;
                        _callback();
                        return;
                }
            }
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Returns the actual elapsed time
        /// </summary>
        public float GetElapsedTime() => _elapsedTime;


        /// <summary>
        /// Returns the progress from 0 to 1 where 0 being incomplete and 1 being complete
        /// </summary>
        public float GetProgress() => Mathf.Clamp01(_elapsedTime / _duration);


        /// <summary>
        /// Call this method to add time to an already running timer. If timer has already passed the set duration then calling 
        /// this method is useless
        /// </summary>
        /// <param name="amount">
        /// Amount of time to be added, the sign of this value isn't relevant since the value is changed to absolute value
        /// </param>
        public float AddTime(float amount) => _duration += Mathf.Abs(amount);


        /// <summary>
        /// Call this method to deduct time to an already running timer. If timer has already passed the set duration then calling 
        /// this method is useless
        /// </summary>
        /// <param name="amount">
        /// Amount of time to be deducted, the sign of this value isn't relevant since the value is changed to absolute value
        /// </param>
        public float DeductTime(float amount) => _duration += Mathf.Abs(amount);


        /// <summary>
        /// Call this method to pause timer
        /// </summary>
        public void PauseTimer() => _canTick = false;


        /// <summary>
        /// Call this method to resume timer
        /// </summary>
        public void ResumeTimer() => _canTick = true;
    }
}