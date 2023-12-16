using System;
using System.Collections.Generic;
using System.Linq;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using UnityEditor;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.MainMenu
{
    public class DialogsTab : IMainMenuTabView
    {
        public event Action<IMainMenuTabView> OnViewSelected;
        
        private ListView _listView;
        private readonly List<DialogueContainer> _containers;
        private readonly VisualTreeAsset _itemTemplate;
        private TemplateContainer _root;

        public DialogsTab()
        {
            _containers = AssetProvider.FindAllAssets<DialogueContainer>();
            _itemTemplate = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("ListItemView");
        }


        public VisualElement GetContentTree(Button tabButton)
        {
            var treeAsset = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("DialogsTabView");
            _root = treeAsset.CloneTree();
            _listView = _root.Q<ListView>();

            _listView.viewDataKey = "DialogsTabListView";
            _listView.makeItem = MakeItem;
            _listView.bindItem = BindItem;
            _listView.itemsSource = _containers;
            _listView.selectionType = SelectionType.Single;
            
            _listView.reorderable = false;

            _listView.onSelectedIndicesChange += ints =>
            {
                var selectionIndex = ints.First();
                EditorGUIUtility.PingObject(_containers[selectionIndex]);
            };

            tabButton.clicked += () => OnViewSelected?.Invoke(this);
            
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
            var label = e.Q<Label>();
            label.text = _containers[i].name;
        }
    }
}