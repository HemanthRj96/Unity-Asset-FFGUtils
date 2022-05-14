using UnityEngine;


namespace FFG.BehaviourTree
{
    public class DebugErrorNode : ActionNode
    {
        public string Message;

        public override void OnStart()
        {
            Debug.LogError(Message);
        }

        public override void OnStop()
        {
            Debug.LogError(Message);
        }

        public override EState OnUpdate()
        {
            Debug.LogError(Message);
            return EState.Failure;
        }
    }
}