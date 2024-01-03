using System.Collections.Generic;
using System.Linq;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.ResourcesLoader;
using Samples.SimpleUtils.SimpleDialogue.Scripts.UI;
using SimpleUtils.SimpleDialogue.Runtime.Components;
using SimpleUtils.SimpleDialogue.Runtime.System;
using SimpleUtils.SimpleDialogue.Runtime.System.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.System.Data;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Dialogue
{
    internal class DialogueSystemWrapper : IDialogueSystemWrapper
    {
        private readonly DialogueSystem _dialogueSystem;
        private List<DialogueStarter> _dialogueStarters;
        private IGameButtonPlace _gameButtonPlace;
        private IDialogPhraseHolder _dialogPhraseHolder;
        private List<Phrase> _currentPhrases;
        private List<DialogueEventObserver> _dialogueEventObservers;
        private int _dialogueId;
        private readonly DialogueConditionHandler _dialogConditionHandler;

        public DialogueSystemWrapper(ILoadResources loadResources)
        {
            var dialogueLoadService = new DialogueLoadService(loadResources);
            _dialogConditionHandler = new DialogueConditionHandler(new JsonConditionSaveLoadService());
            _dialogueSystem = new DialogueSystem(dialogueLoadService, _dialogConditionHandler);
#pragma warning disable CS4014 
            _dialogConditionHandler.Initialize();
#pragma warning restore CS4014 
            _dialogueSystem.OnEventOccurred += EventOccured;
        }

        public async void SaveConditions()
        {
            await _dialogConditionHandler.SaveConditions();
        } 

        public void InitNewScene(List<DialogueStarter> dialogueStarters,
            List<DialogueEventObserver> dialogueEventObservers,
            List<DialogueConditionModifier> conditionModifiers,
            IGameButtonPlace gameButtonPlace,
            IDialogPhraseHolder dialogPhraseHolder)
        {
            _dialogueEventObservers = dialogueEventObservers;
            _dialogPhraseHolder = dialogPhraseHolder;
            _gameButtonPlace = gameButtonPlace;
            
            if (_dialogueStarters != null)
            {
                foreach (var dialogueStarter in _dialogueStarters)
                {
                    dialogueStarter.OnDialogueStart -= OnStartDialogue;
                }
            }

            _dialogueStarters = dialogueStarters;

            foreach (var dialogueStarter in _dialogueStarters)
            {
                dialogueStarter.OnDialogueStart += OnStartDialogue;
            }

            foreach (var conditionModifier in conditionModifiers)
            {
                conditionModifier.OnConditionStateChanged += ChangeConditionState;
            }
        }

        private void EventOccured(DialogueEvent dialogueEvent)
        {
            foreach (var eventObserver in _dialogueEventObservers)
            {
                eventObserver.EventOccured(_dialogueId, dialogueEvent.EventID);
            }
        }

        private void ChangeConditionState(int conditionID, int newState)
        {
            _dialogConditionHandler.ChangeConditionState(conditionID, newState);
        }

        private async void OnStartDialogue(int dialogueId)
        {
            _dialogueId = dialogueId;
            _currentPhrases = await _dialogueSystem.TryInitNewDialogue(dialogueId);
            
            ProcessCurrentPhrases();
        }

        private void OnDialogPhraseChoices(int buttonIndex)
        {
            _dialogPhraseHolder.SetLabelText(_currentPhrases[buttonIndex].CurrentLocalePhrase);
            _currentPhrases = _dialogueSystem.GetNextPhrases(_currentPhrases[buttonIndex].PhraseID);
            
            ProcessCurrentPhrases();
        }

        private void ProcessCurrentPhrases()
        {
            if (_currentPhrases.Count == 0)
            {
                _gameButtonPlace.Clear();
                _dialogPhraseHolder.Clear();
                return;
            }

            //That's too bad, but I'll leave it alone for now.
            if (_currentPhrases[0].ActorName == "Player")
            {
                _gameButtonPlace.SetButtonItems( 
                    _currentPhrases.Select(p => p.CurrentLocalePhrase).ToList(),
                    OnDialogPhraseChoices);
            }
            else
            {
                _dialogPhraseHolder.SetLabelText($"{_currentPhrases[0].ActorName}: {_currentPhrases[0].CurrentLocalePhrase}");
                _currentPhrases = _dialogueSystem.GetNextPhrases(_currentPhrases[0].PhraseID);
                ProcessCurrentPhrases();
            }
        }
        
    }
}