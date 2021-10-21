using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace FickleFrames
{
    public class LevelManager : Singleton<LevelManager>
    {
        #region Private Fields

        [SerializeField] private string levelControllerPath;
        [SerializeField] private LevelController[] levelControllers;

        private Dictionary<string, LevelController> singleLevels = new Dictionary<string, LevelController>();
        private Dictionary<string, LevelController> additiveLevels = new Dictionary<string, LevelController>();
        private List<string> loadedAdditiveLevels = new List<string>();
        private string currentSingleLevel = "";
        private string previousSingleLevel = "";

        #endregion Private Fields

        #region Private Methods

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
            for (int i = 0; i < levelControllers.Length; ++i)
            {
                if (levelControllers[i].IsAdditive())
                    additiveLevels.TryAdd(levelControllers[i].levelName, levelControllers[i]);
                else
                    singleLevels.TryAdd(levelControllers[i].levelName, levelControllers[i]);
            }
            currentSingleLevel = SceneManager.GetActiveScene().name;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Returns a level controller
        /// </summary>
        /// <param name="levelName">Name of the level</param>
        public LevelController GetLevelController(string levelName)
        {
            if (singleLevels.ContainsKey(levelName))
                return singleLevels[levelName];
            else if (additiveLevels.ContainsKey(levelName))
                return additiveLevels[levelName];
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
                    loadedAdditiveLevels.TryAdd(levelName);
                    return;
                }
                loadedAdditiveLevels.TryAdd(levelName);
            }
            else
            {
                if (controller.IsLoaded())
                    return;
                previousSingleLevel = currentSingleLevel;
                currentSingleLevel = levelName;
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
            if (!singleLevels.ContainsKey(previousSingleLevel) || singleLevels[previousSingleLevel].IsLoaded())
                return;
            // Swap values
            string temp = previousSingleLevel;
            previousSingleLevel = currentSingleLevel;
            currentSingleLevel = temp;
            // Load the previous single level
            StartCoroutine(singleLevels[currentSingleLevel].LoadLevel());
        }


        /// <summary>
        /// Unloads a target level if it exists
        /// </summary>
        /// <param name="levelName">Target level name</param>
        public void UnloadLevel(string levelName)
        {
            // Only check in additive scenes as you cannot unload a single scene
            if (additiveLevels.ContainsKey(levelName))
            {
                // Check if it's unloaded already if yes then try to remove the value
                if (!additiveLevels[levelName].IsLoaded())
                {
                    loadedAdditiveLevels.TryRemove(levelName);
                    return;
                }
                // Remove the level from list and call unloading routine
                loadedAdditiveLevels.TryRemove(levelName);
                StartCoroutine(additiveLevels[levelName].UnloadLevel());
            }
        }


        /// <summary>
        /// Returns all active scenes, default scene at index 0 and rest are additive
        /// </summary>
        public string[] GetAllActiveLevels()
        {
            List<string> levels = new List<string>();

            levels.Add(currentSingleLevel);
            levels.AddRange(loadedAdditiveLevels);

            return levels.ToArray();
        }


        /// <summary>
        /// Returns all active additive scenes
        /// </summary>
        public string[] GetAllActiveAdditiveLevels()
        {
            return loadedAdditiveLevels.ToArray();
        }

        #endregion Public Methods
    }
}