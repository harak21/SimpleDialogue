using Samples.SimpleUtils.SimpleDialogue.Scripts.Scenario;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Scene
{
    internal interface ISceneResourceHandler : IService
    {
        public InteractiveComponentsProvider InteractiveComponentsProvider { get; set; }
        DialogStartersProvider DialogStartersProvider { get; set; }
        DialogEventObserversProvider DialogEventObserversProvider { get; set; }
        DialogueConditionModifiersProvider DialogueConditionModifiersProvider { get; set; }
    }

    internal class SceneResourceHandler : ISceneResourceHandler
    {
        public InteractiveComponentsProvider InteractiveComponentsProvider { get; set; }
        public DialogStartersProvider DialogStartersProvider { get; set; }
        public DialogEventObserversProvider DialogEventObserversProvider { get; set; }
        public DialogueConditionModifiersProvider DialogueConditionModifiersProvider { get; set; }
    }
}