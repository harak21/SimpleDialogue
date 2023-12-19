using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Graph.Nodes
{
    internal class EventNodeView : DialogueNodeView
    {
        private readonly DialogueEventNode _dialogueEventNode;
        public override IEnumerable<int> NextNodes => _dialogueEventNode.NextNodes;
        public override int ID => _dialogueEventNode.ID;
        public override IDialogueNode DialogueNode => _dialogueEventNode;

        public EventNodeView(DialogueEventNode eventNode)
        {
            AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("EventNodeView").CloneTree(this);
            
            _dialogueEventNode = eventNode;
            tooltip = eventNode.ID.ToString();

            var eventName = this.Q<TextField>("eventName");
            eventName.value = eventNode.EventName;
            eventName.RegisterValueChangedCallback(evt => eventNode.EventName = evt.newValue);
            
            ConstructNode();
        }
        
        public override void RemoveNextNode(int nextNodeID)
        {
            _dialogueEventNode.RemoveNextNode(nextNodeID);
        }

        public override void AddNextNode(int nextNodeID)
        {
            _dialogueEventNode.AddNextNode(nextNodeID);
        }
    }
}