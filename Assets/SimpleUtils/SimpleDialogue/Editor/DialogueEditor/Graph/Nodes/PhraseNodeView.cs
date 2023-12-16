﻿using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Graph.Nodes
{
    internal class PhraseNodeView : DialogueNodeView
    {
        private readonly DialoguePhraseNode _dialogPhraseNode;
        public override IEnumerable<int> NextNodes => _dialogPhraseNode.NextNodes;
        public override int ID => _dialogPhraseNode.ID;
        public override IDialogueNode DialogueNode => _dialogPhraseNode;

        public PhraseNodeView(Actor actor, DialoguePhraseNode dialogPhraseNode, string title)
        {
            AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("NodeView").CloneTree(this);
            
            _dialogPhraseNode = dialogPhraseNode;
            this.Q<Label>("phraseText").text = title;
            this.Q<Label>("nodeType").text = actor.ActorName;
            
            ConstructNode();
        }
        
        public override void RemoveNextNode(int i)
        {
            _dialogPhraseNode.RemoveNextNode(i);
        }
        
        public override void AddNextNode(int nextNodeID)
        {
            _dialogPhraseNode.AddNextNode(nextNodeID);
        }
    }
}