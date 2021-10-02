using System.Collections.Generic;
using UnityEngine;

namespace FickleFrames
{
    /// <summary>
    /// Statc class that manages all the poolers inside the game
    /// </summary>
    public static class PoolManager
    {
        #region Internals

        private static Dictionary<string, Pooler> poolerLookup = new Dictionary<string, Pooler>();

        #endregion Internals


        /// <summary>
        /// Adds pooler to the collection, it is automatically called by all Pooler objects
        /// </summary>
        public static void AddPooler(string tag, Pooler pooler)
        {
            if (poolerLookup.ContainsKey(tag))
            {
                Debug.LogWarning($"Pooler {tag} exists already, skipping add operation");
                return;
            }
            poolerLookup.Add(tag, pooler);
        }


        /// <summary>
        /// Returns pooler from collection
        /// </summary>
        public static Pooler GetPooler(string tag)
        {
            if (!poolerLookup.ContainsKey(tag))
            {
                Debug.LogWarning($"Pooler {tag} do not exist");
                return null;
            }
            return poolerLookup[tag];
        }


        /// <summary>
        /// Deletes pooler 
        /// </summary>
        public static void DeletePooler(string tag)
        {
            if (!poolerLookup.ContainsKey(tag))
            {
                Debug.LogWarning($"Pooler {tag} do not exist");
                return;
            }
            poolerLookup[tag].ReleaseAllPools();
            Object.Destroy(poolerLookup[tag], 1f);
            poolerLookup.Remove(tag);
        }
    }
}