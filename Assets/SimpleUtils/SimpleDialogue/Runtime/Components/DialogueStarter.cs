using System;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Runtime.Components
{
    public class DialogueStarter : MonoBehaviour
    {
        public event Action<int> OnDialogueStart;
        
        [SerializeField] private int dialogueID;

        public void StartDialogue()
        {
            OnDialogueStart?.Invoke(dialogueID);
        }

#if UNITY_EDITOR
        public int DialogueID
        {
            get => dialogueID;
            set => dialogueID = value;
        }
#endif
    }
}