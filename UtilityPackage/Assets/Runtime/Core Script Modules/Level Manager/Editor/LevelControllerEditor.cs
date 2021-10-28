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
            propertyField(getProperty("serializedScene"), "Controlled Scene", "Scene that will be controlled by this controller");
            scene = (SceneAsset)getProperty("serializedScene").objectReferenceValue;
            if(scene == null)
            {
                info("This value cannot be null please initialize!!", MessageType.Error);
                return;
            }
            else
                getProperty("LevelName").stringValue = scene.name;


            // LoadMode
            propertyField(getProperty("LoadMode"), "Scene Load Mode", "How this level should be loaded");


            // PhysicsMode
            propertyField(getProperty("PhysicsMode"), "Physics Mode", "Physics mode for this level");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            InspectorUpdate();
            serializedObject.ApplyModifiedProperties();
        }
    }
}