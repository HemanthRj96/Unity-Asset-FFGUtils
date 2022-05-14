using System.Collections;
using UnityEngine;

namespace FFG.BehaviourTree
{
    public class WaitNode : ActionNode
    {
        [Min(0f)]
        public float Duration = 0;

        private float _startTime = 0;

        public override void OnStart()
        {
            _startTime = Time.time;
        }

        public override void OnStop() { }

        public override EState OnUpdate()
        {
            if(Time.time - _startTime < Duration)
                return EState.Running;
            else
                return EState.Success;
        }
    }
}