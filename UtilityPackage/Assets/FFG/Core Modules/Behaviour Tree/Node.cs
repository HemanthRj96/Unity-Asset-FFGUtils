using UnityEngine;


namespace FFG.BehaviourTree
{
    public abstract class Node : ScriptableObject
    {
        public string Guid;
        public EState State = EState.Running;
        public bool HasStarted = false;
        public Vector2 Position;


        public EState Update()
        {
            if (HasStarted == false)
            {
                OnStart();
                HasStarted = true;
            }

            State = OnUpdate();

            if (State == EState.Failure || State == EState.Success)
            {
                HasStarted = false;
                OnStop();
            }

            return State;
        }

        public abstract void OnStart();
        public abstract void OnStop();
        public abstract EState OnUpdate();
    }
}