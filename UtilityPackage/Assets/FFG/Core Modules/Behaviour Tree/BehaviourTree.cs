using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace FFG.BehaviourTree
{
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
        public Node RootNode;
        public List<Node> Nodes = new List<Node>();
        public EState TreeState = EState.Running;

        public EState Update()
        {
            if (RootNode.State == EState.Running)
                TreeState = RootNode.Update();
            return TreeState;
        }

        public Node CreateNode(System.Type type)
        {
            Node node = CreateInstance(type) as Node;
            node.name = type.Name;
            node.Guid = GUID.Generate().ToString();
            Nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode(Node node)
        {
            Nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }
    }
}