using System;
using SimpleUtils.SimpleDialogue.Runtime.Components;
using UnityEngine;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario
{
    internal class DialogueConditionModifier : MonoBehaviour
    {
        public event Action<int, int> OnConditionStateChanged;
        
        [SerializeField] private DialogueEventObserver dialogueEventObserver;
        [SerializeField] private int conditionID;
        [SerializeField] private int newState;

        private void Awake()
        {
            dialogueEventObserver.OnEventOccured += ChangeCondition;
        }

        private void ChangeCondition()
        {
            OnConditionStateChanged?.Invoke(conditionID, newState);
        }
    }
}