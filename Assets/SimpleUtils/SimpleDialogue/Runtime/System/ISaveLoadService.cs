using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public interface ISaveLoadService
    {
        public Task<Dictionary<int, int>> LoadSavedConditionsStates();
        public Task SaveConditionsState(Dictionary<int, int> states);
    }
}