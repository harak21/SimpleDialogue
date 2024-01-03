using System.Collections.Generic;
using Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario;
using Samples.SimpleUtils.SimpleDialogue.Scripts.UI;
using SimpleUtils.SimpleDialogue.Runtime.Components;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Dialogue
{
    internal interface IDialogueSystemWrapper : IService
    {
        void InitNewScene(List<DialogueStarter> dialogueStarters,
            List<DialogueEventObserver> dialogueEventObservers,
            List<DialogueConditionModifier> conditionModifiers,
            IGameButtonPlace gameButtonPlace,
            IDialogPhraseHolder dialogPhraseHolder);

        void SaveConditions();
    }
}