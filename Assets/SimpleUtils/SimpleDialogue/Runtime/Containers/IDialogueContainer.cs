using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;

namespace SimpleUtils.SimpleDialogue.Runtime.Containers
{
    public interface IDialogueContainer
    {
        /// <summary>
        /// dialog id. must be unique
        /// </summary>
        int DialogueID { get; }
        
        /// <summary>
        /// id of the first node from which dialog playback will start
        /// </summary>
        int FirstNodeID { get; }
        
        /// <summary>
        /// dialog phrase dictionary. the key is the phrase id
        /// </summary>
        Dictionary<int, DialoguePhraseNode> DialogueNodes { get; }
        
        /// <summary>
        /// dictionary of dialog conditions. the key is the id of the phrase
        /// </summary>
        Dictionary<int, DialogueConditionNode> ConditionNodes { get; }
        
        /// <summary>
        /// dialog event dictionary. the key is the id of the phrase
        /// </summary>
        Dictionary<int, DialogueEventNode> EventNodes { get; }  
        
        /// <summary>
        /// a dictionary of dialog actors. the key is the id of the phrase
        /// </summary>
        Dictionary<int, Actor> Actors { get; }
    }
}