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
        #region Internal

        /*.............................................Serialized Fields....................................................*/
#pragma warning disable 0649
        [SerializeField] private SubGameManagerContainer[] _subManagers;

        /*.............................................Private Fields.......................................................*/
        private static GameManager s_instance = null;
        private bool _isValid = false;
        private string _newSubManagerName;
        private SubGameManagerBase _subManager;
        private Dictionary<string, SubGameManagerBase> _subManagerCollection = new Dictionary<string, SubGameManagerBase>();
#pragma warning restore 0649


        #region Private Methods

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
                _isValid = true;
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
            _subManager = _subManagerCollection.FirstOrDefault().Value;
        }

        /// <summary>
        /// Method that runs on every Update
        /// </summary>
        private void gameManagerUpdate()
        {
            // Return if the current sub manager is null
            if (!_subManager)
            {
                Debug.LogError($"Sub Manager Failed Initialization / Destroyed!! " +
                    $"[SubManagerName = {(string.IsNullOrEmpty(_newSubManagerName) ? "-" : _newSubManagerName)}]");
                return;
            }

            // Run sub manager and store the return value
            _newSubManagerName = _subManager.Run();

            // The string is empty then don't do anything
            if (string.IsNullOrEmpty(_newSubManagerName))
                return;

            // Otherwise update the _subManager
            else if (_subManagerCollection.ContainsKey(_newSubManagerName))
                _subManager = _subManagerCollection[_newSubManagerName];

            // This condition shouldn't occur unless there's a messup
            else
            {
                //Handle game manager error
                Debug.LogError($"Invalid Sub Manager!! [SubManagerName = {_newSubManagerName}]. Please review the code!!");
            }
        }

        #endregion Private Methods

        #endregion Internal
    }
}