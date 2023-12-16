using System;
using SimpleUtils.SimpleDialogue.Runtime.Utils;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Runtime.DialogueNodes
{
    [Serializable]
    public class Actor : ISerializedDictionaryValue
    {
        [SerializeField] private int id;
        [SerializeField] private string actorName;

        public int ID
        {
            get => id; 
            set => id = value;
        }

        public string ActorName
        {
            get => actorName;
            set => actorName = value;
        }

        public Actor(int id)
        {
            this.id = id;
        }

    }
}