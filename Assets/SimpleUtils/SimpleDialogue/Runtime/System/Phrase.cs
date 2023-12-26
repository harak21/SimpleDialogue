using System;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public class Phrase
    {
        public string LocalizationTableKey { get; set; }
        public long LocalizationEntryKey { get; set; }
        public string CurrentLocalePhrase { get; set; }
        public string ActorName { get; set; }

        public Phrase(string localizationTableKey, long localizationEntryKey, string currentLocalePhrase, string actorName)
        {
            LocalizationTableKey = localizationTableKey;
            LocalizationEntryKey = localizationEntryKey;
            CurrentLocalePhrase = currentLocalePhrase;
            ActorName = actorName;
        }
    }
}