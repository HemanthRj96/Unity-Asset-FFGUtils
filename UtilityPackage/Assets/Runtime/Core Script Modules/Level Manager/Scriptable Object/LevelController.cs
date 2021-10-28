using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace FickleFrames.Managers.Internal
{
    [CreateAssetMenu(fileName = "Level_Controller", menuName = "Scriptable Objects/Level Controller")]
    public class LevelController : ScriptableObject
    {
        #region Editor
#if UNITY_EDITOR
        [SerializeField] public SceneAsset serializedScene;
#endif
        #endregion Editor

        #region Internals

#pragma warning disable 0649, 0414

        //********************************************Serialized Fields**************************************************
        [SerializeField] public string LevelName;
        [SerializeField] public LoadSceneMode LoadMode = LoadSceneMode.Single;
        [SerializeField] public LocalPhysicsMode PhysicsMode = LocalPhysicsMode.None;


        //*********************************************Private Fields****************************************************
        private AsyncOperation _loadOperation;
        private AsyncOperation _unloadOperation;
        private Action<string> _onLevelLoad = delegate { };
        private Action<string> _onLevelUnload = delegate { };
        private Dictionary<string, UnityEngine.Object> _gameAssets = new Dictionary<string, UnityEngine.Object>();

#pragma warning restore 0649, 0414

        #region Private Methods

        //*********************************************Private Methods***************************************************
        /// <summary>
        /// Release game assets on destroy
        /// </summary>
        private void OnDestroy()
        {
            releaseGameAssets();
        }


        /// <summary>
        /// Releases all the game asset made under this level controller
        /// </summary>
        private void releaseGameAssets()
        {
            foreach (var asset in _gameAssets)
                Destroy(asset.Value);
        }

        #endregion Private Methods

        #endregion Internals

        #region Public Methods

        //*********************************************Public Methods****************************************************
        /// <summary>
        /// Returns true if the scene associated with this controller is loaded
        /// </summary>
        public bool IsLoaded()
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
                if (LevelName == SceneManager.GetSceneAt(i).name)
                    return true;
            return false;
        }


        /// <summary>
        /// Returns true if the scene controlled by this controller is additive
        /// </summary>
        public bool IsAdditive()
        {
            return LoadMode == LoadSceneMode.Additive;
        }


        /// <summary>
        /// Used to add a game asset which can be dereferenced later on
        /// </summary>
        /// <param name="assetName">Name of the asset</param>
        public void AddLevelAsset(string assetName, UnityEngine.Object gameAsset)
        {
            if (_gameAssets.ContainsKey(assetName))
                _gameAssets[assetName] = gameAsset;
            else
                _gameAssets.Add(assetName, gameAsset);
        }


        /// <summary>
        /// Returns asset if found otherwise null
        /// </summary>
        /// <param name="assetName">Name of the asset</param>
        public TReturn GetLevelAsset<TReturn>(string assetName) where TReturn : UnityEngine.Object
        {
            if (_gameAssets.ContainsKey(assetName))
            {
                if (_gameAssets[assetName] is TReturn)
                    return _gameAssets[assetName] as TReturn;
                else return null;
            }
            else return null;
        }


        /// <summary>
        /// Loads level controlled by this controller
        /// </summary>
        public IEnumerator LoadLevel()
        {
            LoadSceneParameters parameters = new LoadSceneParameters(LoadMode, PhysicsMode);
            _loadOperation = SceneManager.LoadSceneAsync(LevelName, parameters);
            while (!_loadOperation.isDone)
                yield return null;
            _onLevelLoad(LevelName);
        }


        /// <summary>
        /// Unloads level controlled by this controller
        /// </summary>
        public IEnumerator UnloadLevel()
        {
            _unloadOperation = SceneManager.UnloadSceneAsync(LevelName);
            while (!_unloadOperation.isDone)
                yield return null;
            _onLevelUnload(LevelName);
        }


        /// <summary>
        /// Return the progress for the level loading
        /// </summary>
        public float GetProgress()
        {
            if (_loadOperation != null)
                return _loadOperation.progress;
            if (_unloadOperation != null)
                return _unloadOperation.progress;
            return -1;
        }


        /// <summary>
        /// Call this method to subscribe to level loading and unloading event
        /// </summary>
        public void SubscribeToEvents(Action<string> onLevelLoad = null, Action<string> onLevelUnload = null)
        {
            if (onLevelLoad != null)
                this._onLevelLoad += onLevelLoad;
            if (onLevelUnload != null)
                this._onLevelUnload += onLevelUnload;
        }

        #endregion Public Methods
    }
}
