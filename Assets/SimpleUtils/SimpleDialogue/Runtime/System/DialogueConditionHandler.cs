using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleUtils.SimpleDialogue.Runtime.Containers;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public class DialogueConditionHandler : IDialogueConditionHandler
    {
        public event Action<int, int> OnConditionStateChanged; 
        
        private readonly ISaveLoadService _saveLoadService;
        private Dictionary<int, int> _conditionsStates = new();

        public DialogueConditionHandler(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public DialogueConditionHandler() : this(new DummySaveLoadService())
        {
            
        }

        public async Task Initialize()
        {
            _conditionsStates = await _saveLoadService.LoadSavedConditionsStates();
        }

        public Task SaveConditions()
        {
            return _saveLoadService.SaveConditionsState(_conditionsStates);
        }
        
        public void ChangeConditionState(int conditionId, int newState)
        {
            if (!_conditionsStates.TryGetValue(conditionId, out var currentState))
                return;

            if (currentState == newState)
                return;

            _conditionsStates[conditionId] = newState;
            OnConditionStateChanged?.Invoke(conditionId, newState);
        }

        public int GetConditionState(int conditionId)
        {
            return _conditionsStates.TryGetValue(conditionId, out var value) ? value : 0;
        }

    }
}