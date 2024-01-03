using System;
using System.Collections.Generic;
using UnityEngine.LowLevel;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Scene
{
    internal class SceneUpdateLoop : ISceneUpdateLoop
    {
        private readonly List<IUpdatable> _updatableList = new();
        private bool _needClear;
        
        public SceneUpdateLoop()
        {
            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            var arrayLength = currentPlayerLoop.subSystemList.Length;
            Array.Resize(ref currentPlayerLoop.subSystemList, arrayLength + 1);
            currentPlayerLoop.subSystemList[arrayLength] = new PlayerLoopSystem()
            {
                type = typeof(SceneUpdateLoop),
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
}