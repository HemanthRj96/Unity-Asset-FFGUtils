using FickleFrames.StateMachine;

public class WalkState : State
{
    public WalkState(WalkSettings settings) : base(settings) { }

    public override I_State getState()
    {
        return this;
    }

    public override void updateState()
    {
        // Do something
    }
}
