using FickleFrames.Systems.Internal;
using UnityEngine;


namespace FickleFrames.Systems
{
    public class PoolSlave : MonoBehaviour, IPoolSlave
    {

        #region Private Methods

        private void OnEnable() { OnUse(); }

        private void OnDisable() { OnRelease(); }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// This method is invoked when this gameObject becomes enabled in the scene
        /// </summary>
        public virtual void OnUse() { }


        /// <summary>
        /// This method is invoked when the gameObject becomes disabled in the scene
        /// </summary>
        public virtual void OnRelease() { }

        #endregion Public Methods
    }
}