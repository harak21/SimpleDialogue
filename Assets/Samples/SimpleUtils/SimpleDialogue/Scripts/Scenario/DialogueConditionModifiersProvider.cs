using System;
using System.Collections.Generic;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Scene;
using UnityEngine;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario
{
    internal class DialogueConditionModifiersProvider : MonoBehaviour
    {
        [SerializeField] private List<DialogueConditionModifier> modifiers;

        public List<DialogueConditionModifier> Modifiers => modifiers;

        private void Awake()
        {
            AllServices.Get<ISceneResourceHandler>().DialogueConditionModifiersProvider = this;
        }
    }
}