namespace FickleFrames.ActionSystem.Internal
{
    public enum EOnActionBegin
    {
        ExecuteOnStart,
        ExecuteOnUpdate,
        ExecuteOnFixedUpdate,
        ExecuteExternally,
    }

    public enum EOnActionEnd
    {
        DoNothing,
        ExecuteAnotherAction,
        DestroySelf,
    } 
}