using System;
using System.Collections.Generic;

namespace SimpleUtils.SimpleDialogue.Runtime.DialogueNodes
{
    [Serializable]
    internal class ActorData
    {
        public int actorID;
        public List<string> tables = new ();
    }
}