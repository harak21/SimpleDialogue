using System;
using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.ConditionsTab
{
    internal class ConditionValuesListHandler
    {
        public event Action<ConditionValue, Vector2> OnNodeCreate;
        
        private readonly VisualTreeAsset _itemViewTemplate;
        private readonly ListView _listView;
        private readonly IConditionValuesProvider _conditionValuesProvider;
        private string _dataKey;
        private List<ConditionValue> _conditionValues;

        public ConditionValuesListHandler(VisualTreeAsset itemViewTemplate, ListView listView,
            IConditionValuesProvider conditionValuesProvider, string dataKey)
        {
            _listView = listView;
            _conditionValuesProvider = conditionValuesProvider;
            _itemViewTemplate = itemViewTemplate;
            listView.viewDataKey = dataKey;
            LoadData();
        }
        
        public void Show()
        {
            _listView.RemoveFromClassList("phrasesList--hidden");
        }

        public void Hide()
        {
            _listView.AddToClassList("phrasesList--hidden");
        }

        public void UpdateList()
        {
            _listView.itemsSource = _conditionValues = _conditionValuesProvider.ConditionValues.GetValues();
            _listView.RefreshItems();
        }

        private void LoadData()
        {
            _listView.makeItem = MakeItem;
            _listView.bindItem = BindItem;
            _listView.itemsSource = _conditionValues = _conditionValuesProvider.ConditionValues.GetValues();
            _listView.selectionType = SelectionType.Single;
            _listView.reorderable = false;
        }
        
        private VisualElement MakeItem()
        {
            var item = new DraggableListItem(_itemViewTemplate);
            item.OnNodeCreate += NodeCreate; 
            return item;
        }

        private void BindItem(VisualElement e, int i)
        {
            ((DraggableListItem)e).Bind(_conditionValues[i].ID,
                Guid.Empty,
                _conditionValues[i].Description);
        }
        
        private void NodeCreate(long longKey, Guid guidKey, Vector2 position)
        {
            var condition = _conditionValues.Find(n => n.ID == longKey);
            if (condition is null)
                throw new ArgumentException();
            OnNodeCreate?.Invoke(condition, position);
        }
    }
}