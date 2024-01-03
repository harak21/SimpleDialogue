using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario
{
    [CreateAssetMenu(fileName = "SceneDataContainer", menuName = "SampleSimpleDialogue/SceneDataContainer", order = 0)]
    internal class SceneDataContainer : ScriptableObject
    {
        [SerializeField] private int mainSceneIndex;
        [SerializeField] private List<SceneData> sceneData = new();

        public AssetReference this[int index] => sceneData.Find(d => d.SceneIndex == index).Scene;
        public AssetReference MainScene => sceneData.Find(d => d.SceneIndex == mainSceneIndex).Scene;
    }
}