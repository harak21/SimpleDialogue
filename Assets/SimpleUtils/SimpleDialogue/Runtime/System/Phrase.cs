using System;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public class Phrase
    {
        public int PhraseID { get; }
        public string LocalizationTableKey { get;}
        public long LocalizationEntryKey { get;}
        public string CurrentLocalePhrase { get;}
        public string ActorName { get; }
        public int ActorID { get; }

        public Phrase(int id, string localizationTableKey, long localizationEntryKey, string currentLocalePhrase, Actor actor)
        {
            LocalizationTableKey = localizationTableKey;
            LocalizationEntryKey = localizationEntryKey;
            CurrentLocalePhrase = currentLocalePhrase;
            PhraseID = id;
            ActorName = actor.ActorName;
            ActorID = actor.ID;
        }
    }
}