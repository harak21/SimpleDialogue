using Samples.SimpleUtils.SimpleDialogue.Input;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Dialogue;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Input;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.ResourcesLoader;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Scene;
using UnityEngine;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts
{
    internal class Bootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            CreateServices();
            var game = new Game(
                AllServices.Get<ISceneLoader>(),
                AllServices.Get<ISceneResourceHandler>(),
                AllServices.Get<ILoadResources>(),
                AllServices.Get<IInputService>(),
                AllServices.Get<ISceneUpdateLoop>(),
                AllServices.Get<IDialogueSystemWrapper>());
        }

        private void CreateServices()
        {
            AllServices.Register<IInputService>(new InputService(new DefaultInputActions()));
            var loadResources = new LoadResources();
            AllServices.Register<ILoadResources>(loadResources);
            AllServices.Register<ISceneResourceHandler>(new SceneResourceHandler());
            AllServices.Register<ISceneLoader>(new SceneLoader());
            AllServices.Register<ISceneUpdateLoop>(new SceneUpdateLoop());
            AllServices.Register<IDialogueSystemWrapper>(new DialogueSystemWrapper(loadResources));
        }
    }
}