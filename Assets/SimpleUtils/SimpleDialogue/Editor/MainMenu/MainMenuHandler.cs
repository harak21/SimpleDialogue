using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Editor.Conditions;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.MainMenu
{
    internal class MainMenuHandler
    {
        private readonly TemplateContainer _root;
        private readonly List<IMainMenuTabView> _tabViews = new();

        public MainMenuHandler(TemplateContainer root)
        {
            _root = root;
            CreateGlobalValuesTab();
            CreateDialogsTab();

            foreach (var tabView in _tabViews)
            {
                tabView.Hide();
            }
            
            _tabViews[0].Show();
        }

        private void CreateGlobalValuesTab()
        {
            var globalValues = AssetProvider.FindSingleAsset<GlobalValues>();
            var globalValuesTab = new GlobalValuesTab(globalValues);
            var tabButton = _root.Q<Button>("globalValues");
            globalValuesTab.OnViewSelected += TabSelected;
            _root.Q<VisualElement>("content").Add(globalValuesTab.GetContentTree(tabButton));
            _tabViews.Add(globalValuesTab);
        }

        private void CreateDialogsTab()
        {
            var dialogsTab = new DialogsTab();
            dialogsTab.OnViewSelected += TabSelected;
            var tabButton = _root.Q<Button>("dialogs");
            _root.Q<VisualElement>("content").Add(dialogsTab.GetContentTree(tabButton));
            _tabViews.Add(dialogsTab);
        }

        private void TabSelected(IMainMenuTabView tabView)
        {
            foreach (var view in _tabViews)
            {
                view.Hide();
            }
            tabView.Show();
        }
    }
}