using System.Collections;
using UnityEngine;
using FickleFrames;

public class Testing_02 : ActionSlave
{
    public override void doAction(IActionParameters parameters = null)
    {
        Debug.Log("Action invoked from testing_02 at : " + Time.time);
    }
}
