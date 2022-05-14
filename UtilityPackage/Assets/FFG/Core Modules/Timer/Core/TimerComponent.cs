using UnityEngine;
using UnityEngine.Events;


namespace FFG
{
    public enum TimerMode
    {
        Simple,
        Countdown,
        Lap,
    }

    public class TimerComponent : MonoBehaviour
    {
        #region Fields & Properties


        [SerializeField]
        private TimerMode _timerMode = TimerMode.Simple;
        [SerializeField, Min(0)]
        private float _duration = 0;
        [SerializeField]
        private UnityEvent _callBack = null;


        public TimerHandle TimerHandle { get; private set; }


        #endregion
        #region Private Methods


        private void Awake()
        {
            switch (_timerMode)
            {
                case TimerMode.Lap:
                    TimerHandle = Timer.CreateLapTimer(onTimerInvoke, _duration);
                    break;
                case TimerMode.Simple:
                    TimerHandle = Timer.CreateTimer();
                    break;
                case TimerMode.Countdown:
                    TimerHandle = Timer.CreateCountdownTimer(onTimerInvoke, _duration);
                    break;
            }
        }

        private void onTimerInvoke() => _callBack?.Invoke(); 


        #endregion
    }
}