using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Scene
{
    internal class SceneLoader : ISceneLoader
    {
        public async Task LoadScene(AssetReference sceneReference)
        {
            await Addressables.LoadSceneAsync(sceneReference).Task;
        }
    }
}