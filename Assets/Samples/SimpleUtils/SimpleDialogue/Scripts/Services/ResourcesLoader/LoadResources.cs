using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.ResourcesLoader
{
    internal class LoadResources : ILoadResources
    {
        private readonly Dictionary<string, AsyncOperationHandle> _cachedHandles = new();

        public async Task<T> LoadAsync<T>(string address) where T : class
        {
            if (_cachedHandles.TryGetValue(address, out AsyncOperationHandle completedHandle))
                return await completedHandle.Task as T;
            
            return await RunWithCacheOnComplete(
                Addressables.LoadAssetAsync<T>(address),
                cacheKey:address);
        }
        
        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            var result = await handle.Task;
            _cachedHandles[cacheKey] = handle;
            return result;
        }
    }
}