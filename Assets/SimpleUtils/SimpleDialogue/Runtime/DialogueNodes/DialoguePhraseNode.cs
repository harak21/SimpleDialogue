using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Runtime.DialogueNodes
{
    [Serializable]
    public class DialoguePhraseNode : IDialogueNode
    {
        [SerializeField] private int id;
        [SerializeField] private List<int> nextNodes = new();
        [SerializeField] private string tableName;
        [SerializeField] private long entryKey;
        [SerializeField] private Actor actor;

        public int ID
        {
            get => id;
            set => id = value;
        }

        public IEnumerable<int> NextNodes => nextNodes;
        
        public string TableName
        {
            get => tableName;
            private set => tableName = value;
        }

        public long EntryKey
        {
            get => entryKey; 
            private set => entryKey = value;
        }

        public Actor Actor
        {
            get => actor;
            set => actor = value;
        }

        public DialoguePhraseNode(int id, long entryKey, string tableName, Actor actor)
        {
            ID = id;
            EntryKey = entryKey;
            TableName = tableName;
            this.actor = actor;
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