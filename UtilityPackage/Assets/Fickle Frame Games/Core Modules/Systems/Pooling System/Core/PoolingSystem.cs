using FickleFrameGames.Systems.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FickleFrameGames.Systems
{
    /// <summary>
    /// Pooler class can create any type of object pools
    /// </summary>
    public class PoolingSystem : MonoBehaviour
    {
        /*.............................................Serialized Fields....................................................*/

        [Header("Pooler Settings")]
        [SerializeField] private string _poolingSystemName = default;
        [SerializeField] private bool _doNotDestroyOnLoad = false;
        [Tooltip("If true then the pool gets created on Awake")]
        [SerializeField] private bool _shouldWarmPoolOnAwake = false;
        [Header("Pool Settings")]
        [SerializeField] private List<GameobjectPool> _gameObjectPool = new List<GameobjectPool>();

        /*.............................................Private Fields.......................................................*/

        private Dictionary<string, Queue<GameObject>> _mainPoolLookup = new Dictionary<string, Queue<GameObject>>();

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
            PoolSystemManager.DeletePooler(_poolingSystemName);
            ReleaseAllPools();
        }


        /// <summary>
        /// Initializes data properly
        /// </summary>
        private void bootstrapper()
        {
            // Change poolerName if poolerName is empty
            if (string.IsNullOrEmpty(_poolingSystemName))
                _poolingSystemName = gameObject.name;

            // Add this pooler to pool manager
            PoolSystemManager.AddPooler(_poolingSystemName, this);

            // Change pool values to default values
            for (int i = 0; i < _gameObjectPool.Count; ++i)
                _gameObjectPool[i] = new GameobjectPool(_gameObjectPool[i].PoolName, _gameObjectPool[i].Prefab, _gameObjectPool[i].PoolSize);

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
        /// Adds the GameObject back into queue
        /// </summary>
        private void loadIntoPool(string poolTag, GameObject entity)
        {
            _mainPoolLookup[poolTag].Enqueue(entity);
        }


        /// <summary>
        /// Returns true if valid pool
        /// </summary>
        private bool checkIfPoolExists(string poolTag)
        {
            return _mainPoolLookup.ContainsKey(poolTag);
        }

        /*.............................................Public Methods.......................................................*/

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
        /// Call this method to load the gameObject back into the pool
        /// </summary>
        /// <param name="poolTag">Target pool tag</param>
        /// <param name="targetObject">Target object</param>
        public void LoadBackIntoPool(string poolTag, GameObject targetObject)
        {
            targetObject.SetActive(false);
            loadIntoPool(poolTag, targetObject);
        }


        /// <summary>
        /// Warms pool by intantiating all GameObject and objects inside the collection
        /// </summary>
        public void WarmPool()
        {
            // Loadup main pool
            foreach (var pool in _gameObjectPool)
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
        }


        /// <summary>
        /// Adds a new item to the UnityEngine.Object pool
        /// </summary>
        public void AddToPool(string poolTag, GameObject entity, int size, bool shouldWarm)
        {
            if (checkIfPoolExists(poolTag))
                return;

            if (entity != null)
            {
                _gameObjectPool.Add(new GameobjectPool(poolTag, entity, size));
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
        }
    }
}