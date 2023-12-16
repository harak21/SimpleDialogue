using System;
using System.Collections.Generic;
using System.Linq;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEditor.Localization;
using UnityEditor.UIElements;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.ActorsSettings
{
    internal class ActorsSettingsTab : IDialogueEditorTabView
    {
        public event Action<Actor, ActorData> OnNewActorViewCreate;
        public event Action OnTabViewsChanged;
        public event Action<ITabView> OnViewSelected;

        private readonly TemplateContainer _root;
        private readonly DialogueContainer _dialogueContainer;
        private readonly string _dataKey;
        private readonly VisualElement _actorsMenu;
        private bool _isActorMenuShowed;
        private ListView _actorList;

        public ActorsSettingsTab(TemplateContainer root, DialogueContainer dialogueContainer, string dataKey)
        {
            _root = root;
            _dialogueContainer = dialogueContainer;
            _dataKey = dataKey;
            _actorsMenu = root.Q<VisualElement>("actorsMenu");
            _actorsMenu.AddToClassList("actorsMenu--hidden");
            
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
        }
        
        public void  LoadActorsData()
        {
            foreach (var actor in _dialogueContainer.Actors.GetValues())
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
                _actorList.itemsSource = dialogueContainer.Actors.GetValues();
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
            _actorList.itemsSource = dialogueContainer.Actors.GetValues();
            _actorList.selectionType = SelectionType.None;
            _actorList.reorderable = false;
        }
        
        void BindItem(VisualElement e, int i)
        {
            var stringTableCollection = LocalizationEditorSettings.GetStringTableCollections();
            var textField = (e).Q<TextField>();
            textField.SetValueWithoutNotify(_dialogueContainer.ActorsList[i].ActorName);
            textField.RegisterValueChangedCallback(evt =>
            {
                _dialogueContainer.ActorsList[i].ActorName = evt.newValue;
            });
            var mask = e.Q<MaskField>();
            var choices = stringTableCollection.Select(t => t.SharedData.TableCollectionName).ToList();
            mask.choices = choices;
            mask.SetValueWithoutNotify(StringsToIntMask(choices, 
                _dialogueContainer.ActorsData[i].tables.Select(t => t.TableCollectionName).ToList()));
            mask.RegisterValueChangedCallback(evt =>
            {
                var tables = IntMaskToListSharedData(
                    evt.newValue, 
                    stringTableCollection.Select(t=> t.SharedData).ToList());
                
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

        private List<SharedTableData> IntMaskToListSharedData(int value, List<SharedTableData> baseList)
        {
            var list = new List<SharedTableData>();
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
                    list.Add(baseList[i]);
                }
            }

            return list;
        }
    }
}