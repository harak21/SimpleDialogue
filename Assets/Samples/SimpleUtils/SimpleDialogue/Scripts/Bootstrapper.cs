using System;
using System.Collections.Generic;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services;
using UnityEngine;
using UnityEngine.LowLevel;
using Object = UnityEngine.Object;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts
{
    public class Bootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            CreateServices();
            var game = new Game(
                AllServices.Get<SceneLoader>(),
                AllServices.Get<SceneResourceHandler>(),
                AllServices.Get<LoadResources>());
        }

        private void CreateServices()
        {
            AllServices.Register(new LoadResources());
            AllServices.Register(new SceneLoader());
            AllServices.Register(new SceneResourceHandler());
            AllServices.Register(new UpdateLoop());
        }
    }

    public class Game
    {
        private const string MainScene = "MainScene";
        private readonly LoadResources _loadResources;

        public Game(SceneLoader sceneLoader, SceneResourceHandler sceneResourceHandler, LoadResources loadResources)
        {
            _loadResources = loadResources;
            CreateMainHud();
        }

        private async void CreateMainHud()
        {
            var hud = await _loadResources.LoadAsync<GameObject>("MainHUD");
            Object.Instantiate(hud);
        }
    }

    public class UpdateLoop : IService
    {
        private readonly List<IUpdatable> _updatableList = new();
        private bool _needClear;
        
        public UpdateLoop()
        {
            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            var arrayLength = currentPlayerLoop.subSystemList.Length;
            Array.Resize(ref currentPlayerLoop.subSystemList, arrayLength + 1);
            currentPlayerLoop.subSystemList[arrayLength] = new PlayerLoopSystem()
            {
                type = typeof(UpdateLoop),
                updateDelegate = OnUpdate
            };
            PlayerLoop.SetPlayerLoop(currentPlayerLoop);
        }

        public void Register(IUpdatable updatable)
        {
            _updatableList.Add(updatable);
        }

        public void Clear()
        {
            _needClear = true;
        }

        private void OnUpdate()
        {
            if (_needClear)
            {
                _updatableList.Clear();
                _needClear = false;
            }

            foreach (var updatable in _updatableList)
            {
                updatable.OnUpdate();
            }
        }
    }

    public interface IUpdatable
    {
        public void OnUpdate()
        {
            
        }
    }
}