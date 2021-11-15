using UnityEngine;


namespace FickleFrames.Systems.Internal
{
    [System.Serializable]
    public struct GameobjectPool
    {
        public GameobjectPool(string poolName, GameObject prefab, int poolSize)
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
}
