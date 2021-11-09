using FickleFrames.Systems.Internal;
using UnityEngine;

namespace FickleFrames.Systems
{
    public class DebugLogNode : ActionNode
    {
        public string LogMessage = default;
        public bool ShouldRunOnUpdate = false;

        protected override void OnStart()
        {
            if (!ShouldRunOnUpdate)
                Debug.Log(LogMessage);
        }

        protected override EStates OnUpdate()
        {
            if (ShouldRunOnUpdate)
            {
                Debug.Log(LogMessage);
                return EStates.Running;
            }
            else
                return EStates.Success;
        }

        protected override void OnStop() { }
    }
}