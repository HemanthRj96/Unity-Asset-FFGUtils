using UnityEngine;


namespace FFG.BehaviourTree
{
    public class RepeatNode : DecoratorNode
    {
        [Min(0)]
        public int RepeatCount = 0;
        public bool bUseCondition = false;
        public EState RepeatIfStateIs;

        private int _loopCounter = 0;

        public override void OnStart() 
        {
            _loopCounter = 0;
        }

        public override void OnStop() { }

        public override EState OnUpdate()
        {
            if (RepeatCount == 0)
            {
                Child.Update();
                return EState.Running;
            }
            else if (_loopCounter < RepeatCount)
            {
                if (bUseCondition)
                {
                    if (Child.Update() != RepeatIfStateIs)
                        return EState.Failure;
                    else
                    {
                        ++_loopCounter;
                        return EState.Running;
                    }
                }
                else
                {
                    Child.Update();
                    ++_loopCounter;
                    return EState.Running;
                }
            }
            else
                return EState.Success;
        }
    }
}