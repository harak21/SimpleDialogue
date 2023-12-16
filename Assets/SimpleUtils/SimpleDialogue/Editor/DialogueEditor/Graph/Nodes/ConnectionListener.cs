using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Graph.Nodes
{
    internal class ConnectionListener : IEdgeConnectorListener
    {
        public event Action<Edge, Vector2> OnDroppedOutsidePort; 
        public event Action<Edge, GraphView> OnDropped; 
            
        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            OnDroppedOutsidePort?.Invoke(edge,position);
        }

        public void OnDrop(GraphView graphView, Edge edge)
        {
            OnDropped?.Invoke(edge, graphView);
        }
    }
}