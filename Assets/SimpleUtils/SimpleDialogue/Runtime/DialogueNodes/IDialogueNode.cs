using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Runtime.Utils;

namespace SimpleUtils.SimpleDialogue.Runtime.DialogueNodes
{
    internal interface IDialogueNode : ISerializedDictionaryValue
    {
        public IEnumerable<int> NextNodes { get; }
    }
}