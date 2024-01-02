using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Scene
{
    internal interface ISceneLoader : IService
    {
        Task LoadScene(AssetReference sceneReference);
    }
}