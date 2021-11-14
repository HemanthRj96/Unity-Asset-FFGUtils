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
        /*.............................................Serialized Fields....................................................*/

        [Header("Pooler Settings")]
        [SerializeField] private string _poolerName = default;
        [SerializeField] private bool _doNotDestroyOnLoad = false;
        [Tooltip("If true then the pool gets created on Awake")]
        [SerializeField] private bool _shouldWarmPoolOnAwake = false;
        [Header("Pool Settings")]
        [SerializeField] private List<GameobjectPool> _pools = new List<GameobjectPool>();

        /*.............................................Private Fields.......................................................*/

        private List<ObjectPool> _objectPools = new List<ObjectPool>();
        private Dictionary<string, Queue<GameObject>> _mainPoolLookup = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, Queue<UnityEngine.Object>> _objectPoolLookup = new Dictionary<string, Queue<UnityEngine.Object>>();

        /*.............................................Private Methods......................................................*/
   
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
            PoolerManager.DeletePooler(_poolerName);
            ReleaseAllPools();
        }


        /// <summary>
        /// Initializes data properly
        /// </summary>
        private void bootstrapper()
        {
            // Change poolerName if poolerName is empty
            if (string.IsNullOrEmpty(_poolerName))
                _poolerName = gameObject.name;

            // Change pool values to default values
            for (int i = 0; i < _pools.Count; ++i)
                _pools[i] = new GameobjectPool(_pools[i].PoolName, _pools[i].Prefab, _pools[i].PoolSize);

            // Add this pooler to pool manager
            PoolerManager.AddPooler(_poolerName, this);

            if (_doNotDestroyOnLoad)
                DontDestroyOnLoad(this);
            if (_shouldWarmPoolOnAwake)
                WarmPool();
        }


        /// <summary>
        /// Returns GameObject from queue
        /// </summary>
        private GameObject getFromPool(string poolTag)
        {
            return _mainPoolLookup[poolTag].Dequeue();
        }


        /// <summary>
        /// Returns UnityEngine.Object object from pool
        /// </summary>
        private UnityEngine.Object getFromObjectPool(string poolTag)
        {
            return _objectPoolLookup[poolTag].Dequeue();
        }

        /// <summary>
        /// Adds the GameObject back into queue
        /// </summary>
        private void loadIntoPool(string poolTag, GameObject entity)
        {
            _mainPoolLookup[poolTag].Enqueue(entity);
        }

        /// <summary>
        /// Adds UnityEngine.Object object back into queue
        /// </summary>
        private void loadIntoObjectPool(string poolTag, UnityEngine.Object entity)
        {
            _objectPoolLookup[poolTag].Enqueue(entity);
        }

        /// <summary>
        /// Returns true if valid pool
        /// </summary>
        private bool checkIfPoolExists(string poolTag)
        {
            return _mainPoolLookup.ContainsKey(poolTag);
        }

        /// <summary>
        /// Returns true if the pool is valid
        /// </summary>
        private bool checkIfObjectPoolExists(string poolTag)
        {
            return _objectPoolLookup.ContainsKey(poolTag);
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Returns UnityEngine.Object object from pool
        /// </summary>
        public UnityEngine.Object UseObject(string poolTag)
        {
            if (!_objectPoolLookup.ContainsKey(poolTag))
            {
                Debug.LogError("Pool tag not found!!");
                return null;
            }

            UnityEngine.Object obj = _objectPoolLookup[poolTag].Dequeue();
            _objectPoolLookup[poolTag].Enqueue(obj);
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
            foreach (var pool in _pools)
            {
                Queue<GameObject> tempQueue = new Queue<GameObject>();
                // Duplicate guard
                if (checkIfPoolExists(pool.PoolName))
                    continue;
                for (int i = 0; i < pool.PoolSize; ++i)
                {
                    GameObject instance = Instantiate(pool.Prefab);
                    instance.SetActive(false);
                    tempQueue.Enqueue(instance);
                }
                _mainPoolLookup.Add(pool.PoolName, tempQueue);
            }
            // Loadup object pool
            foreach (var pool in _objectPools)
            {
                Queue<UnityEngine.Object> tempQueue = new Queue<UnityEngine.Object>();
                // Duplicate guard
                if (checkIfObjectPoolExists(pool.PoolTag))
                    continue;
                for (int i = 0; i < pool.PoolSize; ++i)
                {
                    UnityEngine.Object instance = Instantiate(pool.Entity);
                    tempQueue.Enqueue(instance);
                }
                _objectPoolLookup.Add(pool.PoolTag, tempQueue);
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
                _pools.Add(new GameobjectPool(poolTag, (GameObject)entity, size));
                if (shouldWarm)
                    WarmPool();
            }
            else
            {
                _objectPools.Add(new ObjectPool(poolTag, entity, size));
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
                GameObject[] entities = _mainPoolLookup[poolTag].ToArray();
                foreach (var entity in entities)
                    Destroy(entity);
                _mainPoolLookup.Remove(poolTag);
            }
            else if (checkIfObjectPoolExists(poolTag))
            {
                UnityEngine.Object[] entities = _objectPoolLookup[poolTag].ToArray();
                foreach (var entity in entities)
                    Destroy(entity);
                _objectPoolLookup.Remove(poolTag);
            }
        }


        /// <summary>
        /// Releases all objects from pool
        /// </summary>
        public void ReleaseAllPools()
        {
            if (_mainPoolLookup.Count != 0)
            {
                // Release main pool
                foreach (string key in _mainPoolLookup.Keys.ToList())
                    ReleasePool(key);
                _mainPoolLookup.Clear();
            }
            if (_objectPoolLookup.Count != 0)
            {
                // Release alternate pool
                foreach (string key in _objectPoolLookup.Keys.ToList())
                    ReleasePool(key);
                _objectPoolLookup.Clear();
            }
        }
    }
}