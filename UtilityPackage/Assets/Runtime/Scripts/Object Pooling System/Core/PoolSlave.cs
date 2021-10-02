using UnityEngine;

namespace FickleFrames
{
    public class PoolSlave : MonoBehaviour, IPoolSlave
    {
        #region Internals

        private void OnEnable() { OnUse(); }

        private void OnDisable() { OnRelease(); }

        #endregion Internals


        /// <summary>
        /// This method is invoked when this gameObject becomes enabled in the scene
        /// </summary>
        public virtual void OnUse() { }


        /// <summary>
        /// This method is invoked when the gameObject becomes disabled in the scene
        /// </summary>
        public virtual void OnRelease() { }
    }
}