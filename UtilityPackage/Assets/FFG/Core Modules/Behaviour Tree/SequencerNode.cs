namespace FFG.BehaviourTree
{
    public class SequencerNode : CompositeNode
    {
        private int _currentIndex = 0;

        public override void OnStart()
        {
            _currentIndex = 0;
        }

        public override void OnStop() { }

        public override EState OnUpdate()
        {
            var child = Children[_currentIndex];

            switch (child.Update())
            {
                case EState.Running:
                    return EState.Running;
                case EState.Failure:
                    return EState.Failure;
                case EState.Success:
                    _currentIndex++;
                    break;
            }
            return _currentIndex == Children.Count? EState.Success : EState.Running;
        }
    }
}