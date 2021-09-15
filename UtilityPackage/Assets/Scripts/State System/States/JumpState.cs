using FickleFrames.StateMachine;

public class JumpState : State
{
    public JumpState(JumpSettings settings) : base(settings) { }

    public override I_State getState()
    {
        return this;
    }

    public override void updateState()
    {
        // Do something
    }
}
