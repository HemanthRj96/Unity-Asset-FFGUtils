using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FickleFrames.Systems
{
    public class NodeGraph : EditorWindow
    {
        private NodeGraphView nodeGraphView;

        [MenuItem("Graph/Node Graph")]
        public static void OpenWindow()
        {
            var window = GetWindow<NodeGraph>();
            window.titleContent = new GUIContent("Node Graph");
        }

        private void OnEnable()
        {
            generateGraph();
            generateToolbar();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(nodeGraphView);
        }


        private void generateGraph()
        {
            nodeGraphView = new NodeGraphView
            {
                name = "Node Graph"
            };

            nodeGraphView.StretchToParentSize();
            rootVisualElement.Add(nodeGraphView);
        }

        private void generateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            Button createNodeButton = new Button(() =>
            {
                nodeGraphView.createNode("Node");
            });
            createNodeButton.text = "Create New Node";

            toolbar.Add(createNodeButton);
            rootVisualElement.Add(toolbar);
        }

    }
}