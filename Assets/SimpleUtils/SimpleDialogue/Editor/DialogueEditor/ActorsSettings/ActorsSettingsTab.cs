using System;
using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Localization;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.ActorsSettings
{
    internal class ActorsSettingsTab : IDialogueEditorTabView
    {
        public event Action<Actor, ActorData> OnNewActorViewCreate;
        public event Action OnTabViewsChanged;
        public event Action<ITabView> OnViewSelected;
        public event Action<Actor> OnActorNameChanged; 

        private readonly TemplateContainer _root;
        private readonly DialogueContainer _dialogueContainer;
        private readonly string _dataKey;
        private readonly VisualElement _actorsMenu;
        private readonly IEditorLocalization _editorLocalization;
        private bool _isActorMenuShowed;
        private ListView _actorList;

        public ActorsSettingsTab(TemplateContainer root, 
            DialogueContainer dialogueContainer, 
            IEditorLocalization editorLocalization,
            string dataKey)
        {
            _root = root;
            _dialogueContainer = dialogueContainer;
            _dataKey = dataKey;
            _actorsMenu = root.Q<VisualElement>("actorsMenu");
            _actorsMenu.AddToClassList("actorsMenu--hidden");
            _editorLocalization = editorLocalization;
            
            CreateActorList(dialogueContainer);

            BindActorsSettingsButton();

            BindCreateButton(dialogueContainer);
        }

        public void Show()
        {
            _actorsMenu.AddToClassList("actorsMenu--hidden");
            _isActorMenuShowed = false;
        }

        public void Hide()
        {
            _actorsMenu.RemoveFromClassList("actorsMenu--hidden");
            _isActorMenuShowed = true;
        }

        public void Update()
        {
            _actorList.itemsSource = _dialogueContainer.ActorsList;
            _actorList.RefreshItems();
        }
        
        public void  LoadActorsData()
        {
            foreach (var actor in _dialogueContainer.ActorsList)
            {
                var actorData = _dialogueContainer.ActorsData.Find(d => d.actorID == actor.ID);
                OnNewActorViewCreate?.Invoke(actor, actorData);
            }
        }
        
        private void BindCreateButton(DialogueContainer dialogueContainer)
        {
            _actorsMenu.Q<Button>("actorAddButton").clicked += () =>
            {
                var newActor = new Actor(Guid.NewGuid().GetHashCode())
                {
                    ActorName = "NewActorName"
                };
                dialogueContainer.AddActor(newActor);
                var actorData = new ActorData()
                {
                    actorID = newActor.ID
                };
                dialogueContainer.ActorsData.Add(actorData);
                OnNewActorViewCreate?.Invoke(newActor, actorData);
                _actorList.itemsSource = dialogueContainer.ActorsList;
                _actorList.RefreshItems();
            };
        }

        private void BindActorsSettingsButton()
        {
            var actorsMenuButton = _root.Q<Button>("actorsButton");
            actorsMenuButton.clicked += () =>
            {
                if (_isActorMenuShowed)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            };
        }

        private void CreateActorList(DialogueContainer dialogueContainer)
        {
            _actorList = _actorsMenu.Q<ListView>("actorsList");
            _actorList.makeItem = MakeItem;
            _actorList.bindItem = BindItem;
            _actorList.itemsSource = dialogueContainer.ActorsList;
            _actorList.selectionType = SelectionType.None;
            _actorList.reorderable = false;
        }
        
        void BindItem(VisualElement e, int i)
        {
            var textField = (e).Q<TextField>();
            textField.SetValueWithoutNotify(_dialogueContainer.ActorsList[i].ActorName);
            textField.RegisterValueChangedCallback(evt =>
            {
                _dialogueContainer.ActorsList[i].ActorName = evt.newValue;
                OnActorNameChanged?.Invoke(_dialogueContainer.ActorsList[i]);
            });
            
            var mask = e.Q<MaskField>();
            var choices = _editorLocalization.GetTablesName();
            mask.choices = choices;
            mask.SetValueWithoutNotify(StringsToIntMask(choices, 
                _editorLocalization.GetTablesName(_dialogueContainer.ActorsData[i].tables)));
            mask.RegisterValueChangedCallback(evt =>
            {
                var tables = IntMaskToListTables(
                    evt.newValue, 
                    choices);
                
                _dialogueContainer.ActorsData[i].tables = tables;
                OnTabViewsChanged?.Invoke();
            });
        }
        
        private VisualElement MakeItem()
        {
            var actorViewTemplate = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("ActorItemView");
            var item = actorViewTemplate.CloneTree();
            item.AddToClassList("actorsList__itemContainer");
            // item.OnNodeCreate += NodeCreate; 
            return item;
        }
        
        private int StringsToIntMask(List<string> baseList, List<string> selectedList)
        {
            int num = 0;
            for (int i = 0; i < baseList.Count; i++)
            {
                if (selectedList.Contains(baseList[i]))
                {
                    num |= 1 << i;
                }
            }

            return num;
        }

        private List<string> IntMaskToListTables(int value, List<string> baseList)
        {
            var list = new List<string>();
            if (value == -1)
            {
                return baseList;
            }

            if (value == 0)
            {
                return list;
            }
            for (int i = 0; i < baseList.Count; i++)
            {
                if ((1 << i & value) != 0)
                {
                    list.Add(_editorLocalization.GetTableId(baseList[i]));
                }
            }

            return list;
        }
    }
}