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
        [SerializeField] private long tableKey;
        [SerializeField] private long entryKey;
        //[SerializeField] private Actor actor;
        [SerializeField] private int actorID;

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

        public int ActorID
        {
            get => actorID;
            set => actorID = value;
        }

        public DialoguePhraseNode(int id, long entryKey, string tableName, Actor actor)
        {
            ID = id;
            EntryKey = entryKey;
            TableName = tableName;
            ActorID = actor.ID;
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