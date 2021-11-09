using System.Collections.Generic;
using UnityEngine;


namespace FickleFrames.Systems
{
    /// <summary>
    /// Statc class that manages all the poolers inside the game
    /// </summary>
    public static class PoolerManager
    {
        #region Internal

        /*.............................................Private Fields.......................................................*/
        private static Dictionary<string, Pooler> _poolerLookup = new Dictionary<string, Pooler>();

        #endregion Internal

        #region Public Methods

        /*.............................................Public Methods.......................................................*/
        /// <summary>
        /// Adds pooler to the collection, it is automatically called by all Pooler objects
        /// </summary>
        public static void AddPooler(string tag, Pooler pooler)
        {
            if (_poolerLookup.ContainsKey(tag))
            {
                Debug.LogWarning($"Pooler {tag} exists already, skipping add operation");
                return;
            }
            _poolerLookup.Add(tag, pooler);
        }


        /// <summary>
        /// Returns pooler from collection
        /// </summary>
        public static Pooler GetPooler(string tag)
        {
            if (!_poolerLookup.ContainsKey(tag))
            {
                Debug.LogWarning($"Pooler {tag} do not exist");
                return null;
            }
            return _poolerLookup[tag];
        }


        /// <summary>
        /// Deletes pooler 
        /// </summary>
        public static void DeletePooler(string tag)
        {
            if (!_poolerLookup.ContainsKey(tag))
            {
                Debug.LogWarning($"Pooler {tag} do not exist");
                return;
            }
            _poolerLookup[tag].ReleaseAllPools();
            Object.Destroy(_poolerLookup[tag], 1f);
            _poolerLookup.Remove(tag);
        }

        #endregion Public Methods
    }
}