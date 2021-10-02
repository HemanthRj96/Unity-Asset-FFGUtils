using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "Level_Controller", menuName = "Scriptable Objects/Level Controller")]
public class LevelController : ScriptableObject
{

    #region Internals


    [Header("Level Controller Settings")]
    [SerializeField]
    private LevelSettings levelSettings = null;

    private AsyncOperation loadOperation;
    private AsyncOperation unloadOperation;
    private Action<string> onLevelLoad = delegate { };
    private Action<string> onLevelUnload = delegate { };
    private Dictionary<string, UnityEngine.Object> gameAssets = new Dictionary<string, UnityEngine.Object>();


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
        foreach (var asset in gameAssets)
            Destroy(asset.Value);
    }


    #endregion Internals


    /// <summary>
    /// Returns true if the scene associated with this controller is loaded
    /// </summary>
    public bool IsLoaded()
    {
        for (int i = 0; i < SceneManager.sceneCount; ++i)
            if (GetLevelName() == SceneManager.GetSceneAt(i).name)
                return true;
        return false;
    }


    /// <summary>
    /// Returns true if this controller can be used
    /// </summary>
    public bool IsControllerUsable()
    {
        if (levelSettings == null || levelSettings.targetLevel == null)
            return false;
        return true;
    }


    /// <summary>
    /// Returns the level controller name
    /// </summary>
    public string GetLevelName()
    {
        return levelSettings.targetLevel.name; ;
    }


    /// <summary>
    /// Returns true if the scene controlled by this controller is additive
    /// </summary>
    public bool IsAdditive()
    {
        return levelSettings.sceneLoadParameters.loadSceneMode == LoadSceneMode.Additive;
    }


    /// <summary>
    /// Used to add a game asset which can be dereferenced later on
    /// </summary>
    /// <param name="assetName">Name of the asset</param>
    public void AddLevelAsset(string assetName, UnityEngine.Object gameAsset)
    {
        if (gameAssets.ContainsKey(assetName))
            gameAssets[assetName] = gameAsset;
        else
            gameAssets.Add(assetName, gameAsset);
    }


    /// <summary>
    /// Returns asset if found otherwise null
    /// </summary>
    /// <param name="assetName">Name of the asset</param>
    public TReturn GetLevelAsset<TReturn>(string assetName) where TReturn : UnityEngine.Object
    {
        if (gameAssets.ContainsKey(assetName))
        {
            if (gameAssets[assetName] is TReturn)
                return gameAssets[assetName] as TReturn;
            else return null;
        }
        else return null;
    }


    /// <summary>
    /// Loads level controlled by this controller
    /// </summary>
    public IEnumerator LoadLevel()
    {
        string levelName = levelSettings.targetLevel.name;
        loadOperation = SceneManager.LoadSceneAsync(levelName, levelSettings.sceneLoadParameters);
        while (!loadOperation.isDone)
            yield return null;
        onLevelLoad(levelName);
    }


    /// <summary>
    /// Unloads level controlled by this controller
    /// </summary>
    public IEnumerator UnloadLevel()
    {
        string levelName = levelSettings.targetLevel.name;
        unloadOperation = SceneManager.UnloadSceneAsync(levelName);
        while (!unloadOperation.isDone)
            yield return null;
        onLevelUnload(levelName);
    }


    /// <summary>
    /// Return the progress for the level loading
    /// </summary>
    public float GetProgress()
    {
        return loadOperation != null ? loadOperation.progress : (unloadOperation != null ? unloadOperation.progress : -1);
    }


    /// <summary>
    /// Call this method to subscribe to level loading and unloading event
    /// </summary>
    public void SubscribeToEvents(Action<string> onLevelLoad = null, Action<string> onLevelUnload = null)
    {
        if (onLevelLoad != null)
            this.onLevelLoad += onLevelLoad;
        if (onLevelUnload != null)
            this.onLevelUnload += onLevelUnload;
    }
}
