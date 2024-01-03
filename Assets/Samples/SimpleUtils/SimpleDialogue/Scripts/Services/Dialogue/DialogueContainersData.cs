using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using UnityEngine;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services
{
    [CreateAssetMenu(fileName = "DialogueContainersData", menuName = "SampleSimpleDialogue/DialogueContainersData", order = 0)]
    internal class DialogueContainersData : ScriptableObject
    {
        [SerializeField] private List<DialogueContainer> dialogueContainers;

        public DialogueContainer this[int id] => dialogueContainers.Find(c => c.DialogueID == id);
    }
}