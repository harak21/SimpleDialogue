using System;
using System.Collections.Generic;
using System.Linq;
using SimpleUtils.SimpleDialogue.Editor.Conditions;
using SimpleUtils.SimpleDialogue.Editor.DialogueEditor.ActorsSettings;
using SimpleUtils.SimpleDialogue.Editor.DialogueEditor.ConditionsTab;
using SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Graph;
using SimpleUtils.SimpleDialogue.Editor.DialogueEditor.PhrasesTab;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor
{
    internal class DialogueEditorHandler
    {
        private readonly DialogueEditorWindow _currentWindow;
        private readonly TemplateContainer _rootView;
        private readonly DialogueContainer _dialogueContainer;
        private readonly GlobalValues _globalConditionValues;

        private DialogueGraph _graph;
        private PhrasesListHandler _npcListHandler;
        private PhrasesListHandler _playerListHandler;

        private readonly List<ITabView> _tabViews = new();
        private ITabView _currentTabView;

        private bool _isActorMenuShowed;
        private ActorsSettingsTab _actorsSettingsTab;
        private ConditionValuesTabView _localConditionValuesView;

        public DialogueEditorHandler(DialogueEditorWindow currentWindow, TemplateContainer rootView,
            DialogueContainer dialogueContainer)
        {
            _currentWindow = currentWindow;
            _rootView = rootView;
            _dialogueContainer = dialogueContainer;
            _globalConditionValues = AssetProvider.FindSingleAsset<GlobalValues>();

            CreateGlobalConditionsTab();
            CreateLocalConditionsTab();
            CreateActorsSettingsTab();
            CreateEventButton();

            if (_tabViews.Count > 0)
            {
                _currentTabView = _tabViews.Last();
                NextTabView();
            }

            CreateGraph();
            LoadData();
            AddShortcuts();
            
            Undo.undoRedoPerformed += UndoRedoPerformed;
        }

        private void UndoRedoPerformed()
        {
            for (var i = 0; i < _tabViews.Count; i++)
            {
                var tabView = _tabViews[i];
                if (tabView is ActorPhrasesTabView actorPhrasesTabView)
                {
                    if (actorPhrasesTabView.IsShowed)
                    {
                        NextTabView();
                    }
                    actorPhrasesTabView.Dispose();
                    _tabViews.Remove(tabView);
                    i--;
                }
                else
                {
                    tabView.Update();
                }
            }

            _actorsSettingsTab.LoadActorsData();
            _actorsSettingsTab.Update();
            _graph.ClearGraph();
            LoadData();
        }

        private void AddShortcuts()
        {
            var eventDictionary = new Dictionary<Event, ShortcutDelegate>
            {
                { Event.KeyboardEvent("tab"), NextTabView }
            };
            var shortcutHandler = new ShortcutHandler(eventDictionary);
            _rootView.AddManipulator(shortcutHandler);
        }

        private EventPropagation NextTabView()
        {
            var indexOf = _tabViews.IndexOf(_currentTabView) + 1;
            if (indexOf >= _tabViews.Count)
            {
                indexOf = 0;
            }

            foreach (var tabView in _tabViews)
            {
                tabView.Hide();
            }

            _tabViews[indexOf].Show();
            return EventPropagation.Stop;
        }

        private void CreateGlobalConditionsTab()
        {
            var conditionView = new ConditionValuesTabView(_rootView,
                "Global Conditions",
                _globalConditionValues,
                true);
            conditionView.OnViewSelected += ViewSelected;
            conditionView.OnNodeCreate += CreateConditionNode;
            _tabViews.Add(conditionView);
        }

        private void CreateLocalConditionsTab()
        {
            _localConditionValuesView = new ConditionValuesTabView(_rootView,
                $"{_currentWindow.name} Local Conditions",
                _dialogueContainer,
                false);
            _localConditionValuesView.OnViewSelected += ViewSelected;
            _localConditionValuesView.OnNodeCreate += CreateConditionNode;
            _tabViews.Add(_localConditionValuesView);
        }

        private void CreateActorsSettingsTab()
        {
            _actorsSettingsTab = new ActorsSettingsTab(_rootView, _dialogueContainer, _currentWindow.name);
            _actorsSettingsTab.OnNewActorViewCreate += CreateActorView;
            _actorsSettingsTab.OnViewSelected += ViewSelected;
            _actorsSettingsTab.OnTabViewsChanged += UpdateTabs;
            _actorsSettingsTab.OnActorNameChanged += (actor) =>
            {
                _graph.UpdatePhraseNodes(actor);
            };
            _actorsSettingsTab.LoadActorsData();
        }

        private void CreateActorView(Actor actor, ActorData actorData)
        {
            var actorView = new ActorPhrasesTabView(_rootView, actor, actorData, _currentWindow.name);
            actorView.OnNodeCreate += CreatePhraseNode;
            actorView.OnViewSelected += ViewSelected;
            _tabViews.Add(actorView);
        }
        
        
        private void CreateEventButton()
        {
            var draggableItem = new DraggableItem(_rootView.Q<Button>("addEvent"));
            draggableItem.OnItemDropped += CreateEventNode;
        }

        private void ViewSelected(ITabView selectedView)
        {
            _currentTabView = selectedView;
            foreach (var view in _tabViews)
            {
                view.Hide();
            }
        }

        private void UpdateTabs()
        {
            foreach (var tabView in _tabViews)
            {
                tabView.Update();
            }
        }

        private void CreatePhraseNode(SharedTableData.SharedTableEntry entry, Vector2 pos, string tableKey, Actor actor)
        {
            var id = Guid.NewGuid().GetHashCode();
            var node = new DialoguePhraseNode(id, entry.Id, tableKey, actor);
            _dialogueContainer.AddNode(node);
            _dialogueContainer.NodeData.Add(new DialogueNodeData() { nodeID = id, position = pos });
            _graph.AddPhraseNode(entry.Key, pos, node, actor, true);

            Save();
        }

        private void CreateConditionNode(ConditionValue conditionValue, Vector2 pos, bool isReadOnly)
        {
            var id = Guid.NewGuid().GetHashCode();
            var node = new DialogueConditionNode(id, conditionValue);
            _dialogueContainer.AddNode(node);
            _dialogueContainer.NodeData.Add(new DialogueNodeData() { nodeID = id, position = pos });
            _graph.AddConditionNode(node, conditionValue, pos, true, isReadOnly);
            
            Save();
        }
        
        private void CreateEventNode(Vector2 pos)
        {
            var id = Guid.NewGuid().GetHashCode();
            var eventNode = new DialogueEventNode(id);
            _dialogueContainer.AddNode(eventNode);
            _dialogueContainer.NodeData.Add(new DialogueNodeData() {nodeID = id, position = pos});
            _graph.AddEventNode(eventNode, pos, true);
            
            Save();
        }

        private void LoadData()
        {
            foreach (var node in _dialogueContainer.PhrasesList)
            {
                var stringTableCollection = LocalizationEditorSettings.GetStringTableCollection(node.TableName);
                var sharedTableEntry = stringTableCollection.SharedData.GetEntry(node.EntryKey);
                if (sharedTableEntry is null)
                {
                    Debug.LogWarning($"no row found in localization tables for node [{node.ID}]");
                    continue;
                }

                var position = _dialogueContainer.NodeData.Find(
                    d => d.nodeID == node.ID).position;

                var actor = _dialogueContainer.Actors[node.ActorID];
                _graph.AddPhraseNode(sharedTableEntry.Key, position, node, actor, false);
            }

            foreach (var conditionNode in _dialogueContainer.ConditionsList)
            {
                var position = _dialogueContainer.NodeData.Find(
                    d => d.nodeID == conditionNode.ID).position;

                if (_dialogueContainer.ConditionValues.TryGetValue(conditionNode.ConditionID, out var conditionValue))
                {
                    _graph.AddConditionNode(conditionNode, conditionValue, position, false, false);
                    continue;
                }
                if (_globalConditionValues.ConditionValues.TryGetValue(conditionNode.ConditionID, out conditionValue))
                {
                    _graph.AddConditionNode(conditionNode, conditionValue, position, false, true);
                    continue;
                }

                Debug.LogError($"failed to find ConditionValue with id {conditionNode.ConditionID}");
            }

            foreach (var eventNode in _dialogueContainer.EventsList)
            {
                var position = _dialogueContainer.NodeData.Find(
                    d => d.nodeID == eventNode.ID).position;
                
                _graph.AddEventNode(eventNode, position, false);
            }

            _graph.ConnectNodes();
        }

        private void CreateGraph()
        {
            _graph = new DialogueGraph(_currentWindow);
            _graph.StretchToParentSize();
            _graph.viewDataKey = $"SimpleDialogGraph{_currentWindow.name}";
            _rootView.Q<VisualElement>("graphContainer").Add(_graph);

            _graph.OnNodesMoved += list =>
            {
                foreach (var nodeView in list)
                {
                    var nodeData = _dialogueContainer.NodeData.Find(
                        d => d.nodeID == nodeView.ID);
                    nodeData.position = nodeView.NodeLayoutPosition;
                }

                Save();
            };
            _graph.OnNodesRemoved += list =>
            {
                foreach (var nodeView in list)
                {
                    var nodeData = _dialogueContainer.NodeData.Find(
                        d => d.nodeID == nodeView.ID);
                    _dialogueContainer.NodeData.Remove(nodeData);
                    _dialogueContainer.RemoveNode(nodeView.DialogueNode);
                }

                Save();
            };
            _graph.OnNewConditionNodeCreate += (pos) =>
            {
                var id = Guid.NewGuid().GetHashCode();
                var localCondition = new ConditionValue
                {
                    ID = Guid.NewGuid().GetHashCode(),
                    Description = "Condition description",
                    Value = 0
                };
                _dialogueContainer.AddConditionValue(localCondition);
                var node = new DialogueConditionNode(id, localCondition);
                _dialogueContainer.AddNode(node);
                _dialogueContainer.NodeData.Add(new DialogueNodeData() { nodeID = id, position = pos });
                Save();
                _localConditionValuesView.Update();
                return (node, localCondition);
            };
            _graph.OnConditionValueChanged += () => _localConditionValuesView.Update();
            _graph.OnFirstNodeChanged += node => _dialogueContainer.FirstNode = node;
        }

        public void Clear()
        {
            Undo.undoRedoPerformed -= UndoRedoPerformed;
            Save();
        }

        private void Save()
        {
            EditorUtility.SetDirty(_dialogueContainer);
            AssetDatabase.SaveAssetIfDirty(_dialogueContainer);
        }
    }
}