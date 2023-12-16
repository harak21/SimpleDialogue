using System;
using System.Collections.Generic;
using UnityEngine.Localization.Tables;

namespace SimpleUtils.SimpleDialogue.Runtime.DialogueNodes
{
    [Serializable]
    internal class ActorData
    {
        public int actorID;
        public List<SharedTableData> tables = new ();
    }
}