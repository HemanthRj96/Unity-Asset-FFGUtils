using System;


namespace FFG.Controllers
{
    public interface IState
    {
        Action OnStateUpdate { get; }
        Action OnStateFixedUpdate { get; }

        void StateUpdate();
    } 
}