using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public class DialogueConditionHandler : IDialogueConditionHandler
    {
        public event Action<int, int> OnConditionStateChanged; 
        
        private readonly IConditionSaveLoadService _conditionSaveLoadService;
        private Dictionary<int, int> _conditionsStates = new();

        public DialogueConditionHandler(IConditionSaveLoadService conditionSaveLoadService)
        {
            _conditionSaveLoadService = conditionSaveLoadService;
        }

        public DialogueConditionHandler() : this(new DummyConditionSaveLoadService())
        {
            
        }

        public async Task Initialize()
        {
            _conditionsStates = await _conditionSaveLoadService.LoadSavedConditionsStates() ?? new();
        }

        public Task SaveConditions()
        {
            return _conditionSaveLoadService.SaveConditionsState(_conditionsStates);
        }
        
        public void ChangeConditionState(int conditionId, int newState)
        {
            if (_conditionsStates.TryGetValue(conditionId, out var currentState))
            {
                if (currentState == newState)
                    return;
            }
            
            _conditionsStates[conditionId] = newState;
            OnConditionStateChanged?.Invoke(conditionId, newState);
        }

        public int GetConditionState(int conditionId)
        {
            return _conditionsStates.TryGetValue(conditionId, out var value) ? value : 0;
        }

    }
}