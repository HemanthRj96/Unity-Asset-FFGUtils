﻿using UnityEngine;


namespace FickleFrames.Systems
{
    /// <summary>
    /// This abstract class should be inherited to create prefab slaves or scene object slaves for the 
    /// action component
    /// </summary>
    public abstract class ActionSlave : MonoBehaviour, IActionSlave
    {
        public ActionComponent Component { get; set; }
        public abstract void DoAction(IActionParameters parameters);
    }
}