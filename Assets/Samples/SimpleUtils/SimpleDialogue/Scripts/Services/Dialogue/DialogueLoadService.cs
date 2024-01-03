using System.Threading.Tasks;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Constants;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Services.ResourcesLoader;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using SimpleUtils.SimpleDialogue.Runtime.System;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Dialogue
{
    internal class DialogueLoadService : ILoadDialogueService
    {
        private readonly ILoadResources _loadResources;
        private DialogueContainersData _dialogueContainersData;

        public DialogueLoadService(ILoadResources loadResources)
        {
            _loadResources = loadResources;
            LoadContainersData();
        }

        private async void LoadContainersData()
        {
            _dialogueContainersData = 
                await _loadResources.LoadAsync<DialogueContainersData>(AddressableConstants.DialogueContainersData);
        }
        
        public Task<IDialogueContainer> Load(int id)
        {
            return Task.FromResult<IDialogueContainer>(_dialogueContainersData[id]);
        }
    }
}