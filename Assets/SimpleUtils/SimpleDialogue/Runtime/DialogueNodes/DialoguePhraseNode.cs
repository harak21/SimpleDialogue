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
        [SerializeField] private string tableKey;
        [SerializeField] private long entryKey;
        [SerializeField] private int actorID;

        public int ID
        {
            get => id;
            set => id = value;
        }

        public IEnumerable<int> NextNodes => nextNodes;

        /// <summary>
        /// value key in the localization table
        /// </summary>
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

        /// <summary>
        /// localization table key
        /// </summary>
        public string TableKey
        {
            get => tableKey;
            set => tableKey = value;
        }

        public DialoguePhraseNode(int id, long entryKey, Guid tableKey, Actor actor)
        {
            ID = id;
            EntryKey = entryKey;
            ActorID = actor.ID;
            TableKey = tableKey.ToString();
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