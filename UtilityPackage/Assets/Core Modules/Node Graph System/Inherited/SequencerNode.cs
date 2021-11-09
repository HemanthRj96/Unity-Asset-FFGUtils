using FickleFrames.Systems.Internal;
using UnityEngine;

namespace FickleFrames.Systems
{
    public class SequencerNode : CompositeNode
    {
        private int currentIndex = 0;

        protected override void OnStart() 
        {
            currentIndex = 0;
        }

        protected override void OnStop() { }

        protected override EStates OnUpdate()
        {
            switch (Children[currentIndex].Update())
            {
                case EStates.Running:
                    return EStates.Running;
                case EStates.Failure:
                    return EStates.Failure;
                case EStates.Success:
                    currentIndex++;
                    break;
            }
            return currentIndex == Children.Count ? EStates.Success : EStates.Running;
        }
    }
}