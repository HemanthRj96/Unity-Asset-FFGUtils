using System;
using UnityEngine;


namespace FFG
{
    /// <summary>
    /// Use this class to create function timers
    /// </summary>
    public static class Timer
    {
        #region Internal Classes


        /// <summary>
        /// Hook component which is used to hook the TimerHandles to update method
        /// </summary>
        private class MonoBehaviourHook : MonoBehaviour
        {
            public Action OnUpdate = delegate { };

            private void Update() => OnUpdate();

            public void MakePersistent() => DontDestroyOnLoad(this);
        }


        #endregion
        #region Fields & Properties


        private static MonoBehaviourHook s_hook = null;


        #endregion
        #region Private Methods


        private static void init()
        {
            var gameobject = new GameObject("-Timer-MonobehaviourHook-", typeof(MonoBehaviourHook));
            s_hook = gameobject.GetComponent<MonoBehaviourHook>();
            s_hook.MakePersistent();
        }

        #endregion
        #region Public Methods

        /// <summary>
        /// Creates a countdown timer which invoked the callback after duration (seconds)
        /// </summary>
        /// <param name="callback">Method to be called after timer completes</param>
        /// <param name="duration">Total duration of timer</param>
        /// <returns>Returns the timer handle which can be used to pause, resume, and know the elapsed time</returns>
        public static TimerHandle CreateCountdownTimer(Action callback, float duration)
        {
            if (s_hook == null)
                init();

            var handle = new TimerHandle(callback, duration, false, out var tick);
            s_hook.OnUpdate += tick;
            return handle;
        }

        /// <summary>
        /// Creates a timer that'll run forever. Used generally to run a background timer without any duration or callbacks
        /// </summary>
        /// <returns>Returns the timer handle which can be used to pause, resume, and know the elapsed time</returns>
        public static TimerHandle CreateTimer()
        {
            if (s_hook == null)
                init();

            var handle = new TimerHandle(out var tick);
            s_hook.OnUpdate += tick;
            return handle;
        }

        /// <summary>
        /// Creates a repeating timer which invokes callback every duration (seconds)
        /// </summary>
        /// <param name="callback">Method to be called after timer completes</param>
        /// <param name="lapTime">Duration between laps</param>
        /// <returns>Returns the timer handle which can be used to pause, resume, and know the elapsed time</returns>
        public static TimerHandle CreateLapTimer(Action callback, float lapTime)
        {
            if (s_hook == null)
                init();

            var handle = new TimerHandle(callback, lapTime, true, out var tick);
            s_hook.OnUpdate += tick;
            return handle;
        }


        #endregion
    }
}