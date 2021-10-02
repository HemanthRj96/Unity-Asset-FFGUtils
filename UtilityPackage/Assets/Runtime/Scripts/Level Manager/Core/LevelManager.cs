using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace FickleFrames
{
    public class LevelManager : Singleton<LevelManager>
    {

        #region Internals

        [Space(5)]
        [Header("-Level Manager Settings-")]
        [Tooltip("Store every level controllers in a single folder as prefabs and provide the path here")]
        [SerializeField]
        private string levelControllerPath = "";

        private List<string> loadedAdditiveLevels = new List<string>();
        private Dictionary<string, LevelController> singleLevelControllers = new Dictionary<string, LevelController>();
        private Dictionary<string, LevelController> additiveLevelControllers = new Dictionary<string, LevelController>();
        private string currentSingleLevel = "";
        private string previousSingleLevel = "";

        private new void Awake()
        {
            base.Awake();
            bootstrapper();
        }


        /// <summary>
        /// Loads all the LevelController prefabs and childs it to this gameObject
        /// </summary>
        private void bootstrapper()
        {
            // Get all level controller prefabs from the filepath
            foreach (string filePath in Directory.GetFiles(levelControllerPath, "*.asset"))
            {
                LevelController levelController = AssetDatabase.LoadAssetAtPath<LevelController>(filePath);
                if (levelController.IsControllerUsable())
                {
                    if (levelController.IsAdditive())
                        additiveLevelControllers.TryAdd(levelController.GetLevelName(), levelController);
                    else
                        singleLevelControllers.TryAdd(levelController.GetLevelName(), levelController);
                }
            }
            currentSingleLevel = SceneManager.GetActiveScene().name;
        }


        #endregion Internals


        /// <summary>
        /// Returns a level controller
        /// </summary>
        /// <param name="levelName">Name of the level</param>
        public LevelController GetLevelController(string levelName)
        {
            // Check inside singleLevelController
            if (singleLevelControllers.ContainsKey(levelName))
                return singleLevelControllers[levelName];
            // Check inside additiveLevelController
            else if (additiveLevelControllers.ContainsKey(levelName))
                return additiveLevelControllers[levelName];
            // Return null otherwise
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
            if (!singleLevelControllers.ContainsKey(previousSingleLevel) || singleLevelControllers[previousSingleLevel].IsLoaded())
                return;
            // Swap values
            string temp = previousSingleLevel;
            previousSingleLevel = currentSingleLevel;
            currentSingleLevel = temp;
            // Load the previous single level
            StartCoroutine(singleLevelControllers[currentSingleLevel].LoadLevel());
        }


        /// <summary>
        /// Unloads a target level if it exists
        /// </summary>
        /// <param name="levelName">Target level name</param>
        public void UnloadLevel(string levelName)
        {
            // Only check in additive scenes as you cannot unload a single scene
            if (additiveLevelControllers.ContainsKey(levelName))
            {
                // Check if it's unloaded already if yes then try to remove the value if the value exists inside
                // loadedAdditvieLevels list
                if (!additiveLevelControllers[levelName].IsLoaded())
                {
                    loadedAdditiveLevels.TryRemove(levelName);
                    return;
                }
                // Remove the level from list and call unloading routine
                loadedAdditiveLevels.TryRemove(levelName);
                StartCoroutine(additiveLevelControllers[levelName].UnloadLevel());
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
    }
}