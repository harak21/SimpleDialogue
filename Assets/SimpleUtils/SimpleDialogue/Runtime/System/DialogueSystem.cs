using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using SimpleUtils.SimpleDialogue.Runtime.System.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.System.Data;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public class DialogueSystem
    {
        /// <summary>
        /// event, which is called when an event is performed in the current dialog
        /// </summary>
        public event Action<DialogueEvent> OnEventOccurred;
        
        private readonly IDialogueConditionHandler _dialogueConditionHandler;
        private readonly LocalizedStringDatabase _stringDatabase;
        private readonly ILoadDialogueService _loadDialogueService;
        
        private IDialogueContainer _currentDialogueContainer;

        private readonly HashSet<DialoguePhraseNode> _currentPhraseNodes = new();

        /// <summary>
        /// constructor that accepts the dialog download service
        /// </summary>
        /// <param name="loadDialogueService">dialog loading service</param>
        public DialogueSystem(ILoadDialogueService loadDialogueService) : this(loadDialogueService, new DummyDialogConditionHandler())
        {
        }

        /// <summary>
        /// constructor that accepts dialog loading service and condition state loading/saving service.
        /// use it if you need this functionality
        /// </summary>
        /// <param name="loadDialogueService">dialog loading service</param>
        /// <param name="dialogueConditionHandler">stores the current values of the conditions</param>
        public DialogueSystem(ILoadDialogueService loadDialogueService, IDialogueConditionHandler dialogueConditionHandler)
        {
            _dialogueConditionHandler = dialogueConditionHandler;
            _loadDialogueService = loadDialogueService;
            _stringDatabase = LocalizationSettings.Instance.GetStringDatabase();
        }

        /// <summary>
        /// try to initialize a new dialog
        /// </summary>
        /// <param name="dialogID">dialog ID</param>
        /// <returns>returns the first phrases of the dialog,
        /// or an empty list if no dialog is found/the first phrase of the dialog is not found.
        /// currently, the starting node will be discarded, regardless of its type</returns>
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

        /// <summary>
        /// returns the phrases that follow the current one
        /// </summary>
        /// <param name="currentPhraseID">the id of the current phrase, for which the following will be selected</param>
        /// <returns></returns>
        public List<Phrase> GetNextPhrases(int currentPhraseID)
        {
            List<Phrase> phrases = new();
            
            if (!_currentDialogueContainer.DialogueNodes.TryGetValue(currentPhraseID, out var previousNode))
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