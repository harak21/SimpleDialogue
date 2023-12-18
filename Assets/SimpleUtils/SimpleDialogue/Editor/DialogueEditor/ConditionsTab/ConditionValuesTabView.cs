using System;
using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.ConditionsTab
{
    internal class ConditionValuesTabView : IDialogueEditorTabView
    {
        public event Action<ITabView> OnViewSelected;
        public event Action<ConditionValue, Vector2> OnNodeCreate;
        
        public string TabTitle { get;}

        private readonly TemplateContainer _root;
        private readonly IConditionValuesProvider _conditionValuesProvider;
        private readonly string _dataKey;
        private Button _viewButton;
        private ConditionValuesListHandler _conditionValuesListHandler;
        private Label _tabLabel;

        public ConditionValuesTabView(TemplateContainer root, string dataKey, IConditionValuesProvider conditionValuesProvider)
        {
            _root = root;
            _conditionValuesProvider = conditionValuesProvider;
            _dataKey = $"SimpleDialogGraph{dataKey}";
            TabTitle = dataKey;
            CreateView();
        }


        public void Show()
        {
            OnViewSelected?.Invoke(this);
            _viewButton.AddToClassList("menu__button--selected");
            _conditionValuesListHandler.Show();
            _tabLabel.text = TabTitle;
        }

        public void Hide()
        {
            _viewButton.RemoveFromClassList("menu__button--selected");
            _conditionValuesListHandler.Hide();
        }

        public void Update()
        {
            _conditionValuesListHandler.UpdateList();
        }

        private void CreateView()
        {
            var itemViewTemplate = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("ListItemView");
            var buttonTemplate = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("LeftMenuButton");
            var listTemplate = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("PhrasesList");
            
            var leftMenu = _root.Q<VisualElement>("leftMenu");
            var rightMenu = _root.Q<VisualElement>("rightMenu");
            
            _tabLabel = rightMenu.Q<Label>("itemsViewTitle");
            
            var list = listTemplate.CloneTree().Q<ListView>();
            rightMenu.Add(list);

            _conditionValuesListHandler = new ConditionValuesListHandler(itemViewTemplate, list, _conditionValuesProvider, _dataKey);
            
            _viewButton = buttonTemplate.CloneTree().Q<Button>();
            _viewButton.Q<Label>().text = TabTitle.Replace("Conditions", "");
            leftMenu.Add(_viewButton);
            _viewButton.clicked += Show;
            
            _conditionValuesListHandler.Hide();
            _conditionValuesListHandler.OnNodeCreate += (node, pos) => OnNodeCreate?.Invoke(node, pos);
        }
    }
}