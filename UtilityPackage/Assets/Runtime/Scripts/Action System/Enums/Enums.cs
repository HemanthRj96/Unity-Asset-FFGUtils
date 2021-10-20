namespace FickleFrames.Systems.Internal
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