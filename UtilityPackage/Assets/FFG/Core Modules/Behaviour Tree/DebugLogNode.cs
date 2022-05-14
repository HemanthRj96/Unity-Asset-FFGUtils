using UnityEngine;


namespace FFG.BehaviourTree
{
    public class DebugLogNode : ActionNode
    {
        public string Message;

        public override void OnStart()
        {
            Debug.Log($"OnStart: {Message}");
        }

        public override void OnStop()
        {
            Debug.Log($"OnStop: {Message}");
        }

        public override EState OnUpdate()
        {
            Debug.Log($"OnUpdate: {Message}");
            return EState.Success;
        }
    }
}