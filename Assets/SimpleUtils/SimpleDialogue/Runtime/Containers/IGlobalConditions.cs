using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using SimpleUtils.SimpleDialogue.Runtime.Utils;

namespace SimpleUtils.SimpleDialogue.Runtime.Containers
{
    public interface IGlobalConditions
    {
        internal SimpleSerializedDictionary<DialogueConditionNode> GlobalCondition { get; }
    }
}