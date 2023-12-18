using System;
using System.Threading.Tasks;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public interface IDialogueConditionHandler
    {
        event Action<int, int> OnConditionStateChanged;
        Task Initialize();
        Task SaveConditions();
        void ChangeConditionState(int conditionId, int newState);
        int GetConditionState(int conditionId);
    }
}