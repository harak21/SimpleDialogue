using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;

namespace SimpleUtils.SimpleDialogue.Editor.Conditions.ConditionLoader
{
    public interface IConditionLoader
    {
        List<DialogueConditionNode> Load();
    }
}