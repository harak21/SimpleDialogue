using System;
using System.Collections.Generic;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Scene;
using UnityEngine;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario
{
    internal class InteractiveComponentsProvider : MonoBehaviour
    {
        [SerializeField] private List<InteractiveComponent> interactiveComponents;

        public List<InteractiveComponent> InteractiveComponents => interactiveComponents;

        private void Awake()
        {
            AllServices.Get<ISceneResourceHandler>().InteractiveComponentsProvider = this;
        }
    }
}