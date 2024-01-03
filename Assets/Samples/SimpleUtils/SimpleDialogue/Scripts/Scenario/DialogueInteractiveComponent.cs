using SimpleUtils.SimpleDialogue.Runtime.Components;
using UnityEngine;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario
{
    internal class DialogueInteractiveComponent : InteractiveComponent
    {
        [SerializeField] private DialogueStarter dialogueStarter;
        public override bool CanInteract => true;
        public override string Description => gameObject.name;
        public override void Interact()
        {
            dialogueStarter.StartDialogue();
        }
    }
}