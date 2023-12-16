using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEngine.Localization.Settings;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public class DialogueSystem
    {
        private readonly IDialogueConditionHandler _dialogueConditionHandler;
        private readonly LocalizedStringDatabase _stringDatabase;
        private readonly ILoadDataService _loadDataService;
        
        private IDialogueContainer _currentDialogueContainer;

        private readonly HashSet<DialoguePhraseNode> _currentNodes = new();

        public DialogueSystem(ILoadDataService loadDataService) : this(loadDataService, new DummyDialogConditionHandler())
        {
        }

        public DialogueSystem(ILoadDataService loadDataService, IDialogueConditionHandler dialogueConditionHandler)
        {
            _dialogueConditionHandler = dialogueConditionHandler;
            _loadDataService = loadDataService;
            _stringDatabase = LocalizationSettings.Instance.GetStringDatabase();
        }
        

        public async Task InitNewDialogue(int dialogID)
        {
            _currentDialogueContainer = await _loadDataService.Load(dialogID);

            var firstNode = _currentDialogueContainer.FirstNode;
            _currentNodes.Clear();
            
            GetNextNodes(firstNode.NextNodes);
        }

        public Dictionary<string, Actor> GetNextPhrases()
        {
            Dictionary<string, Actor> nodeData = new();

            foreach (var currentNode in _currentNodes)
            {
                var phrase = _stringDatabase.GetLocalizedString(
                    currentNode.TableName, currentNode.EntryKey);
                nodeData.Add(phrase, currentNode.Actor);
            }
            
            var selectNextNodesId = _currentNodes.Select(n => n.NextNodes);
            _currentNodes.Clear();
            foreach (var nodeID in selectNextNodesId)
            {
                GetNextNodes(nodeID);
            }

            return nodeData;
        }

        private void GetNextNodes(IEnumerable<int> nextNodesId)
        {
            foreach (var id in nextNodesId)
            {
                if (_currentDialogueContainer.DialogueNodes.TryGetValue(id, out var dialoguePhraseNode))
                {
                    _currentNodes.Add(dialoguePhraseNode);
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