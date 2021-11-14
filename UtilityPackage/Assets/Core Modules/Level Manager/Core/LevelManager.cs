using FickleFrames.Managers.Internal;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace FickleFrames.Managers
{
    /// <summary>
    /// Singleton class which manages all scene loading and scene data manipulations
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        /*.............................................Serialized Fields....................................................*/

        [SerializeField] private string _levelControllerPath;
        [SerializeField] private LevelController[] _levelControllers;

        /*.............................................Public Fields........................................................*/

        public static LevelManager Instance = null;

        /*.............................................Private Fields.......................................................*/

        private Dictionary<string, LevelController> _levelControllersLookup = new Dictionary<string, LevelController>();
        private LevelController _cachedLevelController = null;
        private List<string> _loadedAdditiveLevels = new List<string>();
        private string _loadedSingleLevel = "";

        /*.............................................Private Methods......................................................*/

        /// <summary>
        /// Implement singleton pattern
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }


        /// <summary>
        /// Intializes collections
        /// </summary>
        private void Start()
        {
            prepareLookup();
        }


        /// <summary>
        /// Initializes lookup
        /// </summary>
        private void prepareLookup()
        {
            foreach (LevelController controller in _levelControllers)
                if (controller != null)
                    _levelControllersLookup.Add(controller.LevelName, controller);
        }


        /// <summary>
        /// Returns a level controller of a target level if it exists
        /// </summary>
        /// <param name="levelName">Target level</param>
        private LevelController fetchLevelController(string levelName)
        {
            if (_levelControllersLookup.ContainsKey(levelName))
            {
                if (_levelControllersLookup[levelName] != null)
                    return _levelControllersLookup[levelName];
                // Remove elements which has null value
                else
                    _levelControllersLookup.Remove(levelName);
            }
            return null;
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Loads a target level if it exists
        /// </summary>
        /// <param name="levelName">Target level name</param>
        public void LoadLevel(string levelName)
        {
            _cachedLevelController = fetchLevelController(levelName);

            // Return if controller not found
            if (_cachedLevelController == null)
            {
                Debug.LogWarning($"Level controller : {levelName} not found!");
                return;
            }

            // Check if the level is additive or not
            if (_cachedLevelController.IsAdditive)
            {
                // Check if  it's already loaded
                for (int i = 0; i < SceneManager.sceneCount; ++i)
                    if (levelName == SceneManager.GetSceneAt(i).name)
                    {
                        _loadedAdditiveLevels.TryAdd(levelName);
                        return;
                    }
                _loadedAdditiveLevels.TryAdd(levelName);
            }
            else
            {
                if (_cachedLevelController.IsLoaded)
                    return;
                _loadedSingleLevel = levelName;
            }

            // Load level
            StartCoroutine(_cachedLevelController.LoadLevel());
        }


        /// <summary>
        /// Unloads a target level if it exists
        /// </summary>
        /// <param name="levelName">Target level name</param>
        public void UnloadLevel(string levelName)
        {
            _cachedLevelController = fetchLevelController(levelName);

            // Null check
            if (_cachedLevelController == null)
            {
                Debug.LogWarning($"Invalid Level Name : [LevelName : {levelName}]");
                return;
            }

            // Check if the scene is additive since they are the scenes that can be unloaded
            if (_cachedLevelController.IsAdditive)
            {
                bool found = false;
                // Check if the scene is already loaded
                for (int i = 0; i < SceneManager.sceneCount; ++i)
                    if (levelName == SceneManager.GetSceneAt(i).name)
                    {
                        _loadedAdditiveLevels.TryRemove(levelName);
                        found = true;
                    }

                if (found)
                    _cachedLevelController.UnloadLevel();
            }
        }


        /// <summary>
        /// Returns all active scenes, default scene at index 0 and rest are additive
        /// </summary>
        public string[] GetAllActiveLevels()
        {
            List<string> levels = new List<string>();

            levels.Add(_loadedSingleLevel);
            levels.AddRange(_loadedAdditiveLevels);

            return levels.ToArray();
        }


        /// <summary>
        /// Returns the current single loaded level
        /// </summary>
        public string GetCurrentScene()
        {
            return _loadedSingleLevel;
        }


        /// <summary>
        /// Returns all active additive scenes
        /// </summary>
        public string[] GetAllActiveAdditiveLevels()
        {
            return _loadedAdditiveLevels.ToArray();
        }
    }
}