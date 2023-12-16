using System;
using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.ConditionsTab
{
    internal class ConditionValuesListHandler
    {
        public event Action<ConditionValue, Vector2> OnNodeCreate;
        
        private readonly VisualTreeAsset _itemViewTemplate;
        private readonly ListView _listView;
        private readonly List<ConditionValue> _conditionNodes;
        private string _dataKey;

        public ConditionValuesListHandler(VisualTreeAsset itemViewTemplate, ListView listView,
            List<ConditionValue> conditionNodes, string dataKey)
        {
            _listView = listView;
            _itemViewTemplate = itemViewTemplate;
            listView.viewDataKey = dataKey;
            _conditionNodes = conditionNodes;
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
            _listView.RefreshItems();
        }

        private void LoadData()
        {
            _listView.makeItem = MakeItem;
            _listView.bindItem = BindItem;
            _listView.itemsSource = _conditionNodes;
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
            ((DraggableListItem)e).Bind(_conditionNodes[i].ID,
                string.Empty,
                _conditionNodes[i].Description);
        }
        
        private void NodeCreate(long longKey, string stringKey, Vector2 position)
        {
            var condition = _conditionNodes.Find(n => n.ID == longKey);
            if (condition is null)
                throw new ArgumentException();
            OnNodeCreate?.Invoke(condition, position);
        }
    }
}