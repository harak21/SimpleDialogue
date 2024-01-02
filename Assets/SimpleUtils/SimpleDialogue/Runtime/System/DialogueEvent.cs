using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using NotImplementedException = System.NotImplementedException;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public class DialogueEvent
    {
        public int EventID { get; }
        public string EventName { get; }

        public DialogueEvent(DialogueEventNode dialogueEventNode)
        {
            EventID = dialogueEventNode.ID;
            EventName = dialogueEventNode.EventName;
        }
    }
}