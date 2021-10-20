using UnityEngine;


namespace FickleFrames.ActionSystem
{
    /// <summary>
    /// This abstract class should be inherited to create prefab slaves or scene object slaves for the 
    /// action component
    /// </summary>
    public abstract class ActionSlave : MonoBehaviour, IActionSlave
    {
        public ActionComponent component { get; set; }
        public abstract void doAction(IActionParameters parameters);
    }
}