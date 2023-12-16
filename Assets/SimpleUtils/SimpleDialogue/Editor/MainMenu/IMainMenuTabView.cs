using System;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.MainMenu
{
    public interface IMainMenuTabView : ITabView
    {
        event Action<IMainMenuTabView> OnViewSelected;
        VisualElement GetContentTree(Button tabButton);
    }
}