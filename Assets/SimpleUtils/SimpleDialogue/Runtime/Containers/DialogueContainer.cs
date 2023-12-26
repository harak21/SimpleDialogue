using System;
using System.Collections;
using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using SimpleUtils.SimpleDialogue.Runtime.Utils;
using UnityEditor;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Runtime.Containers
{
    [CreateAssetMenu(fileName = "[Name]DialogContainer", menuName = "SimpleFlow/DialogContainer")]
    internal class DialogueContainer : ScriptableObject, IDialogueContainer, IConditionValuesProvider
    {
        [SerializeField] private int dialogueID;
        [SerializeField] private int firstNodeID;
        [SerializeField] private SimpleSerializedDictionary<DialoguePhraseNode> dialogueNodes = new();
        [SerializeField] private SimpleSerializedDictionary<DialogueConditionNode> conditionNodes = new();
        [SerializeField] private SimpleSerializedDictionary<Actor> actors = new();
        [SerializeField] private SimpleSerializedDictionary<ConditionValue> conditionValues = new();
        [SerializeField] private SimpleSerializedDictionary<DialogueEventNode> eventNodes = new();

        public int DialogueID => dialogueID;

        public int FirstNodeID
        {
            get => firstNodeID;
            set => firstNodeID = value;
        }

        public SimpleSerializedDictionary<DialoguePhraseNode> DialogueNodes => dialogueNodes;
        public SimpleSerializedDictionary<DialogueConditionNode> ConditionNodes => conditionNodes;
        public SimpleSerializedDictionary<DialogueEventNode> EventNodes { get; }
        public SimpleSerializedDictionary<ConditionValue> ConditionValues => conditionValues;
        public SimpleSerializedDictionary<Actor> Actors => actors;

#if UNITY_EDITOR
        private void Reset()
        {
            dialogueID = Guid.NewGuid().GetHashCode();
        }

        [SerializeField] private List<DialogueNodeData> nodeData = new();
        [SerializeField] private List<ActorData> actorsData = new();

        internal List<DialogueNodeData> NodeData => nodeData;
        internal List<ActorData> ActorsData => actorsData;

        internal List<Actor> ActorsList => Actors.GetValues();
        internal List<DialoguePhraseNode> PhrasesList => dialogueNodes.GetValues();
        internal List<DialogueConditionNode> ConditionsList => conditionNodes.GetValues();
        internal List<ConditionValue> ConditionValuesList => conditionValues.GetValues();
        internal List<DialogueEventNode> EventsList => eventNodes.GetValues();

        internal void AddNode(IDialogueNode phraseNode)
        {
            Undo.RecordObject(this, "Node added");

            if (PhrasesList.Count == 0 && ConditionsList.Count == 0 && EventsList.Count == 0)
            {
                FirstNodeID = phraseNode.ID;
            }
            
            switch (phraseNode)
            {
                case DialoguePhraseNode dialoguePhraseNode:
                    dialogueNodes.Add(dialoguePhraseNode);
                    break;
                case DialogueConditionNode dialogueConditionNode:
                    conditionNodes.Add(dialogueConditionNode);
                    break;
                case DialogueEventNode dialogueEventNode:
                    eventNodes.Add(dialogueEventNode);
                    break;
            }

            EditorUtility.SetDirty(this);
        }

        internal void RemoveNode(IDialogueNode phraseNode)
        {
            Undo.RecordObject(this, "Node removed");
            
            switch (phraseNode)
            {
                case DialoguePhraseNode dialoguePhraseNode:
                    dialogueNodes.Remove(dialoguePhraseNode);
                    break;
                case DialogueConditionNode dialogueConditionNode:
                    conditionNodes.Remove(dialogueConditionNode);
                    break;
                case DialogueEventNode dialogueEventNode:
                    eventNodes.Remove(dialogueEventNode);
                    break;
            }
            
            EditorUtility.SetDirty(this);
        }

        internal void AddConditionValue(ConditionValue conditionValue)
        {
            Undo.RecordObject(this, "Condition value added");
            conditionValues.Add(conditionValue);
            EditorUtility.SetDirty(this);
        }

        internal void AddActor(Actor actor)
        {
            Undo.RecordObject(this, "Actor added");
            actors.Add(actor);
            EditorUtility.SetDirty(this);
        }

        internal void RemoveActor(Actor actor)
        {
            Undo.RecordObject(this, "Actor removed");
            actors.Remove(actor);
            EditorUtility.SetDirty(this);
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