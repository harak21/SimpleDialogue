using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public class DialogueSystem
    {
        private readonly IDialogueConditionHandler _dialogueConditionHandler;
        private readonly LocalizedStringDatabase _stringDatabase;
        private readonly ILoadDataService _loadDataService;
        
        private IDialogueContainer _currentDialogueContainer;

        private readonly HashSet<DialoguePhraseNode> _currentPhraseNodes = new();

        public DialogueSystem(ILoadDataService loadDataService) : this(loadDataService, new DummyDialogConditionHandler())
        {
        }

        public DialogueSystem(ILoadDataService loadDataService, IDialogueConditionHandler dialogueConditionHandler)
        {
            _dialogueConditionHandler = dialogueConditionHandler;
            _loadDataService = loadDataService;
            _stringDatabase = LocalizationSettings.Instance.GetStringDatabase();
        }

        public async Task<bool> TryInitNewDialogue(int dialogID)
        {
            _currentDialogueContainer = null;
            _currentDialogueContainer = await _loadDataService.Load(dialogID);

            if (_currentDialogueContainer is null)
            {
                Debug.LogWarning("Dialogue not found");
                return false;
            }

            var firstNodeID = _currentDialogueContainer.FirstNodeID;
            var firstNode = GetFirstDialogueNode(firstNodeID);

            if (firstNode is null)
            {
                Debug.LogWarning("First dialogue node not found");
                return false;
            }
            
            _currentPhraseNodes.Clear();
            
            GetNextNodes(firstNode.NextNodes);

            return true;
        }

        public List<Phrase> GetNextPhrases()
        {
            List<Phrase> phrases = new();

            foreach (var currentNode in _currentPhraseNodes)
            {
                var localizedPhrase = _stringDatabase.GetLocalizedString(
                    new Guid(currentNode.TableKey), currentNode.EntryKey);
                _currentDialogueContainer.Actors.TryGetValue(currentNode.ActorID, out var actor);
                phrases.Add(new Phrase(currentNode.TableKey, currentNode.EntryKey, localizedPhrase, actor.ActorName));
            }
            
            var selectNextNodesId = _currentPhraseNodes.Select(n => n.NextNodes);
            _currentPhraseNodes.Clear();
            foreach (var nodeID in selectNextNodesId)
            {
                GetNextNodes(nodeID);
            }

            return phrases;
        }

        private IDialogueNode GetFirstDialogueNode(int firstNodeID)
        {
            IDialogueNode firstNode = null;
            
            if (_currentDialogueContainer.DialogueNodes.TryGetValue(firstNodeID, out var dialoguePhraseNode))
            {
                firstNode = dialoguePhraseNode;
            }
            else if (_currentDialogueContainer.ConditionNodes.TryGetValue(firstNodeID, out var conditionNode))
            {
                firstNode = conditionNode;
            }
            else if (_currentDialogueContainer.EventNodes.TryGetValue(firstNodeID, out var eventNode))
            {
                firstNode = eventNode;
            }

            return firstNode;
        }

        private void GetNextNodes(IEnumerable<int> nextNodesId)
        {
            foreach (var id in nextNodesId)
            {
                if (_currentDialogueContainer.DialogueNodes.TryGetValue(id, out var dialoguePhraseNode))
                {
                    _currentPhraseNodes.Add(dialoguePhraseNode);
                }
                else if (_currentDialogueContainer.ConditionNodes.TryGetValue(id, out DialogueConditionNode conditionNode))
                {
                    var conditionState = _dialogueConditionHandler.GetConditionState(conditionNode.ConditionID);
                    conditionNode.SetCurrentConditionValue(conditionState);
                    GetNextNodes(conditionNode.NextNodes);   
                }
            }
        }
    }
}