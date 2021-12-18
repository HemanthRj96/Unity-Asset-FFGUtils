using System;


namespace FickleFrameGames.Controllers
{
    public interface IState
    {
        Action OnStateUpdate { get; }
        Action OnStateFixedUpdate { get; }

        void StateUpdate();
    } 
}