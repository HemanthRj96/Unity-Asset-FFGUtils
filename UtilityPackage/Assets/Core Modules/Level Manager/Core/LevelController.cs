using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace FickleFrames.Managers.Internal
{
    [CreateAssetMenu(fileName = "Level_Controller", menuName = "Fickle Frames/Controllers/Create New Level Controller")]
    public class LevelController : ScriptableObject
    {
        #region Editor
#if UNITY_EDITOR
        [SerializeField] public SceneAsset SerializedScene;
#endif
        #endregion Editor

        /*.............................................Serialized Fields....................................................*/

        [SerializeField] private string _levelName = default;
        [SerializeField] private LoadSceneMode _loadMode = LoadSceneMode.Single;
        [SerializeField] private LocalPhysicsMode _physicsMode = LocalPhysicsMode.None;

        /*.............................................Private Fields.......................................................*/

        private AsyncOperation _operation;

        /*.............................................Properties...........................................................*/

        public string LevelName { get { return _levelName; } }
        public bool IsLoaded { get; private set; }
        public bool IsAdditive { get { return _loadMode == LoadSceneMode.Additive; } }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Loads level controlled by this controller
        /// </summary>
        public IEnumerator LoadLevel()
        {
            LoadSceneParameters parameters = new LoadSceneParameters(_loadMode, _physicsMode);
            _operation = SceneManager.LoadSceneAsync(_levelName, parameters);
            yield return new WaitUntil(() => _operation.isDone);
            IsLoaded = true;
        }


        /// <summary>
        /// Unloads level controlled by this controller
        /// </summary>
        public IEnumerator UnloadLevel()
        {
            _operation = SceneManager.UnloadSceneAsync(_levelName);
            yield return new WaitUntil(() => _operation.isDone);
            IsLoaded = false;
        }


        /// <summary>
        /// Return the progress for the level loading
        /// </summary>
        public float GetProgress()
        {
            if (_operation != null)
                return _operation.progress;
            return -1;
        }
    }
}
