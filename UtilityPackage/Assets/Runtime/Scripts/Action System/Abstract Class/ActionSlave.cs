using UnityEngine;


namespace FickleFrames.ActionSystem
{
    public abstract class ActionSlave : MonoBehaviour, IActionSlave
    {
        #region Public Properties

        public ActionComponent usingComponent { protected get; set; }

        #endregion Public Properties

        #region Public Methods

        public abstract void doAction(IActionParameters parameters = null);

        #endregion Public Methods
    }
}
