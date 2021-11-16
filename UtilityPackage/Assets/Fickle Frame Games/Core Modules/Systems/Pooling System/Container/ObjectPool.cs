using UnityEngine;


namespace FickleFrameGames.Systems.Internal
{
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