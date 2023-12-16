using System.Threading.Tasks;
using SimpleUtils.SimpleDialogue.Runtime.Containers;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public interface ILoadDataService
    {
        public Task<IDialogueContainer> Load(int id);
    }
}