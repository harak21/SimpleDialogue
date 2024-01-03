using System;
using System.Threading.Tasks;

namespace SimpleUtils.SimpleDialogue.Runtime.System.Conditions
{
    /// <summary>
    /// stores the current values of the conditions
    /// implement to create your own handler
    /// </summary>
    public interface IDialogueConditionHandler
    {
        /// <summary>
        /// event to be called to notify about the change of the condition value
        /// </summary>
        event Action<int, int> OnConditionStateChanged;
        
        /// <summary>
        /// method, where you can initialize the system as you wish, e.g., load condition values
        /// </summary>
        /// <returns>returns the Task to wait for</returns>
        Task Initialize();
        
        /// <summary>
        /// method, to maintain the current values of the conditions
        /// </summary>
        /// <returns>returns the Task to wait for</returns>
        Task SaveConditions();
        
        /// <summary>
        /// method, to change the current values of the condition
        /// </summary>
        /// <param name="conditionId">conditions id</param>
        /// <param name="newState">new condition value</param>
        void ChangeConditionState(int conditionId, int newState);
        
        /// <summary>
        /// method to request the current value of the condition
        /// </summary>
        /// <param name="conditionId">conditions id</param>
        /// <returns>condition value</returns>
        int GetConditionState(int conditionId);
    }
}