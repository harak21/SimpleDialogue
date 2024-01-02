using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Runtime.Components;
using UnityEngine;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Scene
{
    internal class DialogEventObserversProvider : MonoBehaviour
    {
        [SerializeField] private List<DialogueEventObserver> observers;

        public List<DialogueEventObserver> Observers => observers;

        private void Awake()
        {
            AllServices.Get<ISceneResourceHandler>().DialogEventObserversProvider = this;
        }
    }
}