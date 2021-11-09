using UnityEngine;

namespace FickleFrames.Systems.Internal
{
    [System.Serializable]
    public struct Pool
    {
        public Pool(string poolName, GameObject prefab, int poolSize)
        {
            if (poolSize == 0)
                poolSize = 2;
            if (poolName == "")
                poolName = prefab.name + "_" + poolSize.ToString();

            PoolName = poolName;
            Prefab = prefab;
            PoolSize = poolSize;
        }

        public string PoolName;
        public GameObject Prefab;
        public int PoolSize;
    }

    [System.Serializable]
    public struct ObjectPool
    {
        public ObjectPool(string poolTag, Object entity, int poolSize)
        {
            PoolTag = poolTag;
            Entity = entity;
            PoolSize = poolSize;
        }

        public string PoolTag;
        public Object Entity;
        public int PoolSize;
    } 
}
