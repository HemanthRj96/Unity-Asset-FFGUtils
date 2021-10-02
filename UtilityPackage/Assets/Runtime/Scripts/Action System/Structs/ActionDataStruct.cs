using FickleFrames;
using UnityEngine;

[System.Serializable]
public struct ActionDataStruct
{
    public string actionName;
    public ActionSlave slave;
    public EOnActionBegin onActionBegin;
    public bool shouldAddToRegistry;
    public EOnActionEnd onActionEnd;
    public float delayBeforeCurrentAction;
    public string nextCustomActionName;
    public ActionComponent nextCustomAction;
    public float delayBeforeDestroy;
}