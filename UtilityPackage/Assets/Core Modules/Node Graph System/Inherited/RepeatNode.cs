using FickleFrames.Systems.Internal;
using System.Collections;
using UnityEngine;

namespace FickleFrames.Systems
{
    public class RepeatNode : DecoratorNode
    {
        public int RepeatCount = -1;
        private int count = 0;

        protected override void OnStart()
        {
            count = 0;
        }

        protected override void OnStop() { }

        protected override EStates OnUpdate()
        {
            if (Child.Update() == EStates.Success)
            {
                if (RepeatCount == -1)
                    return EStates.Running;

                ++count;
                if (count == RepeatCount)
                    return EStates.Success;
            }
            return EStates.Running;
        }
    }
}