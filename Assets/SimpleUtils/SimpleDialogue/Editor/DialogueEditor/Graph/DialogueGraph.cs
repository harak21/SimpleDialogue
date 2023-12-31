﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public event Action OnConditionValueChanged;
        public event Action<IDialogueNode> OnFirstNodeChanged;

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

        public void ClearGraph()
        {
            foreach (var edge in edges)
            {
                RemoveElement(edge);
            }

            foreach (var node in _dialogGraphNodeViews)
            {
                RemoveElement(node);
            }
            _dialogGraphNodeViews.Clear();
        }
        
        public void UpdatePhraseNodes(Actor actor)
        {
            foreach (var node in _dialogGraphNodeViews)
            {
                if (node is PhraseNodeView dialoguePhraseNode && dialoguePhraseNode.DialogPhraseNode.ActorID == actor.ID)
                {
                    dialoguePhraseNode.UpdateActorName(actor.ActorName);
                }
            }
        }

        public void AddPhraseNode(string nodeTitle, Vector2 pos, DialoguePhraseNode phraseNode, Actor actor, bool isNew)
        {
            var nodeView = new PhraseNodeView(actor, phraseNode, nodeTitle);

            AddNode(pos, isNew, nodeView);
        }
        
        
        public void AddEventNode(DialogueEventNode eventNode, Vector2 pos, bool isNew)
        {
            var nodeView = new EventNodeView(eventNode);
            
            AddNode(pos, isNew, nodeView);
        }

        public ConditionNodeView AddConditionNode(DialogueConditionNode conditionNode, ConditionValue conditionValue, Vector2 pos, bool isNew, bool isReadOnly)
        {
            var nodeView = new ConditionNodeView(conditionNode, conditionValue, isReadOnly);
            nodeView.OnLocalConditionChanged += changedCondition =>
            {
                foreach (var dialogGraphNodeView in _dialogGraphNodeViews)
                {
                    if (dialogGraphNodeView is ConditionNodeView conditionNodeView 
                        && conditionNodeView.ConditionNode.ConditionID == changedCondition.ID)
                    {
                        conditionNodeView.ChangeDescription(changedCondition);
                        OnConditionValueChanged?.Invoke();
                    }
                }
            };

            AddNode(pos, isNew, nodeView);

            return nodeView;
        }

        private void AddNode(Vector2 pos, bool isNew, DialogueNodeView nodeView)
        {
            if (isNew)
            {
                var localMousePosition = GetLocalMousePosition(pos);
                nodeView.style.left = localMousePosition.x;
                nodeView.style.top = localMousePosition.y;
                nodeView.NodeLayoutPosition = localMousePosition;
                OnNodesMoved?.Invoke(new List<DialogueNodeView>(){nodeView});
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
            var localMousePosition = GetLocalMousePosition(pos);
            var valueTuple = OnNewConditionNodeCreate?.Invoke(localMousePosition);
            if (valueTuple is null)
            {
                return;
            }
            var conditionNode = valueTuple.Value.Item1;
            var conditionValue = valueTuple.Value.Item2;

            var nodeView = AddConditionNode(conditionNode, conditionValue,  localMousePosition, false, false);
            
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
                    && startPort.GetFirstAncestorOfType<DialogueNodeView>() !=
                    port.GetFirstAncestorOfType<DialogueNodeView>()
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
                menuEvent =>
                {
                    menuEvent.menu.MenuItems().Clear();
                    menuEvent.menu.AppendAction("Set as start", SetAsStartNode);
                });
            return manipulator;
        }

        private void SetAsStartNode(DropdownMenuAction action)
        {
            var node = selection.Find(s => s is DialogueNodeView);
            
            if (node is not DialogueNodeView nodeView)
                return;
            
            OnFirstNodeChanged?.Invoke(nodeView.DialogueNode);
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
                        node.NodeLayoutPosition = node.layout.position;
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