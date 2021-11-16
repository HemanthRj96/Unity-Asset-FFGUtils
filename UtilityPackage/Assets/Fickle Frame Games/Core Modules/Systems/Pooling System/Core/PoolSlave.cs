using FickleFrameGames.Systems.Internal;
using UnityEngine;


namespace FickleFrameGames.Systems
{
    public abstract class PoolSlave : MonoBehaviour, IPoolSlave
    {
        /*.............................................Private Methods......................................................*/

        /// <summary>
        /// Call OnUse() on enable
        /// </summary>
        private void OnEnable() 
        { 
            OnUse(); 
        }


        /// <summary>
        /// Call OnRelease() upon deactivation
        /// </summary>
        private void OnDisable() 
        { 
            OnRelease(); 
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// This method is invoked when this gameObject becomes enabled in the scene
        /// </summary>
        public abstract void OnUse();


        /// <summary>
        /// This method is invoked when the gameObject becomes disabled in the scene
        /// </summary>
        public abstract void OnRelease();

    }
}