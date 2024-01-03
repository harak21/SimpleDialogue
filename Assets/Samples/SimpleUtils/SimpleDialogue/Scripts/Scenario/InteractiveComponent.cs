using UnityEngine;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario
{
    internal abstract class InteractiveComponent : MonoBehaviour
    {
        [SerializeField] private Transform interactionPoint;

        public Vector3 InteractionPoint => interactionPoint.position;
        public abstract bool CanInteract { get; }
        public abstract string Description { get; }

        public abstract void Interact();
    }
}