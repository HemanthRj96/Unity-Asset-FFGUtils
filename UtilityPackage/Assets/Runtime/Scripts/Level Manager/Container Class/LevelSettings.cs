using UnityEditor;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelSettings
{
    public LevelSettings(SceneAsset parentScene, LoadSceneParameters sceneLoadParameters)
    {
        this.targetLevel = parentScene;
        this.sceneLoadParameters = sceneLoadParameters;
    }

    public SceneAsset targetLevel;
    public LoadSceneParameters sceneLoadParameters;
}
