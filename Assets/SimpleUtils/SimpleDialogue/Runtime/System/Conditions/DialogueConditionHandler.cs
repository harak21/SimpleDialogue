using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleUtils.SimpleDialogue.Runtime.System.Conditions
{
    public class DialogueConditionHandler : IDialogueConditionHandler
    {
        /// <summary>
        /// is called if the value of the condition has changed
        /// </summary>
        public event Action<int, int> OnConditionStateChanged; 
        
        private readonly IConditionSaveLoadService _conditionSaveLoadService;
        private Dictionary<int, int> _conditionsStates = new();

        /// <summary>
        /// constructor accepting the save/load service
        /// </summary>
        /// <param name="conditionSaveLoadService">will be used when loading or saving values</param>
        public DialogueConditionHandler(IConditionSaveLoadService conditionSaveLoadService)
        {
            _conditionSaveLoadService = conditionSaveLoadService;
        }

        /// <summary>
        /// default constructor. use if you don't need to save/load values
        /// </summary>
        public DialogueConditionHandler() : this(new DummyConditionSaveLoadService())
        {
            
        }

        /// <summary>
        /// call before system operation, if you want to load values
        /// </summary>
        public async Task Initialize()
        {
            _conditionsStates = await _conditionSaveLoadService.LoadSavedConditionsStates() ?? new();
        }

        /// <summary>
        /// saves the values of the conditions
        /// </summary>
        /// <returns>returns the Task to wait for</returns>
        public Task SaveConditions()
        {
            return _conditionSaveLoadService.SaveConditionsState(_conditionsStates);
        }
        
        /// <summary>
        /// changes the current value of the condition
        /// </summary>
        /// <param name="conditionId">id of the condition that will be changed,
        /// if there is no condition with this id in the dictionary, a new one will be created.</param>
        /// <param name="newState">new condition value</param>
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

        /// <summary>
        /// query the value of the condition
        /// </summary>
        /// <param name="conditionId">id of the condition</param>
        /// <returns>returns the current value of the condition and if the condition is not found, returns 0.</returns>
        public int GetConditionState(int conditionId)
        {
            return _conditionsStates.TryGetValue(conditionId, out var value) ? value : 0;
        }

    }
}