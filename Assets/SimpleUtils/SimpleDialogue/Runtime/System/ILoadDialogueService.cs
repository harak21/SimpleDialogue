using System.Threading.Tasks;
using SimpleUtils.SimpleDialogue.Runtime.Containers;

namespace SimpleUtils.SimpleDialogue.Runtime.System
{
    public interface ILoadDialogueService
    {
        public Task<IDialogueContainer> Load(int id);
    }
}