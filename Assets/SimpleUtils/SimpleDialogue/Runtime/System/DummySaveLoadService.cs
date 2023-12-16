using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    internal class DummySaveLoadService : ISaveLoadService
    {
        public Task<Dictionary<int, int>> LoadSavedConditionsStates()
        {
            return Task.FromResult(new Dictionary<int, int>());
        }

        public Task SaveConditionsState(Dictionary<int, int> states)
        {
            return Task.CompletedTask;
        }
    }
}