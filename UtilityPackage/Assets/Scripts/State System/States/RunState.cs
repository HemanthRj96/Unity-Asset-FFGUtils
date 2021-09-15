using FickleFrames.StateMachine;

public class RunState : State
{
    public RunState(RunSettings settings = null) : base(settings) { }

    public override I_State getState()
    {
        return this;
    }

    public override void updateState()
    {
        // Do something
    }
}

