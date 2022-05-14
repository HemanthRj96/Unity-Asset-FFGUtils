using UnityEditor;
using FFG;


namespace FFG_Editors
{
    [CustomEditor(typeof(TimerComponent))]
    public class TimerEditor : BaseEditor<TimerEditor>
    {
        public override void InspectorUpdate()
        {
            SerializedProperty timerMode = GetProperty("_timerMode");
            SerializedProperty duration = GetProperty("_duration");
            SerializedProperty unityEvent = GetProperty("_callBack");

            Heading("Timer Settings");
            Space(10);
            PropertyField(timerMode, "Timer Mode : ", "Select the mode this timer has to be run.");

            switch ((TimerMode)timerMode.enumValueIndex)
            {
                case TimerMode.Simple:
                    break;
                case TimerMode.Lap:
                    {
                        Space(5);
                        PropertyField(duration, "Target Lap Time : ", "Duration between timer repeat");
                        Space(5);
                        PropertyField(unityEvent, "Callback on complete : ", "Callback on each lap time");
                    }
                    break;
                case TimerMode.Countdown:
                    {
                        Space(5);
                        PropertyField(duration, "Target Delay Time : ", "Duration before invoking the event");
                        Space(5);
                        PropertyField(unityEvent, "Callback on complete : ", "Callback upon timer complete");
                    }
                    break;
            }
        }
    }
}
