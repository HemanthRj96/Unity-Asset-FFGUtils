using UnityEngine;

public interface IActionParams
{
    object data { get; set; }
    GameObject source { get; set; }
}

public interface I_State
{
    I_State getState();
    void updateState();
}