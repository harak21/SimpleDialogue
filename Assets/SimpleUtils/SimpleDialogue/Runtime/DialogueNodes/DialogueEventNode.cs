using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Runtime.DialogueNodes
{
    [Serializable]
    public class DialogueEventNode : IDialogueNode
    {
        [SerializeField] private int id;
        [SerializeField] private List<int> nextNodes = new();
        [SerializeField] private string eventName;
        
        public int ID
        {
            get => id;
            set => id = value;
        }

        public IEnumerable<int> NextNodes => nextNodes;

        public string EventName
        {
            get => eventName;
            set => eventName = value;
        }

        public DialogueEventNode(int id)
        {
            ID = id;
            EventName = "new Event";
        }
        
        public void AddNextNode(int nextNodeID) 
        {
            nextNodes.Add(nextNodeID);
        }

        public void RemoveNextNode(int nextNodeID)
        {
            nextNodes.Remove(nextNodeID); 
        }
    }
}