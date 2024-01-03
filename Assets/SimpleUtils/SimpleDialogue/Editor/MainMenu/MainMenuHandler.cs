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
        private GlobalValues _globalValues;

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
            _globalValues = AssetProvider.FindSingleAsset<GlobalValues>();
            _globalValuesTab = new GlobalValuesTab(_globalValues);
            var tabButton = _root.Q<Button>("globalValues");
            _globalValuesTab.OnViewSelected += TabSelected;
            _globalValuesTab.OnRecordModifiedObject += RecordModifiedObject;
            _globalValuesTab.OnMakeModifiedObjectDirty += MakeModifiedObjectDirty;
            _root.Q<VisualElement>("content").Add(_globalValuesTab.GetContentTree(tabButton));
            _tabViews.Add(_globalValuesTab);
        }

        private void MakeModifiedObjectDirty()
        {
            EditorUtility.SetDirty(_globalValues);
        }

        private void RecordModifiedObject(string reason)
        {
            Undo.RecordObject(_globalValues, reason);
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