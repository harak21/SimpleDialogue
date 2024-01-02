using System;
using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Runtime.Components;
using UnityEngine;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Scene
{
    internal class DialogStartersProvider : MonoBehaviour
    {
        [SerializeField] private List<DialogueStarter> dialogueStarters;

        public List<DialogueStarter> DialogueStarters => dialogueStarters;

        private void Awake()
        {
            AllServices.Get<ISceneResourceHandler>().DialogStartersProvider = this;
        }
    }
}