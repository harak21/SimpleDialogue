using System;
using System.Collections.Generic;
using System.Linq;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Runtime.DialogueNodes
{
    [Serializable]
    public class DialogueConditionNode : IDialogueNode
    {
        [SerializeField] private int id;
        [SerializeField] private int conditionID;
        [SerializeField] private int referenceValue;
        [SerializeField] private List<int> nextNodes = new();
        [SerializeField] private DialogCondition condition = DialogCondition.Equal;

        private bool _isConditionQualify;
        public IEnumerable<int> NextNodes => _isConditionQualify ? nextNodes : Enumerable.Empty<int>();
        public int ID 
        {
            get => id;
            set => id = value;
        }

        public int ConditionID => conditionID;

        internal DialogCondition Condition
        {
            get => condition;
            set => condition = value;
        }

        internal int ReferenceValue
        {
            get => referenceValue;
            set => referenceValue = value;
        }

        public DialogueConditionNode(int id, ConditionValue conditionValue)
        {
            ID = id;
            conditionID = conditionValue.ID;
        }

        public void SetCurrentConditionValue(int currentValue)
        {
            _isConditionQualify = condition switch
            {
                DialogCondition.Equal => currentValue == referenceValue,
                DialogCondition.Great => currentValue < referenceValue,
                DialogCondition.Less => currentValue > referenceValue,
                _ => throw new ArgumentOutOfRangeException()
            };
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

    internal enum DialogCondition : byte
    {
        Equal,
        Great,
        Less
    }
}