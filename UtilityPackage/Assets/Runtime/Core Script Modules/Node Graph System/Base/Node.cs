using System;
using UnityEngine;


namespace FickleFrames.Systems.Internal
{
    public abstract class Node : ScriptableObject
    {
        public EStates State = EStates.Running;
        public bool Started = false;

        public EStates Update()
        {
            if(!Started)
            {
                Started = true;
                OnStart();
            }
            State = OnUpdate();
            if (State == EStates.Success || State == EStates.Failure)
            {
                Started = false;
                OnStop();
            }
            return State;
        }

        protected abstract void OnStart();
        protected abstract EStates OnUpdate();
        protected abstract void  OnStop();
    }
}