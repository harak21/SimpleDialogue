using System;
using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Editor.Conditions;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.MainMenu
{
    internal class GlobalValuesTab : IMainMenuTabView
    {
        public event Action<IMainMenuTabView> OnViewSelected;
        
        private readonly GlobalValues _globalValues;
        private readonly VisualTreeAsset _itemTemplate;
        private ListView _listView;
        private List<ConditionValue> _listViewItemsSource;
        private TemplateContainer _root;


        public GlobalValuesTab(GlobalValues globalValues)
        {
            _globalValues = globalValues;
            _itemTemplate = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("GlobalValueListItemView");
            
            Undo.undoRedoPerformed += UndoRedoPerformed;
        }

        private void UndoRedoPerformed()
        {
            Update();
        }

        public VisualElement GetContentTree(Button tabButton)
        {
            var treeAsset = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("GlobalValuesTabView");
            _root = treeAsset.CloneTree();
            _listView = _root.Q<ListView>();
            
            _listView.viewDataKey = "GlobalValuesListView";
            _listView.makeItem = MakeItem;
            _listView.bindItem = BindItem;
            _listView.itemsSource = _listViewItemsSource = _globalValues.ConditionNodes.GetValues();
            _listView.selectionType = SelectionType.Single;
            
            _listView.reorderable = false;

            tabButton.clicked += () => OnViewSelected?.Invoke(this);

            _root.Q<Button>("add").clicked += () =>
            {
                _globalValues.AddNewConditionValue(new ConditionValue()
                {
                    Description = "new description",
                    ID = Guid.NewGuid().GetHashCode(),
                    Value = 42
                });
                Update();
            };
            
            _root.StretchToParentSize();
            return _root;
        }

        public void Show()
        {
            _root.RemoveFromClassList("hidden");
            _listView.RemoveFromClassList("phrasesList--hidden");
        }

        public void Hide()
        {
            _root.AddToClassList("hidden");
            _listView.AddToClassList("phrasesList--hidden");
        }

        public void Update()
        {
            _listView.itemsSource = _listViewItemsSource = _globalValues.ConditionNodes.GetValues();
            _listView.RefreshItems();
        }

        private VisualElement MakeItem()
        {
            var item = _itemTemplate.CloneTree();
            item.AddToClassList("sapphire-blue-background");
            return item;
        }

        private void BindItem(VisualElement e, int i)
        {
            var description = e.Q<TextField>("description");
            description.SetValueWithoutNotify(_listViewItemsSource[i].Description);
            description.RegisterValueChangedCallback(
                evt => _listViewItemsSource[i].Description = evt.newValue);

            var value = e.Q<IntegerField>("value");
            value.SetValueWithoutNotify(_listViewItemsSource[i].Value);
            value.RegisterValueChangedCallback(evt => _listViewItemsSource[i].Value = evt.newValue);
        }
    }
}