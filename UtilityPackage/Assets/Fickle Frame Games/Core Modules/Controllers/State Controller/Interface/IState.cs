using System;


namespace FickleFrameGames.Controllers
{
    public interface IState
    {
        Action OnStateUpdate { get; }
        Action OnStateFixedUpdate { get; }

        string GetState();
        void StateUpdate();
    } 
}