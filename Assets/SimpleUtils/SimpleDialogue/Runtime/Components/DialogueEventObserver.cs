using System;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Runtime.Components
{
    public class DialogueEventObserver : MonoBehaviour
    {
        public event Action OnEventOccured;

        [SerializeField] private int dialogueID;
        [SerializeField] private int eventID;
        [SerializeField] private bool hasValidValue;

        public void EventOccured(int dialogue, int @event)
        {
            if (!hasValidValue)
                return;

            if (dialogueID != dialogue)
                return;

            if (@event != eventID)
                return;
            
            OnEventOccured?.Invoke();
        }

#if UNITY_EDITOR
        public int DialogueID
        {
            get => dialogueID;
            set
            {
                HasValidValue = false;
                dialogueID = value;
            }
        }

        public int EventID
        {
            get => eventID;
            set
            {
                HasValidValue = true;
                eventID = value;
            }
        }

        private bool HasValidValue
        {
            set => hasValidValue = value;
        }
#endif
    }
}