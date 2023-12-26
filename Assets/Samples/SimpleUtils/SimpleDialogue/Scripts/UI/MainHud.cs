using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class MainHud : MonoBehaviour
    {
        public event Action<int> OnLoadScene;
        
        [SerializeField] private UIDocument uiDocument;
        private readonly List<Button> _sceneLoadButtons = new();

        private void Reset()
        {
            uiDocument = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            uiDocument.rootVisualElement.Query<Button>("sceneLoadButton").ForEach(btn =>
            {
                btn.RegisterCallback<ClickEvent, Button>(LoadScene, btn);
                _sceneLoadButtons.Add(btn);
            });
        }

        private void OnDisable()
        {
            foreach (var sceneLoadButton in _sceneLoadButtons)
            {
                sceneLoadButton.UnregisterCallback<ClickEvent, Button>(LoadScene);
            }
            _sceneLoadButtons.Clear();
        }

        private void LoadScene(ClickEvent clickEvent, Button button)
        {
            var sceneIndex = _sceneLoadButtons.IndexOf(button);
            Debug.Log(sceneIndex);
            OnLoadScene?.Invoke(sceneIndex);
        }
    }
}