using UnityEngine;
using FickleFrames;

[CreateAssetMenu(fileName = "Walk_State", menuName = "Scriptable Objects/States/Walk_State")]
public class WalkState : State
{
    public override string GetState()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            return "Walk_State";
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            return "Run_State";
        else
            return "Idle_State";
    }
}
