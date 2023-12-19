using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Graph.Nodes
{
    internal class ConditionNodeView : DialogueNodeView
    {
        public event Action<ConditionValue> OnLocalConditionChanged; 
        
        private readonly DialogueConditionNode _dialogueConditionNode;
        public override IEnumerable<int> NextNodes
        {
            get
            {
                var type = typeof(DialogueConditionNode);
                var fieldInfo = type.GetField("nextNodes", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    var value = fieldInfo.GetValue(_dialogueConditionNode);
                    return (IEnumerable<int>)value;
                }
                Debug.LogError("no field named \"nextNodes\" found");
                return Enumerable.Empty<int>();
            }
        }

        public override int ID => _dialogueConditionNode.ID;
        public override IDialogueNode DialogueNode => _dialogueConditionNode;

        public DialogueConditionNode ConditionNode => _dialogueConditionNode;

        public ConditionNodeView(DialogueConditionNode conditionNode, ConditionValue conditionValue, bool isReadOnly)
        {
            _dialogueConditionNode = conditionNode;
            var visualTreeAsset = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("ConditionNodeView");
            visualTreeAsset.CloneTree(this); 

            var textField = this.Q<TextField>("description");
            textField.value = conditionValue.Description;
            textField.isReadOnly = isReadOnly;
            
            textField.RegisterValueChangedCallback(evt =>
            {
                conditionValue.Description = evt.newValue;
                OnLocalConditionChanged?.Invoke(conditionValue);
            });

            var enumField = this.Q<EnumField>("conditionType");
            enumField.Init(conditionNode.Condition);
            enumField.RegisterValueChangedCallback(evt =>
            {
                conditionNode.Condition = (DialogCondition)evt.newValue;
            });

            var intField = this.Q<IntegerField>("referenceValue");
            intField.value = conditionNode.ReferenceValue;
            intField.RegisterValueChangedCallback(evt =>
            {
                conditionNode.ReferenceValue = evt.newValue;
            });

            tooltip = conditionValue.ID.ToString();
            
            ConstructNode();
        }

        public override void RemoveNextNode(int nextNodeID)
        {
            _dialogueConditionNode.RemoveNextNode(nextNodeID);
        }

        public override void AddNextNode(int nextNodeID)
        {
            _dialogueConditionNode.AddNextNode(nextNodeID);
        }

        public void ChangeDescription(ConditionValue conditionValue)
        {
            var textField = this.Q<TextField>("description");
            textField.value = conditionValue.Description;
        }
    }
}