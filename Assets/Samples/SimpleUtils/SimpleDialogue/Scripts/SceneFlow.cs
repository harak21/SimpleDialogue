using System;
using System.Threading.Tasks;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Constants;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Dialogue;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Input;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.ResourcesLoader;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Scene;
using Samples.SimpleUtils.SimpleDialogue.Scripts.UI;
using UnityEngine;
using UnityEngine.AI;
using CharacterController = Samples.SimpleUtils.SimpleDialogue.Scripts.Character.CharacterController;
using Object = UnityEngine.Object;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts
{
    internal class SceneFlow
    {
        public event Action OnLoadMainMenu;
        
        private readonly ILoadResources _loadResources;
        private readonly ISceneResourceHandler _sceneResourceHandler;
        private readonly IInputService _inputService;
        private readonly ISceneUpdateLoop _sceneUpdateLoop;
        private readonly IDialogueSystemWrapper _dialogueSystemWrapper;
        private GameObject _character;
        private InteractiveSystem _interactiveSystem;
        private SceneHud _sceneHud;

        public SceneFlow(ILoadResources loadResources,
            ISceneResourceHandler sceneResourceHandler,
            IInputService inputService,
            ISceneUpdateLoop sceneUpdateLoop,
            IDialogueSystemWrapper dialogueSystemWrapper)
        {
            _loadResources = loadResources;
            _sceneResourceHandler = sceneResourceHandler;
            _inputService = inputService;
            _sceneUpdateLoop = sceneUpdateLoop;
            _dialogueSystemWrapper = dialogueSystemWrapper;
        }

        public async void HandleScene()
        {
            await ConstructPlayer();
            await ConstructSceneHud();
            ConstructInteractiveSystem();
            _dialogueSystemWrapper.InitNewScene(
                _sceneResourceHandler.DialogStartersProvider.DialogueStarters,
                _sceneResourceHandler.DialogEventObserversProvider.Observers,
                _sceneResourceHandler.DialogueConditionModifiersProvider.Modifiers,
                _sceneHud,
                _sceneHud);
        }

        private async Task ConstructSceneHud()
        {
            var sceneHudAsset = await _loadResources.LoadAsync<GameObject>(AddressableConstants.SceneHud);
            var sceneHudGo = Object.Instantiate(sceneHudAsset);
            _sceneHud = sceneHudGo.GetComponent<SceneHud>(); 
            _sceneHud.ExitButton.clicked += ExitButtonClicked;
            _sceneHud.Show();
        }

        private void ExitButtonClicked()
        {
            _sceneHud.Hide();
            _interactiveSystem.Cleanup();
            _dialogueSystemWrapper.SaveConditions();
            _sceneUpdateLoop.Clear();
            OnLoadMainMenu?.Invoke();
        }
        
        private void ConstructInteractiveSystem()
        {
            _interactiveSystem = new InteractiveSystem(_sceneHud,
                _character.transform,
                _sceneResourceHandler.InteractiveComponentsProvider.InteractiveComponents);
        }

        private async Task ConstructPlayer()
        {
            var characterAsset = await _loadResources.LoadAsync<GameObject>(AddressableConstants.Character);
            _character = Object.Instantiate(characterAsset);
            var agent = _character.GetComponent<NavMeshAgent>();
            var controller = new CharacterController(_inputService, agent);
            
            _sceneUpdateLoop.Register(controller);
        }
    }
}