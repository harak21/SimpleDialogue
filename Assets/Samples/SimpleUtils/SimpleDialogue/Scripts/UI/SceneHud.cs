using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    internal class SceneHud : MonoBehaviour, IGameButtonPlace, IDialogPhraseHolder
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private VisualTreeAsset buttonPrefab;
        public Button ExitButton { get; private set; }

        private ListView _buttonsListView;
        private Label _label;
        
        private Action<int> _currentCallback;
        private List<string> _currentItemSource;


        public void Hide()
        {
            uiDocument.rootVisualElement.AddToClassList("hidden");
        }

        public void Show()
        {
            uiDocument.rootVisualElement.RemoveFromClassList("hidden");
        }
        
        public void SetButtonItems(List<string> labels, Action<int> callback)
        {
            _currentItemSource = labels;
            _currentCallback = callback;
            _buttonsListView.itemsSource = labels;
            _buttonsListView.RefreshItems();
        }

        public void SetLabelText(string text)
        {
            _label.text = text;
        }

        void IDialogPhraseHolder.Clear()
        {
            _buttonsListView.itemsSource = new List<string>();
            _buttonsListView.RefreshItems();
        }

        void IGameButtonPlace.Clear()
        {
            _label.text = string.Empty;
        }

        private void Reset()
        {
            uiDocument = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _buttonsListView = uiDocument.rootVisualElement.Q<ListView>("buttonsPlace");
            ExitButton = uiDocument.rootVisualElement.Q<Button>("exitButton");
            _label = uiDocument.rootVisualElement.Q<Label>("phrase");
            InitButtonPlace();
        }
        
        private void InitButtonPlace()
        {
            _buttonsListView.makeItem = MakeItem;
            _buttonsListView.bindItem = BindItem;
            _buttonsListView.reorderable = false;
        }

        private VisualElement MakeItem()
        {
            var tree = buttonPrefab.CloneTree();
            var button = tree.Q<Button>();
            button.RegisterCallback<ClickEvent, Button>(ButtonClickCallback, button);
            return tree;
        }

        private void BindItem(VisualElement ve, int i)
        {
            var button = ve.Q<Button>();
            button.userData = i;
            button.text = _currentItemSource[i];
        }

        private void ButtonClickCallback(ClickEvent clickEvent, Button button)
        {
            var index = (int)button.userData;
            _currentCallback?.Invoke(index);
        }
    }
}