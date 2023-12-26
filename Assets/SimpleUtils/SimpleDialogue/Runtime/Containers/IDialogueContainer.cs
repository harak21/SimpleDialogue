using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using SimpleUtils.SimpleDialogue.Runtime.Utils;

namespace SimpleUtils.SimpleDialogue.Runtime.Containers
{
    public interface IDialogueContainer
    {
        int DialogueID { get; }
        int FirstNodeID { get; }
        SimpleSerializedDictionary<DialoguePhraseNode> DialogueNodes { get; }
        SimpleSerializedDictionary<DialogueConditionNode> ConditionNodes { get; }
        SimpleSerializedDictionary<DialogueEventNode> EventNodes { get; }
        SimpleSerializedDictionary<Actor> Actors { get; }
    }
}