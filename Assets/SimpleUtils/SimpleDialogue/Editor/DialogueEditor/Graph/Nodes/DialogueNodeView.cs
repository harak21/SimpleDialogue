using System;
using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Graph.Nodes
{
    internal abstract class DialogueNodeView : GraphElement
    {
        public event Action<Edge, Vector2> OnDroppedOutsidePort;
        
        public Port InputPort;
        public Port OutputPort;
        
        public Vector2 NodeLayoutPosition => layout.position;
        
        public List<Edge> Edges
        {
            get
            {
                List<Edge> edges = new List<Edge>();
                edges.AddRange(InputPort.connections);
                edges.AddRange(OutputPort.connections);
                return edges;
            }
        }
        
        public abstract IEnumerable<int> NextNodes { get; }
        public abstract int ID { get; }
        public abstract IDialogueNode DialogueNode { get; }

        protected DialogueNodeView()
        {
            capabilities |= Capabilities.Selectable 
                            | Capabilities.Movable 
                            | Capabilities.Deletable 
                            | Capabilities.Ascendable 
                            | Capabilities.Copiable 
                            | Capabilities.Snappable 
                            | Capabilities.Groupable;
            style.position = Position.Absolute;
            usageHints = UsageHints.DynamicTransform;
        }

        public abstract void RemoveNextNode(int i);
        public abstract void AddNextNode(int nextNodeID);
        
        public override void OnSelected()
        {
            base.OnSelected();
            AddToClassList("node--selected");
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            RemoveFromClassList("node--selected");
        }

        protected void ConstructNode()
        {
            InputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            InputPort.portName = "";
            OutputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            OutputPort.portName = "";
            
            var connectionListener = new ConnectionListener();
            OutputPort.AddManipulator(new EdgeConnector<Edge>(connectionListener));
            connectionListener.OnDroppedOutsidePort += EdgeDroppedOutsidePort;

            var input = this.Q<VisualElement>("input");
            var output = this.Q<VisualElement>("output");
            input.Add(InputPort);
            output.Add(OutputPort);
        }

        private void EdgeDroppedOutsidePort(Edge edge, Vector2 pos)
        {
            OnDroppedOutsidePort?.Invoke(edge, pos);
        }
    }
}