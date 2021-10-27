using FickleFrames.Systems.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FickleFrames.Systems
{
    /// <summary>
    /// Pooler class can create any type of object pools
    /// </summary>
    public class Pooler : MonoBehaviour
    {

        #region Private Fields

        [Header("-Pooler Settings-")]

        [SerializeField] private string poolerName = default;
        [SerializeField] private bool doNotDestroyOnLoad = false;
        [Tooltip("If true then the pool gets created on Awake")]
        [SerializeField] private bool shouldWarmPoolOnAwake = false;

        [Header("-Pool Settings-")]

        [SerializeField] private List<Pool> pools = new List<Pool>();

        private List<ObjectPool> objectPools = new List<ObjectPool>();
        private Dictionary<string, Queue<GameObject>> mainPoolLookup = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, Queue<UnityEngine.Object>> objectPoolLookup = new Dictionary<string, Queue<UnityEngine.Object>>();

        #endregion Private Fields

        #region Private Methods

        /// <summary>
        /// Calls bootstrapper
        /// </summary>
        private void Awake()
        {
            bootstrapper();
        }


        /// <summary>
        /// Removes this instance of pooler from PoolManager
        /// </summary>
        private void OnDestroy()
        {
            PoolManager.DeletePooler(poolerName);
            ReleaseAllPools();
        }


        /// <summary>
        /// Initializes data properly
        /// </summary>
        private void bootstrapper()
        {
            // Change poolerName if poolerName is empty
            if (poolerName == "")
                poolerName = gameObject.name;

            // Change pool values to default values
            for (int i = 0; i < pools.Count; ++i)
                pools[i] = new Pool(pools[i].poolName, pools[i].prefab, pools[i].poolSize);

            // Add this pooler to pool manager
            PoolManager.AddPooler(poolerName, this);

            if (doNotDestroyOnLoad)
                DontDestroyOnLoad(this);
            if (shouldWarmPoolOnAwake)
                WarmPool();
        }


        /// <summary>
        /// Returns GameObject from queue
        /// </summary>
        private GameObject getFromPool(string poolTag)
        {
            return mainPoolLookup[poolTag].Dequeue();
        }


        /// <summary>
        /// Returns UnityEngine.Object object from pool
        /// </summary>
        private UnityEngine.Object getFromObjectPool(string poolTag)
        {
            return objectPoolLookup[poolTag].Dequeue();
        }

        /// <summary>
        /// Adds the GameObject back into queue
        /// </summary>
        private void loadIntoPool(string poolTag, GameObject entity)
        {
            mainPoolLookup[poolTag].Enqueue(entity);
        }

        /// <summary>
        /// Adds UnityEngine.Object object back into queue
        /// </summary>
        private void loadIntoObjectPool(string poolTag, UnityEngine.Object entity)
        {
            objectPoolLookup[poolTag].Enqueue(entity);
        }

        /// <summary>
        /// Returns true if valid pool
        /// </summary>
        private bool checkIfPoolExists(string poolTag)
        {
            return mainPoolLookup.ContainsKey(poolTag);
        }

        /// <summary>
        /// Returns true if the pool is valid
        /// </summary>
        private bool checkIfObjectPoolExists(string poolTag)
        {
            return objectPoolLookup.ContainsKey(poolTag);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Returns UnityEngine.Object object from pool
        /// </summary>
        public UnityEngine.Object UseObject(string poolTag)
        {
            if (!objectPoolLookup.ContainsKey(poolTag))
            {
                Debug.LogError("Pool tag not found!!");
                return null;
            }

            UnityEngine.Object obj = objectPoolLookup[poolTag].Dequeue();
            objectPoolLookup[poolTag].Enqueue(obj);
            return obj;
        }


        /// <summary>
        /// Spawns GameObject at a specific location
        /// </summary>
        public GameObject SpawnGameObject(string poolTag, Vector3 worldPosition)
        {
            return SpawnGameObject(poolTag, worldPosition, Quaternion.identity);
        }


        /// <summary>
        /// Spawns GameObject at a specific location with specific rotation
        /// </summary>
        public GameObject SpawnGameObject(string poolTag, Vector3 worldPosition, Quaternion rotation)
        {
            // Get from pool
            GameObject poolObject = getFromPool(poolTag);
            // Apply location and rotation values
            poolObject.transform.position = worldPosition;
            poolObject.transform.rotation = rotation;
            // Load the object back into the pool
            loadIntoPool(poolTag, poolObject);
            // Activate the object
            poolObject.SetActive(true);

            return poolObject;
        }


        /// <summary>
        /// Warms pool by intantiating all GameObject and objects inside the collection
        /// </summary>
        public void WarmPool()
        {
            // Loadup main pool
            foreach (var pool in pools)
            {
                Queue<GameObject> tempQueue = new Queue<GameObject>();
                // Duplicate guard
                if (checkIfPoolExists(pool.poolName))
                    continue;
                for (int i = 0; i < pool.poolSize; ++i)
                {
                    GameObject instance = Instantiate(pool.prefab);
                    instance.SetActive(false);
                    tempQueue.Enqueue(instance);
                }
                mainPoolLookup.Add(pool.poolName, tempQueue);
            }
            // Loadup object pool
            foreach (var pool in objectPools)
            {
                Queue<UnityEngine.Object> tempQueue = new Queue<UnityEngine.Object>();
                // Duplicate guard
                if (checkIfObjectPoolExists(pool.poolTag))
                    continue;
                for (int i = 0; i < pool.poolSize; ++i)
                {
                    UnityEngine.Object instance = Instantiate(pool.entity);
                    tempQueue.Enqueue(instance);
                }
                objectPoolLookup.Add(pool.poolTag, tempQueue);
            }
        }


        /// <summary>
        /// Adds a new item to the UnityEngine.Object pool
        /// </summary>
        public void AddToPool(string poolTag, UnityEngine.Object entity, int size, bool shouldWarm)
        {
            if (checkIfPoolExists(poolTag) || checkIfObjectPoolExists(poolTag))
                return;
            if (entity is GameObject)
            {
                pools.Add(new Pool(poolTag, (GameObject)entity, size));
                if (shouldWarm)
                    WarmPool();
            }
            else
            {
                objectPools.Add(new ObjectPool(poolTag, entity, size));
                if (shouldWarm)
                    WarmPool();
            }
        }


        /// <summary>
        /// Releases objects from the pool
        /// </summary>
        public void ReleasePool(string poolTag)
        {
            if (checkIfPoolExists(poolTag))
            {
                GameObject[] entities = mainPoolLookup[poolTag].ToArray();
                foreach (var entity in entities)
                    Destroy(entity);
                mainPoolLookup.Remove(poolTag);
            }
            else if (checkIfObjectPoolExists(poolTag))
            {
                UnityEngine.Object[] entities = objectPoolLookup[poolTag].ToArray();
                foreach (var entity in entities)
                    Destroy(entity);
                objectPoolLookup.Remove(poolTag);
            }
        }


        /// <summary>
        /// Releases all objects from pool
        /// </summary>
        public void ReleaseAllPools()
        {
            if (mainPoolLookup.Count != 0)
            {
                // Release main pool
                foreach (string key in mainPoolLookup.Keys.ToList())
                    ReleasePool(key);
                mainPoolLookup.Clear();
            }
            if (objectPoolLookup.Count != 0)
            {
                // Release alternate pool
                foreach (string key in objectPoolLookup.Keys.ToList())
                    ReleasePool(key);
                objectPoolLookup.Clear();
            }
        }

        #endregion Public Methods
    }
}