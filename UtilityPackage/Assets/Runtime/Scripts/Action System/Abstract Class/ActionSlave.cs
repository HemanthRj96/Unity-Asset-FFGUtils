using FickleFrames;
using UnityEngine;

public abstract class ActionSlave : MonoBehaviour, IActionSlave
{
    public ActionComponent component { protected get; set; }

    public abstract void doAction(IActionParameters parameters = null);
}
