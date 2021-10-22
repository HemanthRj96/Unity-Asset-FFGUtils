using UnityEditor.Experimental.GraphView;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace FickleFrames.Systems
{
    public class NodeGraphView : GraphView
    {
        private readonly Vector2 defaultPortSize = new Vector2(200, 150);


        public NodeGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddElement(createNode("Entry", true));
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach((port) =>
            {
                if (startPort != port && startPort.node != port.node)
                    compatiblePorts.Add(port);
            });
            return compatiblePorts;
        }

        private Port generatePort(BaseNode node, Direction portDirection, System.Type type, Port.Capacity capacity = Port.Capacity.Multi)
        {
            return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, type);
        }


        public BaseNode createNode(string nodeName, bool entry = false)
        {
            // create and initialize node
            BaseNode node = new BaseNode
            {
                title = nodeName,
                GUID = System.Guid.NewGuid().ToString(),
                entryPoint = entry
            };
            node.SetPosition(new Rect(new Vector2(200, 150), defaultPortSize));

            if (entry == false)
            {
                // create input port for this
                Port inputPort = generatePort(node, Direction.Input, typeof(int));
                inputPort.portName = "Input";
                node.inputContainer.Add(inputPort);
                Button addButton = new Button(() => { addOutput(node); });
                addButton.text = "Add Output";
                node.titleContainer.Add(addButton);
            }
            else
            {
                Port port = generatePort(node, Direction.Output, typeof(int), Port.Capacity.Single);
                port.portName = "Next";
                node.outputContainer.Add(port);
            }


            // refresh node and add to the graph
            node.RefreshExpandedState();
            node.RefreshPorts();
            AddElement(node);
            return node;
        }

        private void addOutput(BaseNode node)
        {
            Port outputPort = generatePort(node, Direction.Output, typeof(int), Port.Capacity.Single);
            int index = node.outputContainer.Query("connector").ToList().Count;
            outputPort.portName = $"Output #{index}";

            node.outputContainer.Add(outputPort);

            node.RefreshExpandedState();
            node.RefreshPorts();
        }
    }
}