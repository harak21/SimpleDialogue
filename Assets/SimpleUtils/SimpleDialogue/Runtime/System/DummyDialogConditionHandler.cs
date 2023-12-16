using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public class DummyDialogConditionHandler : IDialogueConditionHandler
    {
        public event Action<int, int> OnConditionStateChanged;

        private readonly Dictionary<int, int> _conditions = new();
        
        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public Task SaveConditions()
        {
            return Task.CompletedTask;
        }

        public void ChangeConditionState(int conditionId, int newState)
        {
            _conditions[conditionId] = newState;
            OnConditionStateChanged?.Invoke(conditionId, newState);
        }

        public int GetConditionState(int conditionId)
        {
            return _conditions.TryGetValue(conditionId, out var value) ? value : 0;
        }
    }
}