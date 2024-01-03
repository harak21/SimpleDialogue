using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;

namespace SimpleUtils.SimpleDialogue.Runtime.System.Data
{
    public class Phrase
    {
        /// <summary>
        /// phrase id
        /// </summary>
        public int PhraseID { get; }
        
        /// <summary>
        /// localization table key
        /// </summary>
        public string LocalizationTableKey { get;}
        
        /// <summary>
        /// row key in the localization table
        /// </summary>
        public long LocalizationEntryKey { get;}
        
        /// <summary>
        /// phrase translated into the selected language
        /// </summary>
        public string CurrentLocalePhrase { get;}
        
        /// <summary>
        /// the name of the actor to whom the phrase belongs
        /// </summary>
        public string ActorName { get; }
        
        /// <summary>
        /// the id of the actor to whom the phrase belongs
        /// </summary>
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