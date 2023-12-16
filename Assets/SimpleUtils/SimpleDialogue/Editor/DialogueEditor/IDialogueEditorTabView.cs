using System;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor
{
    public interface IDialogueEditorTabView : ITabView
    {
        event Action<ITabView> OnViewSelected;
    }
}