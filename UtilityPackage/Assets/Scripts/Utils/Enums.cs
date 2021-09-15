public enum E_OnActionBegin
{
    ExecuteOnStart,
    ExecuteOnUpdate,
    ExecuteOnFixedUpdate,
    ExecuteExternally,
    DefaultNull
}

public enum E_OnActionEnd
{
    DoNothing,
    ExecuteAnotherAction,
    RemoveFromRegistry,
    DeactivateSelf,
    DestroySelf,
    DefaultNull
}

public enum E_SoundPlayMode
{
    Play,
    PlayOneShot,
    PlayDelayed,
}

public enum E_StateUpdateMode
{
    Update,
    FixedUpdate
}

