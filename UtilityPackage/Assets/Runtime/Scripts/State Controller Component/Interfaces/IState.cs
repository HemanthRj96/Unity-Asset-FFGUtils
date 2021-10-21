using System;


namespace FickleFrames.Controllers
{
    public interface IState
    {
        Action onStateUpdate { get; }
        Action onStateFixedUpdate { get; }

        string GetState();
        void StateUpdate();
    } 
}