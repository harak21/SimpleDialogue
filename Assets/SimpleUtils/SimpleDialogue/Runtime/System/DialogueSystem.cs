using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public class DialogueSystem
    {
        public event Action<DialogueEvent> OnEventOccurred;
        
        private readonly IDialogueConditionHandler _dialogueConditionHandler;
        private readonly LocalizedStringDatabase _stringDatabase;
        private readonly ILoadDialogueService _loadDialogueService;
        
        private IDialogueContainer _currentDialogueContainer;

        private readonly HashSet<DialoguePhraseNode> _currentPhraseNodes = new();

        public DialogueSystem(ILoadDialogueService loadDialogueService) : this(loadDialogueService, new DummyDialogConditionHandler())
        {
        }

        public DialogueSystem(ILoadDialogueService loadDialogueService, IDialogueConditionHandler dialogueConditionHandler)
        {
            _dialogueConditionHandler = dialogueConditionHandler;
            _loadDialogueService = loadDialogueService;
            _stringDatabase = LocalizationSettings.Instance.GetStringDatabase();
        }

        public async Task<List<Phrase>> TryInitNewDialogue(int dialogID)
        {
            _currentDialogueContainer = null;
            _currentDialogueContainer = await _loadDialogueService.Load(dialogID);

            if (_currentDialogueContainer is null)
            {
                Debug.LogWarning("Dialogue not found");
                return new List<Phrase>();
            }

            var firstNodeID = _currentDialogueContainer.FirstNodeID;
            var firstNode = GetFirstDialogueNode(firstNodeID);

            if (firstNode is null)
            {
                Debug.LogWarning("First dialogue node not found");
                return new List<Phrase>();
            }

            return GetNextPhrases(firstNodeID);
        }

        public List<Phrase> GetNextPhrases(int previousPhraseID)
        {
            List<Phrase> phrases = new();
            
            if (!_currentDialogueContainer.DialogueNodes.TryGetValue(previousPhraseID, out var previousNode))
                return phrases;
            
            _currentPhraseNodes.Clear();
            GetNextNodes(previousNode.NextNodes);

            foreach (var currentNode in _currentPhraseNodes)
            {
                phrases.Add(FillPhraseData(currentNode));
            }

            return phrases;
        }

        private Phrase FillPhraseData(DialoguePhraseNode dialoguePhraseNode)
        {
            var localizedPhrase = _stringDatabase.GetLocalizedString(
                new Guid(dialoguePhraseNode.TableKey), dialoguePhraseNode.EntryKey);
            _currentDialogueContainer.Actors.TryGetValue(dialoguePhraseNode.ActorID, out var actor);
            return new Phrase(dialoguePhraseNode.ID,
                dialoguePhraseNode.TableKey,
                dialoguePhraseNode.EntryKey,
                localizedPhrase,
                actor);
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
                else if (_currentDialogueContainer.EventNodes.TryGetValue(id, out DialogueEventNode dialogueEventNode))
                {
                    OnEventOccurred?.Invoke(new DialogueEvent(dialogueEventNode));
                    GetNextNodes(dialogueEventNode.NextNodes);   
                }
            }
        }
    }
}