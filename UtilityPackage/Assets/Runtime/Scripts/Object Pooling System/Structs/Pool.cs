using UnityEngine;

[System.Serializable]
public struct Pool
{
    public Pool(string poolName, GameObject prefab, int poolSize)
    {
        if (poolSize == 0)
            poolSize = 2;
        if (poolName == "")
            poolName = prefab.name + "_" + poolSize.ToString();

        this.poolName = poolName;
        this.prefab = prefab;
        this.poolSize = poolSize;
    }

    public string poolName;
    public GameObject prefab;
    public int poolSize;
}

[System.Serializable]
public struct ObjectPool
{
    public ObjectPool(string poolTag, Object entity, int poolSize)
    {
        this.poolTag = poolTag;
        this.entity = entity;
        this.poolSize = poolSize;
    }

    public string poolTag;
    public Object entity;
    public int poolSize;
}
