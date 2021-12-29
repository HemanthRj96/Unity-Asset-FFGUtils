using UnityEditor;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace FFG.Managers.Internal
{
    [CreateAssetMenu(fileName = "Sub Level Manager", menuName = "FFG/Create New Sub Level Manager [Type: ScriptableObject, FileType: Asset]")]
    public class SubLevelManager : ScriptableObject
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
        public bool IsLoaded { get { return isLoaded(); } }
        public bool IsAdditive { get { return _loadMode == LoadSceneMode.Additive; } }

        /*.............................................Private Methods......................................................*/

        private bool isLoaded()
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
                if (_levelName == SceneManager.GetSceneAt(i).name)
                    return true;
            return false;
        }


        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Loads level controlled by this controller
        /// </summary>
        public IEnumerator LoadLevel()
        {
            LoadSceneParameters parameters = new LoadSceneParameters(_loadMode, _physicsMode);
            _operation = SceneManager.LoadSceneAsync(_levelName, parameters);
            yield return new WaitUntil(() => _operation.isDone);
        }


        /// <summary>
        /// Unloads level controlled by this controller
        /// </summary>
        public IEnumerator UnloadLevel()
        {
            _operation = SceneManager.UnloadSceneAsync(_levelName);
            yield return new WaitUntil(() => _operation.isDone);
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
