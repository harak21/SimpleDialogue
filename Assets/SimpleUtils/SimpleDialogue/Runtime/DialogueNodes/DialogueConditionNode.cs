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

        /// <summary>
        /// the id of the condition that will affect the state of the node
        /// </summary>
        public int ConditionID => conditionID;

        /// <summary>
        /// condition defining the state of the node
        /// </summary>
        internal DialogCondition Condition
        {
            get => condition;
            set => condition = value;
        }

        /// <summary>
        /// value to which the current value of the condition will be compared
        /// </summary>
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

        /// <summary>
        /// set the current value of the condition. It will be compared to the reference value.
        /// e.g. if the state is greater than, the condition will be executed if the current value is greater than the reference value
        /// </summary>
        /// <param name="currentValue">current condition value </param>
        public void SetCurrentConditionValue(int currentValue)
        {
            _isConditionQualify = condition switch
            {
                DialogCondition.Equal => currentValue == referenceValue,
                DialogCondition.Great => currentValue > referenceValue,
                DialogCondition.Less => currentValue < referenceValue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        internal void AddNextNode(int nextNodeID) 
        {
            nextNodes.Add(nextNodeID);
        }

        internal void RemoveNextNode(int nextNodeID)
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