using System;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Constants;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.ResourcesLoader;
using Samples.SimpleUtils.SimpleDialogue.Scripts.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts
{
    internal class MainMenuFlow
    {
        public event Action<int> OnLoadScene;
        
        private readonly ILoadResources _loadResources;
        private MainHud _mainHud;

        public MainMenuFlow(ILoadResources loadResources)
        {
            _loadResources = loadResources;
            CreateMainHud();
        }

        public void HandleMainScene()
        {
            _mainHud.Show();
        }
        
        private async void CreateMainHud()
        {
            var hudAsset = await _loadResources.LoadAsync<GameObject>(AddressableConstants.MainHud);
            var hud = Object.Instantiate(hudAsset);
            _mainHud = hud.GetComponent<MainHud>();
            _mainHud.OnLoadScene += LoadScene;
        }
        
        private void LoadScene(int sceneIndex)
        {
            _mainHud.Hide();
            OnLoadScene?.Invoke(sceneIndex);
        }
    }
}