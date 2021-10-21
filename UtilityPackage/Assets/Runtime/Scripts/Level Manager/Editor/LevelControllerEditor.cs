using UnityEditor;


namespace FickleFrames.Managers.LevelControllerEditor_
{
    [CustomEditor(typeof(LevelController))]
    public class LevelControllerEditor : CustomInspector<LevelController>
    {
        SceneAsset scene;

        private void InspectorUpdate()
        {
            // sceneAsset and levelName
            propertyField(getProperty("serializedScene"), "Controlled Scene", "Scene that will be controlled by this controller");
            scene = (SceneAsset)getProperty("serializedScene").objectReferenceValue;
            if(scene == null)
            {
                info("This value cannot be null please initialize!!", MessageType.Error);
                return;
            }
            else
                getProperty("levelName").stringValue = scene.name;


            // loadMode
            propertyField(getProperty("loadMode"), "Scene Load Mode", "How this level should be loaded");


            // physicsMode
            propertyField(getProperty("physicsMode"), "Physics Mode", "Physics mode for this level");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            InspectorUpdate();
            serializedObject.ApplyModifiedProperties();
        }
    }
}