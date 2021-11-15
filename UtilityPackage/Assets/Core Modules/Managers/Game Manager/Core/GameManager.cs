using FickleFrames.Managers.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


namespace FickleFrames.Managers
{
    /// <summary>
    /// Singleton class which controls all the game state through it's sub game managers
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /*.............................................Serialized Fields....................................................*/

        [SerializeField] private SubGameManagerContainer[] _subManagers;

        /*.............................................Private Fields.......................................................*/

        private static GameManager s_instance = null;
        private string _cachedSubManagerName = default;
        private SubGameManagerBase _cachedSubManager = null;
        private Dictionary<string, SubGameManagerBase> _subManagerCollection = new Dictionary<string, SubGameManagerBase>();

        /*.............................................Private Methods......................................................*/

        /// <summary>
        /// Singleton pattern
        /// </summary>
        private void Awake()
        {
            if (s_instance != null)
                Destroy(gameObject);
            else
            {
                s_instance = this;
                DontDestroyOnLoad(this);
            }
        }


        private void Start() => bootstrap();


        private void Update() => gameManagerUpdate();


        /// <summary>
        /// Populate sub-manager collection for quick access and intialize sub-manager 
        /// </summary>
        private void bootstrap()
        {
            // Intialize the collection
            foreach (var manager in _subManagers)
                if (manager.SubManager != null)
                {
                    var instance = Instantiate(manager.SubManager, transform);
                    instance.name = string.IsNullOrEmpty(manager.SubManagerName) ? manager.SubManager.name : manager.SubManagerName;
                    //DontDestroyOnLoad(instance);
                    _subManagerCollection.TryAdd(instance.name, instance);
                }

            // Initialize the first memeber inside the collection
            _cachedSubManager = _subManagerCollection.FirstOrDefault().Value;
        }


        /// <summary>
        /// Method that runs on every Update
        /// </summary>
        private void gameManagerUpdate()
        {
            // Return if the current sub manager is null
            if (!_cachedSubManager)
            {
                Debug.LogError($"Sub Manager Failed Initialization / Destroyed!! " +
                    $"[SubManagerName = {(string.IsNullOrEmpty(_cachedSubManagerName) ? "-" : _cachedSubManagerName)}]");
                return;
            }

            // Run sub manager and store the return value
            _cachedSubManagerName = _cachedSubManager.Run();

            // The string is empty then don't do anything
            if (string.IsNullOrEmpty(_cachedSubManagerName))
                return;

            // Otherwise update the _subManager
            else if (_subManagerCollection.ContainsKey(_cachedSubManagerName))
                _cachedSubManager = _subManagerCollection[_cachedSubManagerName];

            // This condition shouldn't occur unless there's a messup
            else
            {
                //Handle game manager error
                Debug.LogError($"Invalid Sub Manager!! [SubManagerName = {_cachedSubManagerName}]. Please review the code!!");
            }
        }
    }
}