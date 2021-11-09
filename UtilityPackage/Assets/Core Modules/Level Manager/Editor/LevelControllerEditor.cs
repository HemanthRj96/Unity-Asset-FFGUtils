using FickleFrames.Managers.Internal;
using UnityEditor;


namespace FickleFrames.Managers.Editor_
{
    [CustomEditor(typeof(LevelController))]
    public class LevelControllerEditor : BaseEditor<LevelController>
    {
        SceneAsset scene;

        private void InspectorUpdate()
        {
            // sceneAsset and LevelName
            PropertyField(GetProperty("serializedScene"), "Controlled Scene", "Scene that will be controlled by this controller");
            scene = (SceneAsset)GetProperty("serializedScene").objectReferenceValue;
            if(scene == null)
            {
                Info("This value cannot be null please initialize!!", MessageType.Error);
                return;
            }
            else
                GetProperty("LevelName").stringValue = scene.name;


            // LoadMode
            PropertyField(GetProperty("LoadMode"), "Scene Load Mode", "How this level should be loaded");


            // PhysicsMode
            PropertyField(GetProperty("PhysicsMode"), "Physics Mode", "Physics mode for this level");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            InspectorUpdate();
            serializedObject.ApplyModifiedProperties();
        }
    }
}