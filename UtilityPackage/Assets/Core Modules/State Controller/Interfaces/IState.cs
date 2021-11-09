using System;


namespace FickleFrames.Controllers
{
    public interface IState
    {
        Action OnStateUpdate { get; }
        Action OnStateFixedUpdate { get; }

        string GetState();
        void StateUpdate();
    } 
}