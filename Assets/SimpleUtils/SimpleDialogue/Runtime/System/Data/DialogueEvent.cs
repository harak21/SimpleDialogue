using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;

namespace SimpleUtils.SimpleDialogue.Runtime.System.Data
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