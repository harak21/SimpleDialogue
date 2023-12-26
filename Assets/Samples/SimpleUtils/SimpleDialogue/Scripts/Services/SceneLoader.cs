using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services
{
    public class SceneLoader : IService
    {
        public async Task LoadScene(string sceneName)
        {
            if (sceneName == SceneManager.GetActiveScene().name) 
                return;

            await Addressables.LoadSceneAsync(sceneName).Task;
        }
    }
}