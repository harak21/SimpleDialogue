using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using SimpleUtils.SimpleDialogue.Runtime.Utils;

namespace SimpleUtils.SimpleDialogue.Runtime.Containers
{
    public interface IDialogueContainer
    {
        DialogueConditionNode FirstNode { get; }
        SimpleSerializedDictionary<DialoguePhraseNode> DialogueNodes { get; }
        SimpleSerializedDictionary<DialogueConditionNode> ConditionNodes { get; }
        SimpleSerializedDictionary<ConditionValue> ConditionValues { get; }
    }
}