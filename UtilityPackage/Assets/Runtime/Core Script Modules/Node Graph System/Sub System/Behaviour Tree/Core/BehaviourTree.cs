using FickleFrames.Systems.Internal;
using UnityEngine;


namespace FickleFrames.Systems
{
    [CreateAssetMenu(fileName = "BehaviourTree", menuName = "Scriptable Objects/BehaviourTree", order = 0)]
    public class BehaviourTree : ScriptableObject
    {
        public Node RootNode;
        public EStates TreeState;

        public EStates Update()
        {
            if (RootNode.State == EStates.Running)
                TreeState = RootNode.Update();
            return TreeState;
        }
    }
}