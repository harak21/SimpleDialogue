using System;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.PhrasesTab
{
    internal class ActorPhrasesTabView : IDialogueEditorTabView
    {
        public event Action<ITabView> OnViewSelected;
        public event Action<SharedTableData.SharedTableEntry, Vector2, string, Actor> OnNodeCreate;

        private string TabTitle { get; }
        public bool IsShowed { get; private set; }

        private readonly TemplateContainer _root;
        private readonly Actor _actor;
        private readonly ActorData _actorData;
        private readonly string _dataKey;
        private PhrasesListHandler _phrasesListHandler;
        private Button _viewButton;
        private Label _tabLabel;


        public ActorPhrasesTabView(TemplateContainer root, Actor actor, ActorData actorData, string dataKey)
        {
            _root = root;
            _actor = actor;
            _actorData = actorData;
            _dataKey = $"SimpleDialogGraph{dataKey}{actor.ActorName}";
            TabTitle = $"{actor.ActorName} phrases";
            CreateActorView();
        }

        public void Show()
        {
            IsShowed = true;
            OnViewSelected?.Invoke(this);
            _viewButton.AddToClassList("menu__button--selected");
            _phrasesListHandler.Show();
            _tabLabel.text = TabTitle;
        }

        public void Hide()
        {
            IsShowed = false;
            _viewButton.RemoveFromClassList("menu__button--selected");
            _phrasesListHandler.Hide();
        }

        public void Update()
        {
            _phrasesListHandler.UpdateList();
        }

        public void Dispose()
        {
            Hide();
            _root.Q<VisualElement>("leftMenu").Remove(_viewButton);
        }

        private void CreateActorView()
        {
            var itemViewTemplate = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("ListItemView");
            var buttonTemplate = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("LeftMenuButton");
            var listTemplate = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("PhrasesList");
            
            var leftMenu = _root.Q<VisualElement>("leftMenu");
            var rightMenu = _root.Q<VisualElement>("rightMenu");

            _tabLabel = rightMenu.Q<Label>("itemsViewTitle");

            var list = listTemplate.CloneTree().Q<ListView>();
            rightMenu.Add(list);

            _phrasesListHandler = new PhrasesListHandler(
                itemViewTemplate,
                list,
                _actorData,
                _dataKey);
            
            _viewButton = buttonTemplate.CloneTree().Q<Button>();
            _viewButton.Q<Label>().text = _actor.ActorName;
            leftMenu.Add(_viewButton);
            _viewButton.clicked += Show;
            
            _phrasesListHandler.Hide();
            _phrasesListHandler.OnNodeCreate += (entry, pos, table) => 
                OnNodeCreate?.Invoke(entry, pos, table, _actor);
        }
    }
}