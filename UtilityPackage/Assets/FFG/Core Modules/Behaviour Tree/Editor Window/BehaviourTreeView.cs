using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
using System.Collections.Generic;


namespace FFG.BehaviourTree.Internal
{
    public class BehaviourTreeView : GraphView
    {
        private BehaviourTree _tree;

        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

        public BehaviourTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/FFG/Core Modules/Behaviour Tree/Editor Window/BehaviourTreeEditor.uss");
            styleSheets.Add(styleSheet);
        }

        public void PopulateView(BehaviourTree tree)
        {
            _tree = tree;

            graphViewChanged -= onGraphViewChanged;
            DeleteElements(graphElements.ToList());
            graphViewChanged += onGraphViewChanged;

            tree.Nodes.ForEach(n => createNodeView(n));
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node
            ).ToList();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var actionNodeTypes = TypeCache.GetTypesDerivedFrom<ActionNode>();
            var decoratorNodeTypes = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            var compositeNodeTypes = TypeCache.GetTypesDerivedFrom<CompositeNode>();

            foreach (var actionNodeType in actionNodeTypes)
                evt.menu.AppendAction
                    (
                        $"Action Node : {actionNodeType.Name}",
                        (a) => { createNode(actionNodeType); }
                    );

            foreach (var decoratorNodeType in decoratorNodeTypes)
                evt.menu.AppendAction
                    (
                        $"Decorator Node : {decoratorNodeType.Name}",
                        (a) => { createNode(decoratorNodeType); }
                    );

            foreach (var compositeNodeType in compositeNodeTypes)
                evt.menu.AppendAction
                    (
                        $"Composite Node : {compositeNodeType.Name}",
                        (a) => { createNode(compositeNodeType); }
                    );
        }

        private GraphViewChange onGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    NodeView nodeView = elem as NodeView;
                    if (nodeView != null)
                        _tree.DeleteNode(nodeView.Node);
                });
            }

            return graphViewChange;
        }

        private void createNode(System.Type type)
        {
            Node node = _tree.CreateNode(type);
            createNodeView(node);
        }

        private void createNodeView(Node node)
        {
            NodeView nodeView = new NodeView(node);
            AddElement(nodeView);
        }
    }
}