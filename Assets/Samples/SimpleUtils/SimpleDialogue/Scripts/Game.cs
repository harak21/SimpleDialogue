using Samples.SimpleUtils.SimpleDialogue.Scripts.Constants;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Dialogue;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Input;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.ResourcesLoader;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Scene;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts
{
    internal class Game
    {
        private readonly ILoadResources _loadResources;
        private readonly IInputService _inputService;
        private readonly ISceneUpdateLoop _sceneUpdateLoop;
        private readonly IDialogueSystemWrapper _dialogueSystemWrapper;
        private readonly ISceneLoader _sceneLoader;
        private readonly ISceneResourceHandler _sceneResourceHandler;
        private SceneDataContainer _sceneDataContainer;
        private SceneFlow _sceneFlowHandler;
        private MainMenuFlow _mainMenuHandler;

        public Game(ISceneLoader sceneLoader,
            ISceneResourceHandler sceneResourceHandler,
            ILoadResources loadResources,
            IInputService inputService,
            ISceneUpdateLoop sceneUpdateLoop, 
            IDialogueSystemWrapper dialogueSystemWrapper)
        {
            _sceneLoader = sceneLoader;
            _sceneResourceHandler = sceneResourceHandler;
            _loadResources = loadResources;
            _inputService = inputService;
            _sceneUpdateLoop = sceneUpdateLoop;
            _dialogueSystemWrapper = dialogueSystemWrapper;

            Initialize();
        }

        private async void Initialize()
        {
            _sceneDataContainer = 
                await _loadResources.LoadAsync<SceneDataContainer>(AddressableConstants.SceneDataContainer);
            
            _mainMenuHandler = new MainMenuFlow(_loadResources);
            _mainMenuHandler.OnLoadScene += LoadSampleScene;

            _sceneFlowHandler = new SceneFlow(_loadResources, 
                _sceneResourceHandler, 
                _inputService, 
                _sceneUpdateLoop,
                _dialogueSystemWrapper);
            _sceneFlowHandler.OnLoadMainMenu += LoadMainMenu;
            
            LoadMainMenu();
        }

        private async void LoadMainMenu()
        {
            await _sceneLoader.LoadScene(_sceneDataContainer.MainScene);
            _mainMenuHandler.HandleMainScene();
        }

        private async void LoadSampleScene(int loadSceneIndex)
        {
            await _sceneLoader.LoadScene(_sceneDataContainer[loadSceneIndex]);
            _sceneFlowHandler.HandleScene();
        }
    }
}