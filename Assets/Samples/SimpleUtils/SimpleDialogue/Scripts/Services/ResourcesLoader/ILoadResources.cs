using System.Threading.Tasks;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.ResourcesLoader
{
    internal interface ILoadResources :  IService
    {
        Task<T> LoadAsync<T>(string address) where T : class;
    }
}