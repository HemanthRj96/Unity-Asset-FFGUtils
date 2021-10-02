using FickleFrames;
using UnityEngine;


public class Testing_01 : ActionSlave
{
    public override void doAction(IActionParameters parameters = null)
    {
        Debug.Log("Action invoked from testing_01 at : " + Time.time);
    }
}
