using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario
{
    [Serializable]
    internal class SceneData
    {
        [SerializeField] private int sceneIndex;
        [SerializeField] private AssetReference scene;

        public int SceneIndex => sceneIndex;

        public AssetReference Scene => scene;
    }
}