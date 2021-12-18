using UnityEngine;

namespace FickleFrameGames.Controllers
{
    public abstract class StateSyncInput : ScriptableObject
    {
        /// <summary>
        /// Returns the corresponding name of the state upon updating the input
        /// </summary>
        public abstract string InputUpdate();
    }
}