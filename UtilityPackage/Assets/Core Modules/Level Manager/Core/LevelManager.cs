using FickleFrames.Managers.Internal;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace FickleFrames.Managers
{
    /// <summary>
    /// Singleton class which manages all scene loading and scene data manipulations
    /// </summary>
    public class LevelManager : Singleton<LevelManager>
    {
        #region Internals

#pragma warning disable 0649, 0414

        /*.............................................Serialized Fields....................................................*/
        [SerializeField] private string _levelControllerPath;
        [SerializeField] private LevelController[] _levelControllers;


        /*.............................................Private Fields.......................................................*/
        private Dictionary<string, LevelController> _singleLevels = new Dictionary<string, LevelController>();
        private Dictionary<string, LevelController> _additiveLevels = new Dictionary<string, LevelController>();
        private List<string> _loadedAdditiveLevels = new List<string>();
        private string _currentSingleLevel = "";
        private string _previousSingleLevel = "";

#pragma warning restore 0649, 0414

        #region Private Methods

        /*.............................................Private Methods......................................................*/
        private new void Awake()
        {
            base.Awake();
            bootstrapper();
        }

        /// <summary>
        /// Creates collections for quick actions
        /// </summary>
        private void bootstrapper()
        {
            for (int i = 0; i < _levelControllers.Length; ++i)
            {
                if (_levelControllers[i].IsAdditive())
                    _additiveLevels.TryAdd(_levelControllers[i].LevelName, _levelControllers[i]);
                else
                    _singleLevels.TryAdd(_levelControllers[i].LevelName, _levelControllers[i]);
            }
            _currentSingleLevel = SceneManager.GetActiveScene().name;
        }

        #endregion Private Methods

        #endregion Internals

        #region Public Methods

        /*.............................................Public Methods.......................................................*/
        /// <summary>
        /// Returns a level controller
        /// </summary>
        /// <param name="levelName">Name of the level</param>
        public LevelController GetLevelController(string levelName)
        {
            if (_singleLevels.ContainsKey(levelName))
                return _singleLevels[levelName];
            else if (_additiveLevels.ContainsKey(levelName))
                return _additiveLevels[levelName];
            return null;
        }


        /// <summary>
        /// Loads a target level if it exists
        /// </summary>
        /// <param name="levelName">Target level name</param>
        public void LoadLevel(string levelName)
        {
            LevelController controller = GetLevelController(levelName);

            // Return if controller not found
            if (controller == null)
            {
                Debug.LogWarning($"Level controller : {levelName} not found!");
                return;
            }

            // Check if the level is additive or not
            if (controller.IsAdditive())
            {
                // Add the level to list
                if (controller.IsLoaded())
                {
                    _loadedAdditiveLevels.TryAdd(levelName);
                    return;
                }
                _loadedAdditiveLevels.TryAdd(levelName);
            }
            else
            {
                if (controller.IsLoaded())
                    return;
                _previousSingleLevel = _currentSingleLevel;
                _currentSingleLevel = levelName;
            }

            // Load level
            StartCoroutine(controller.LoadLevel());
        }


        /// <summary>
        /// Method loads previous level if there's any
        /// </summary>
        public void LoadPreviousLevel()
        {
            // Check if there's valid controllers or check if the controller has the level loaded already
            if (!_singleLevels.ContainsKey(_previousSingleLevel) || _singleLevels[_previousSingleLevel].IsLoaded())
                return;
            // Swap values
            string temp = _previousSingleLevel;
            _previousSingleLevel = _currentSingleLevel;
            _currentSingleLevel = temp;
            // Load the previous single level
            StartCoroutine(_singleLevels[_currentSingleLevel].LoadLevel());
        }


        /// <summary>
        /// Unloads a target level if it exists
        /// </summary>
        /// <param name="levelName">Target level name</param>
        public void UnloadLevel(string levelName)
        {
            // Only check in additive scenes as you cannot unload a single scene
            if (_additiveLevels.ContainsKey(levelName))
            {
                // Check if it's unloaded already if yes then try to remove the value
                if (!_additiveLevels[levelName].IsLoaded())
                {
                    _loadedAdditiveLevels.TryRemove(levelName);
                    return;
                }
                // Remove the level from list and call unloading routine
                _loadedAdditiveLevels.TryRemove(levelName);
                StartCoroutine(_additiveLevels[levelName].UnloadLevel());
            }
        }


        /// <summary>
        /// Returns all active scenes, default scene at index 0 and rest are additive
        /// </summary>
        public string[] GetAllActiveLevels()
        {
            List<string> levels = new List<string>();

            levels.Add(_currentSingleLevel);
            levels.AddRange(_loadedAdditiveLevels);

            return levels.ToArray();
        }


        /// <summary>
        /// Returns all active additive scenes
        /// </summary>
        public string[] GetAllActiveAdditiveLevels()
        {
            return _loadedAdditiveLevels.ToArray();
        }

        #endregion Public Methods
    }
}   