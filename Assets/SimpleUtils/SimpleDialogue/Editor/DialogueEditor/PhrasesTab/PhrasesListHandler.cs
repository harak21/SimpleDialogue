using System;
using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Localization;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.PhrasesTab
{
    internal class PhrasesListHandler
    {
        public event Action<long, Vector2, string> OnNodeCreate;
        
        private readonly VisualTreeAsset _itemViewTemplate;
        private readonly ListView _listView;
        private readonly IEditorLocalization _editorLocalization;
        private readonly List<EntryData> _currentPhrases = new();
        private readonly ActorData _actorData;

        public PhrasesListHandler(VisualTreeAsset itemViewTemplate, ListView listView, IEditorLocalization editorLocalization,
            ActorData actorData, string dataKey)
        {
            _actorData = actorData;
            _itemViewTemplate = itemViewTemplate;
            _listView = listView;
            _editorLocalization = editorLocalization;
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
            foreach (var table in _actorData.tables)
            {
                foreach (var tableEntry in _editorLocalization.GetTableEntriesId(table))
                {
                    _currentPhrases.Add(new EntryData()
                    {
                        TableEntryId = tableEntry,
                        TableID = table
                    });
                }
            }
        }

        void BindItem(VisualElement e, int i)
        {
            //((DraggableListItem)e).Bind(_currentPhrases[i].tableEntryId, _currentPhrases[i].TableID);
            var tableEntryId = _currentPhrases[i].TableEntryId;
            var tableID = _currentPhrases[i].TableID;
            ((DraggableListItem)e).Bind(tableEntryId, 
                tableID, 
                _editorLocalization.GetTableEntryTitle(tableEntryId, tableID));
        }
        
        private VisualElement MakeItem()
        {
            var item = new DraggableListItem(_itemViewTemplate);
            item.OnNodeCreate += NodeCreate; 
            return item;
        }

        private void NodeCreate(long longKey, string guidKey, Vector2 position)
        {
            var phrase = _currentPhrases.Find(p => p.TableEntryId ==longKey);
            if (phrase is null)
                throw new ArgumentException();
            OnNodeCreate?.Invoke(phrase.TableEntryId, position, phrase.TableID);
        }
    }
}