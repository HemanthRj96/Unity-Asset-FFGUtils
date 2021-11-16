using FickleFrameGames.Managers.Internal;
using UnityEditor;


[CustomEditor(typeof(LevelController))]
public class LevelControllerEditor : BaseEditor<LevelController>
{
    SceneAsset scene;
    SerializedProperty _sceneAsset;
    SerializedProperty _levelName;
    SerializedProperty _loadMode;
    SerializedProperty _physicsMode;

    private void InspectorUpdate()
    {
        _sceneAsset = GetProperty("SerializedScene");
        _levelName = GetProperty("_levelName");
        _loadMode = GetProperty("_loadMode");
        _physicsMode = GetProperty("_physicsMode");

        // sceneAsset and LevelName
        PropertyField(_sceneAsset, "Controlled Scene", "Scene that will be controlled by this controller");
        scene = (SceneAsset)_sceneAsset.objectReferenceValue;

        if (scene == null)
        {
            Info("This value cannot be null please initialize!!", MessageType.Error);
            _levelName.stringValue = null;
            return;
        }
        else
            _levelName.stringValue = scene.name;


        // LoadMode
        PropertyField(_loadMode, "Scene Load Mode", "How this level should be loaded");


        // PhysicsMode
        PropertyField(_physicsMode, "Physics Mode", "Physics mode for this level");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        InspectorUpdate();
        serializedObject.ApplyModifiedProperties();
    }
}
