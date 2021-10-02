using System.Collections;
using UnityEngine;
using FickleFrames;

[CreateAssetMenu(fileName = "Run_State", menuName = "Scriptable Objects/States/Run_State")]
public class RunState : State
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
