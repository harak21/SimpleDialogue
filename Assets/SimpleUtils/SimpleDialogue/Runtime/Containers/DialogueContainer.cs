using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using SimpleUtils.SimpleDialogue.Runtime.Utils;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Runtime.Containers
{
    [CreateAssetMenu(fileName = "[Name]DialogContainer", menuName = "SimpleFlow/DialogContainer")]
    internal class DialogueContainer : ScriptableObject, IDialogueContainer
    {
        [SerializeField] private DialogueConditionNode firstNode;
        [SerializeField] private SimpleSerializedDictionary<DialoguePhraseNode> dialogueNodes = new();
        [SerializeField] private SimpleSerializedDictionary<DialogueConditionNode> conditionNodes = new();
        [SerializeField] private SimpleSerializedDictionary<Actor> actors = new();
        [SerializeField] private SimpleSerializedDictionary<ConditionValue> conditionValues = new();
        
        public SimpleSerializedDictionary<DialoguePhraseNode> DialogueNodes => dialogueNodes;
        public SimpleSerializedDictionary<DialogueConditionNode> ConditionNodes => conditionNodes;
        public SimpleSerializedDictionary<ConditionValue> ConditionValues => conditionValues;
        public SimpleSerializedDictionary<Actor> Actors => actors;
        public DialogueConditionNode FirstNode => firstNode;

#if UNITY_EDITOR
        [SerializeField] private List<DialogueNodeData> nodeData = new();
        [SerializeField] private List<ActorData> actorsData = new();

        internal List<DialogueNodeData> NodeData => nodeData;
        internal List<ActorData> ActorsData => actorsData;

        internal List<Actor> ActorsList => Actors.GetValues();
        internal List<DialoguePhraseNode> PhrasesList => dialogueNodes.GetValues();
        internal List<DialogueConditionNode> ConditionsList => conditionNodes.GetValues();
        internal List<ConditionValue> ConditionReferencesList => conditionValues.GetValues();
        
        internal void AddNode(IDialogueNode phraseNode)
        {
            switch (phraseNode)
            {
                case DialoguePhraseNode dialoguePhraseNode:
                    dialogueNodes.Add(dialoguePhraseNode);
                    break;
                case DialogueConditionNode dialogueConditionNode:
                    conditionNodes.Add(dialogueConditionNode);
                    break;
            }
        }

        internal void RemoveNode(IDialogueNode phraseNode)
        {
            switch (phraseNode)
            {
                case DialoguePhraseNode dialoguePhraseNode:
                    dialogueNodes.Remove(dialoguePhraseNode);
                    break;
                case DialogueConditionNode dialogueConditionNode:
                    conditionNodes.Remove(dialogueConditionNode);
                    break;
            }
        }

        internal void AddActor(Actor actor)
        {
            actors.Add(actor);
        }

        internal void RemoveActor(Actor actor)
        {
            actors.Remove(actor);
        }

        [ContextMenu("ClearAll")]
        public void ClearAll()
        {
            dialogueNodes.Clear();
            nodeData.Clear();
            conditionNodes.Clear();
            actors.Clear();
            actorsData.Clear();
        }
#endif
    }
}