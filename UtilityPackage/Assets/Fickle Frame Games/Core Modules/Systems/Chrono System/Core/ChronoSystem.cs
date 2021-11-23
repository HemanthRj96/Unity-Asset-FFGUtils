using System;
using System.Collections.Generic;
using UnityEngine;


namespace FickleFrameGames.Systems
{
    public static class ChronoSystem
    {
        private class MonoBehaviourHook : MonoBehaviour
        {
            public event Action OnAwake = delegate { };
            public event Action OnUpdate = delegate { };

            private void Awake() => OnAwake();
            private void Update() => OnUpdate();
            public void MakePersistent() => DontDestroyOnLoad(this);
        }

        private class ChronoTimer
        {
            public ChronoTimer(string timerTag, Action callBack, float timerDuration)
            {
                Tick = tick;
                _timerTag = timerTag;
                _callBack = callBack;
                _timerDuration = timerDuration;
                _elapsedTime = 0f;
                _timerState = ETimerState.Running;
            }

            public Action Tick;
            private Action _callBack = null;
            private string _timerTag;
            private float _timerDuration;
            private float _elapsedTime;
            private ETimerState _timerState;

            private void stopTimer()
            {
                _timerState = ETimerState.Stop;
                _callBack?.Invoke();
                s_timerLookup.TryRemove(_timerTag);
            }

            private void tick()
            {
                switch (_timerState)
                {
                    case ETimerState.Running:
                        _elapsedTime += Time.deltaTime;
                        break;
                    case ETimerState.Stop:
                        return;
                }

                if (_timerDuration == -1)
                    return;
                if (_elapsedTime >= _timerDuration)
                    stopTimer();
            }

            public void TimerHandle(bool shouldPause = false) => _timerState = shouldPause ? ETimerState.Stop : ETimerState.Running;

            public float GetElapsedTime()
            {
                return _elapsedTime;
            }

            public float GetElapsedPercentage()
            {
                if (_timerDuration == -1)
                    return -1;
                return _elapsedTime * 100 / _timerDuration;
            }

            public void AddTime(float time)
            {
                _timerDuration += time;
            }

            public void DeleteTimer() => stopTimer();
        }

        /*.............................................Private Fields.......................................................*/

        private static Dictionary<string, ChronoTimer> s_timerLookup = new Dictionary<string, ChronoTimer>();
        private static MonoBehaviourHook s_hook = null;

        /*.............................................Private Methods......................................................*/

        /// <summary>
        /// Initializes MonobehaviourHook
        /// </summary>
        private static void initializeHook()
        {
            if (s_hook != null)
                return;

            var gameobject = new GameObject("ChronoSystemHelper", typeof(MonoBehaviourHook));
            s_hook = gameobject.GetComponent<MonoBehaviourHook>();
            s_hook.MakePersistent();
        }


        /// <summary>
        /// Creates a timer and returns the timer object
        /// </summary>
        /// <param name="timerTag">Name for the timer</param>
        /// <param name="callback">Callback function upon timer end</param>
        /// <param name="duration">Duration for timer</param>
        /// <returns></returns>
        private static ChronoTimer timerConstructor(string timerTag, Action callback = null, float duration = -1)
        {
            var timer = new ChronoTimer(timerTag, callback, duration);
            s_hook.OnUpdate += timer.Tick;
            return timer;
        }


        /// <summary>
        /// Returns a timer if it exists otherwise null
        /// </summary>
        /// <param name="timerTag"></param>
        /// <returns></returns>
        private static ChronoTimer getTimer(string timerTag)
        {
            return s_timerLookup.GetOrNull(timerTag);
        }


        /// <summary>
        /// Returns true if the timer tag is unique
        /// </summary>
        private static bool validTimer(string timerTag)
        {
            return !s_timerLookup.ContainsKey(timerTag);
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Method to create new countdown timer
        /// </summary>
        /// <param name="timerTag">Unique name of the timer</param>
        /// <param name="callBack">Callback method upon timer completion</param>
        /// <param name="countdownTime">Duration of countdown</param>
        public static void CreateNewCountdownTimer(string timerTag, Action callBack = null, float countdownTime = -1)
        {
            initializeHook();
            if (validTimer(timerTag))
                s_timerLookup.Add(timerTag, timerConstructor(timerTag, callBack, countdownTime));
        }


        /// <summary>
        /// Method to create new timer
        /// </summary>
        /// <param name="timerTag">Unique name of the timer</param>
        public static void CreateNewTimer(string timerTag)
        {
            initializeHook();
            if (validTimer(timerTag))
                s_timerLookup.Add(timerTag, timerConstructor(timerTag));
        }


        /// <summary>
        /// Returns true if a timer with the target timer exists
        /// </summary>
        /// <param name="timerTag">Unique tag for the timer</param>
        public static bool IsValidTimer(string timerTag)
        {
            return s_timerLookup.ContainsKey(timerTag);
        }


        /// <summary>
        /// Method to delete timer
        /// </summary>
        /// <param name="timerTag">Unnique name of the timer</param>
        public static void StopTimer(string timerTag)
        {
            getTimer(timerTag)?.DeleteTimer();
        }


        /// <summary>
        /// Pauses timer
        /// </summary>
        /// <param name="timerTag">Unique name of the timer</param>
        public static void PauseTimer(string timerTag)
        {
            var timer = getTimer(timerTag);
            if (timer != null)
                timer.TimerHandle(true);
        }


        /// <summary>
        /// Resumes timer
        /// </summary>
        /// <param name="timerTag">Unique name of the timer</param>
        public static void ResumeTimer(string timerTag)
        {
            var timer = getTimer(timerTag);
            if (timer != null)
                timer.TimerHandle(false);
        }


        /// <summary>
        /// Returns the elapsed time of a unique timer
        /// </summary>
        /// <param name="timertag">Unique name of the timer</param>
        public static float GetElapsedTime(string timertag)
        {
            var timer = getTimer(timertag);
            if (timer == null)
                return -1;
            else
                return timer.GetElapsedTime();
        }


        /// <summary>
        /// Returns the elapsed time percentage of a unique timer
        /// </summary>
        /// <param name="timerTag">Unique name of the timer</param>
        /// <returns></returns>
        public static float GetElapsedPercentage(string timerTag)
        {
            var timer = getTimer(timerTag);
            if (timer == null)
                return -1;
            else
                return timer.GetElapsedPercentage();
        }


        /// <summary>
        /// Method to add additional time to the timer duration
        /// </summary>
        /// <param name="timerTag">Unique name of the timer</param>
        /// <param name="additionalTime">Additional amount of time</param>
        public static void AddTime(string timerTag, float additionalTime)
        {
            var timer = getTimer(timerTag);
            if (timer != null)
                timer.AddTime(additionalTime);
        }
    }
}