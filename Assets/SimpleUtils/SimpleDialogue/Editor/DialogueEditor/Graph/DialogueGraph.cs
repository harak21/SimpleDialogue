using System;
using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Graph.Nodes;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Graph
{
    internal class DialogueGraph : GraphView
    {
        public event Action<List<DialogueNodeView>> OnNodesMoved;
        public event Action<List<DialogueNodeView>> OnNodesRemoved;
        public event Func<Vector2, (DialogueConditionNode, ConditionValue)> OnNewConditionNodeCreate; 

        private readonly DialogueEditorWindow _currentWindow;
        private readonly List<DialogueNodeView> _dialogGraphNodeViews = new();

        public DialogueGraph(DialogueEditorWindow currentWindow)
        {
            _currentWindow = currentWindow;

            var styleSheet = AssetProvider.LoadAssetAtAssetName<StyleSheet>("GraphViewStyle");

            AddManipulators();
            OnGraphChanged();
            styleSheets.Add(styleSheet);
        }

        public void AddPhraseNode(string nodeTitle, Vector2 pos, DialoguePhraseNode phraseNode, Actor actor, bool isNew)
        {
            var nodeView = new PhraseNodeView(actor, phraseNode, nodeTitle);

            AddNode(pos, isNew, nodeView);
        }

        public void AddConditionNode(DialogueConditionNode conditionNode, ConditionValue conditionValue, Vector2 pos, bool isNew)
        {
            var nodeView = new ConditionNodeView(conditionNode, conditionValue);
            
            if (isNew)
            {
                var localMousePosition = GetLocalMousePosition(pos);
                nodeView.style.left = localMousePosition.x;
                nodeView.style.top = localMousePosition.y;
                OnNodesMoved?.Invoke(new List<DialogueNodeView>(){nodeView});
            }
            else
            {
                nodeView.style.left = pos.x;
                nodeView.style.top = pos.y;
            }

            AddNode(pos, false, nodeView);
        }

        private void AddNode(Vector2 pos, bool isNew, DialogueNodeView nodeView)
        {
            if (isNew)
            {
                var localMousePosition = GetLocalMousePosition(pos);
                nodeView.style.left = localMousePosition.x;
                nodeView.style.top = localMousePosition.y;
            }
            else
            {
                nodeView.style.left = pos.x;
                nodeView.style.top = pos.y;
            }

            nodeView.OnDroppedOutsidePort += EdgeDroppedOutsidePort;
            
            AddElement(nodeView);
            _dialogGraphNodeViews.Add(nodeView);
        }

        private void EdgeDroppedOutsidePort(Edge edge, Vector2 pos)
        {
            //TODO :the ability to create conditions by dragging a edge is currently disabled
            //we need to add a view update
            return;
            var localMousePosition = GetLocalMousePosition(pos);
            var valueTuple = OnNewConditionNodeCreate?.Invoke(localMousePosition);
            if (valueTuple is null)
            {
                return;
            }
            var conditionNode = valueTuple.Value.Item1;
            var conditionValue = valueTuple.Value.Item2;
            var nodeView = new ConditionNodeView(conditionNode, conditionValue);
            AddNode(localMousePosition, false, nodeView);
            var dialogueNodeView = edge.output.GetFirstAncestorOfType<DialogueNodeView>();
            dialogueNodeView.AddNextNode(conditionNode.ID);
            var newEdge = dialogueNodeView.OutputPort.ConnectTo(nodeView.InputPort);
            AddElement(newEdge);
        }

        public void ConnectNodes()
        {
            List<int> connectionToDelete = new();
            foreach (var node in _dialogGraphNodeViews)
            {
                foreach (var nextNodeID in node.NextNodes)
                {
                    var linkedNode = _dialogGraphNodeViews.Find(n => n.ID == nextNodeID);
                    if (linkedNode is null)
                    {
                        connectionToDelete.Add(nextNodeID);
                        continue;
                    }
                    var edge = node.OutputPort.ConnectTo(linkedNode.InputPort);
                    AddElement(edge);
                }

                foreach (var i in connectionToDelete)
                {
                    node.RemoveNextNode(i);
                }
                connectionToDelete.Clear();
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if (startPort != port
                    && startPort.GetFirstAncestorOfType<PhraseNodeView>() !=
                    port.GetFirstAncestorOfType<PhraseNodeView>()
                    && startPort.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }

        private void AddManipulators()
        {
            SetupZoom(0.02f, 1f);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            this.AddManipulator(ClearContextualMenu());
        }

        private IManipulator ClearContextualMenu()
        {
            ContextualMenuManipulator manipulator = new ContextualMenuManipulator(
                menuEvent => { menuEvent.menu.MenuItems().Clear(); });
            return manipulator;
        }

        private void OnGraphChanged()
        {
            graphViewChanged += changes =>
            {
                if (changes.edgesToCreate is not null)
                {
                    CreateEdge(changes);
                }

                if (changes.movedElements is not null)
                {
                    UpdateElementsPosition(changes);
                }

                if (changes.elementsToRemove is not null)
                {
                    RemoveElements(changes);
                }

                return changes;
            };
        }

        private void CreateEdge(GraphViewChange graphViewChange)
        {
            foreach (var edge in graphViewChange.edgesToCreate)
            {
                var node = edge.output.GetFirstAncestorOfType<DialogueNodeView>();
                var nextNode = edge.input.GetFirstAncestorOfType<DialogueNodeView>();
                node.AddNextNode(nextNode.ID);
                //node?.LinkNode(edge);
            }
        }

        private void UpdateElementsPosition(GraphViewChange graphViewChange)
        {
            List<DialogueNodeView> movedNodes = new();

            foreach (var movedElement in graphViewChange.movedElements)
            {
                switch (movedElement)
                {
                    case DialogueNodeView node:
                        movedNodes.Add(node);
                        //OnNodeMoved?.Invoke(node);
                        break;
                }
            }

            OnNodesMoved?.Invoke(movedNodes);
        }

        private void RemoveElements(GraphViewChange graphViewChange)
        {
            List<DialogueNodeView> removedNodes = new();
            foreach (var element in graphViewChange.elementsToRemove)
            {
                switch (element)
                {
                    case Edge edge:
                    {
                        var node = edge.output.GetFirstAncestorOfType<DialogueNodeView>();
                        var nextNode = edge.input.GetFirstAncestorOfType<DialogueNodeView>();
                        node.RemoveNextNode(nextNode.ID);
                        break;
                    }
                    case DialogueNodeView nodeView:
                    {
                        foreach (var edge in nodeView.InputPort.connections)
                        {
                            var parentNodeView = edge.output.GetFirstAncestorOfType<DialogueNodeView>();
                            parentNodeView.RemoveNextNode(nodeView.ID);
                        }
                        
                        foreach (var edge in nodeView.Edges)
                        {
                            edge.input?.Disconnect(edge);
                            edge.output?.Disconnect(edge);
                            RemoveElement(edge);
                        }

                        removedNodes.Add(nodeView);
                        break;
                    }
                }
            }

            OnNodesRemoved?.Invoke(removedNodes);
        }

        private Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            if (isSearchWindow)
            {
                mousePosition -= _currentWindow.position.position;
            }

            return contentViewContainer.WorldToLocal(mousePosition);
        }
    }
}