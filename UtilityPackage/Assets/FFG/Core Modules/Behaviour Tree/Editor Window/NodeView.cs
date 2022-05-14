using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;

namespace FFG.BehaviourTree.Internal
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Node Node;
        public Port InputPort;
        public Port OutputPort;

        public NodeView(Node node)
        {
            Node = node;
            title = node.name;
            viewDataKey = node.Guid;

            style.left = node.Position.x;
            style.top = node.Position.y;

            createInputPort();
            createOutputPort();
        }
       
        private void createInputPort()
        {
            InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));

            if(InputPort != null)
            {
                InputPort.portName = "";
                inputContainer.Add(InputPort);
            }
        }

        private void createOutputPort()
        {
            if (Node is CompositeNode)
                OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            else if (Node is DecoratorNode)
                OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

            if(outputContainer != null)
            {
                OutputPort.portName = "";
                outputContainer.Add(OutputPort);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Node.Position.x = newPos.xMin;
            Node.Position.y = newPos.yMin;
        }
    }
}