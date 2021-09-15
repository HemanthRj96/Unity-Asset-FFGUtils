using FickleFrames.StateMachine;

public class IdleState : State
{
    public IdleState(IdleSettings settings = null) : base(settings) { }

    public override I_State getState()
    {
        return this;
    }

    public override void updateState()
    {
        // Do something
    }
}
