using System;
using SimpleUtils.SimpleDialogue.Editor.DialogueEditor.PhrasesTab;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Graph
{
    internal class ActorView
    {
        public event Action OnActorViewSelected;
        public Action<SharedTableData.SharedTableEntry, Vector2, Guid, Actor> OnNodeCreate;
        
        private readonly TemplateContainer _root;
        private readonly Actor _actor;
        private readonly ActorData _actorData;
        private readonly string _dataKey;
        private PhrasesListHandler _phrasesListHandler;
        private Button _viewButton;

        public ActorView(TemplateContainer root, Actor actor, ActorData actorData, string dataKey)
        {
            _root = root;
            _actor = actor;
            _actorData = actorData;
            _dataKey = $"SimpleDialogGraph{dataKey}{actor.ActorName}";
            CreateActorView();
        }

        public void Show()
        {
            OnActorViewSelected?.Invoke();
            _viewButton.AddToClassList("menu__button--selected");
            _phrasesListHandler.Show();
        }

        public void Hide()
        {
            _viewButton.RemoveFromClassList("menu__button--selected");
            _phrasesListHandler.Hide();
        }

        public void Update()
        {
            _phrasesListHandler.UpdateList();
        }

        private void CreateActorView()
        {
            var itemViewTemplate = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("ListItemView");
            var buttonTemplate = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("LeftMenuButton");
            var listTemplate = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("PhrasesList");
            
            var leftMenu = _root.Q<VisualElement>("leftMenu");
            var rightMenu = _root.Q<VisualElement>("rightMenu");
            
            var list = listTemplate.CloneTree().Q<ListView>();
            rightMenu.Add(list);

            _phrasesListHandler = new PhrasesListHandler(
                itemViewTemplate,
                list,
                _actorData,
                _dataKey);
            
            _viewButton = buttonTemplate.CloneTree().Q<Button>();
            leftMenu.Add(_viewButton);
            _viewButton.clicked += Show;
            
            _phrasesListHandler.Hide();
            _phrasesListHandler.OnNodeCreate += (entry, pos, table) => OnNodeCreate?.Invoke(entry, pos, table, _actor);
        }
    }
}