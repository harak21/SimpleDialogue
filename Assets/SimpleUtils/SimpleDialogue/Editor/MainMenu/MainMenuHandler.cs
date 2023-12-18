using System;
using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Editor.Conditions;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using UnityEditor;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.MainMenu
{
    internal class MainMenuHandler
    {
        private readonly TemplateContainer _root;
        private readonly List<IMainMenuTabView> _tabViews = new();
        private GlobalValuesTab _globalValuesTab;

        public MainMenuHandler(TemplateContainer root)
        {
            _root = root;
            CreateGlobalValuesTab();
            CreateDialogsTab();
            CreateSettingsTab();

            foreach (var tabView in _tabViews)
            {
                tabView.Hide();
            }
            
            _tabViews[0].Show();
            
            
            Undo.undoRedoPerformed += UndoRedoPerformed;
        }

        private void UndoRedoPerformed()
        {
            foreach (var tabView in _tabViews)
            {
                tabView.Update();
            }
        }

        private void CreateGlobalValuesTab()
        {
            var globalValues = AssetProvider.FindSingleAsset<GlobalValues>();
            _globalValuesTab = new GlobalValuesTab(globalValues);
            var tabButton = _root.Q<Button>("globalValues");
            _globalValuesTab.OnViewSelected += TabSelected;
            _root.Q<VisualElement>("content").Add(_globalValuesTab.GetContentTree(tabButton));
            _tabViews.Add(_globalValuesTab);
        }

        private void CreateDialogsTab()
        {
            var dialogsTab = new DialogsTab();
            dialogsTab.OnViewSelected += TabSelected;
            var tabButton = _root.Q<Button>("dialogs");
            _root.Q<VisualElement>("content").Add(dialogsTab.GetContentTree(tabButton));
            _tabViews.Add(dialogsTab);
        }

        private void CreateSettingsTab()
        {
            var settingsTab = new SettingsTab();
            settingsTab.OnViewSelected += TabSelected;
            settingsTab.OnGlobalValuesChanged += () => _globalValuesTab.Update();
            var tabButton = _root.Q<Button>("settings");
            _root.Q<VisualElement>("content").Add(settingsTab.GetContentTree(tabButton));
            _tabViews.Add(settingsTab);
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