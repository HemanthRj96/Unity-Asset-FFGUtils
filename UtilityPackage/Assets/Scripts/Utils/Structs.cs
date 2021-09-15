using System.Collections.Generic;
using FickleFrames.ActionSystem;
using UnityEngine;

[System.Serializable]
public struct ActionDataStruct
{
    public E_OnActionBegin onActionBegin;
    public bool shouldAddToRegistry;
    public E_OnActionEnd onActionEnd;
    public string tagID;
    public float delayBeforeCurrentAction;
    public string targetTagID;
    public ActionComponent targetCustomAction;
    public float delayBeforeNextAction;
    [Range(0, 100)]
    public float delayBeforeDestroy;
}
