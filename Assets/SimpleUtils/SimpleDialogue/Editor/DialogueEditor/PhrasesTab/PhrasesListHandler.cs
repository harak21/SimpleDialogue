using System;
using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.PhrasesTab
{
    internal class PhrasesListHandler
    {
        public event Action<SharedTableData.SharedTableEntry, Vector2, Guid> OnNodeCreate;
        
        private readonly VisualTreeAsset _itemViewTemplate;
        private readonly ListView _listView;
        private readonly List<EntryData> _currentPhrases = new();
        private readonly ActorData _actorData;

        public PhrasesListHandler(VisualTreeAsset itemViewTemplate, ListView listView,
            ActorData actorData, string dataKey)
        {
            _actorData = actorData;
            _itemViewTemplate = itemViewTemplate;
            _listView = listView;
            listView.viewDataKey = dataKey;
            LoadPhrases();
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
            CreatePhraseCollection();
            _listView.RefreshItems();
        }

        private void LoadPhrases()
        {
            CreatePhraseCollection();

            _listView.makeItem = MakeItem;
            _listView.bindItem = BindItem;
            _listView.itemsSource = _currentPhrases;
            _listView.selectionType = SelectionType.Single;
            _listView.reorderable = false;
        }

        private void CreatePhraseCollection()
        {
            _currentPhrases.Clear();
            foreach (var sharedTable in _actorData.tables)
            {
                foreach (var tableEntry in sharedTable.Entries)
                {
                    _currentPhrases.Add(new EntryData()
                    {
                        SharedTableEntry = tableEntry,
                        TableKey = sharedTable.TableCollectionNameGuid
                    });
                }
            }
        }

        void BindItem(VisualElement e, int i)
        {
            //((DraggableListItem)e).Bind(_currentPhrases[i].SharedTableEntry, _currentPhrases[i].TableKey);
            ((DraggableListItem)e).Bind(_currentPhrases[i].SharedTableEntry.Id, 
                _currentPhrases[i].TableKey, 
                _currentPhrases[i].SharedTableEntry.Key);
        }
        
        private VisualElement MakeItem()
        {
            var item = new DraggableListItem(_itemViewTemplate);
            item.OnNodeCreate += NodeCreate; 
            return item;
        }

        private void NodeCreate(long longKey, Guid guidKey, Vector2 position)
        {
            var phrase = _currentPhrases.Find(p => p.SharedTableEntry.Id ==longKey);
            if (phrase is null)
                throw new ArgumentException();
            OnNodeCreate?.Invoke(phrase.SharedTableEntry, position, phrase.TableKey);
        }
    }
}