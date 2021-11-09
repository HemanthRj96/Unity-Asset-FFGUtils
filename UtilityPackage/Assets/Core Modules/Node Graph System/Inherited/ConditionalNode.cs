using FickleFrames.Systems.Internal;
using System.Collections;
using UnityEngine;


namespace FickleFrames.Systems
{
    public class ConditionalNode : CompositeNode
    {
        private int _index;
        private bool _conditionResult = false;
        private Node _onStop = null;

        protected override void OnStart()
        {
            _index = 0;
            // Set the onStop function as the second child if there's any
            if (Children.Count > 1)
                _onStop = Children[Children.Count - 1];
        }

        protected override void OnStop()
        {
            // If all conditions fail execute the last node
            if (!_conditionResult && _onStop)
                _onStop.Update();
        }

        protected override EStates OnUpdate()
        {
            switch (Children[_index].Update())
            {
                case EStates.Running:
                    return EStates.Running;
                case EStates.Failure:
                    ++_index;
                    break;
                case EStates.Success:
                    _conditionResult = true;
                    return EStates.Success;
            }

            return _index == Children.Count ? EStates.Failure : EStates.Running;
        }
    }
}