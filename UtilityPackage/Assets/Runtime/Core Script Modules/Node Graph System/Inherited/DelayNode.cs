using FickleFrames.Systems.Internal;
using UnityEngine;

namespace FickleFrames.Systems
{
    public class DelayNode : ActionNode
    {
        public float Delay = 1;
        private float timer = 0;

        protected override void OnStart() 
        {
            timer = Time.time;
        }

        protected override void OnStop() { }

        protected override EStates OnUpdate()
        {
            if (Time.time - timer > Delay)
                return EStates.Success;
            return EStates.Running;
        }
    }
}