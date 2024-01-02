using System.Collections.Generic;
using System.Linq;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Components;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEditor;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.ComponentEditors
{
    [CustomEditor(typeof(DialogueEventObserver))]
    public class DialogueEventObserverEditor : UnityEditor.Editor
    {
        private List<DialogueContainer> _dialogueContainers;
        private DialogueEventObserver _eventObserver;
        private DropdownField _eventDropdown;
        private List<DialogueEventNode> _currentEvents;

        public override VisualElement CreateInspectorGUI() 
        {
            _eventObserver = (DialogueEventObserver)target;
            _dialogueContainers = AssetProvider.FindAllAssets<DialogueContainer>();
            var treeAsset = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("DialogueObserverView");
            var view = treeAsset.CloneTree();

            var currentDialogue = _dialogueContainers.Find(d => d.DialogueID == _eventObserver.DialogueID);
            
            var dialogueDropdown = view.Q<DropdownField>("dialogue");
            ConstructDialogueDropdown(dialogueDropdown, currentDialogue);

            _eventDropdown = view.Q<DropdownField>("event");
            ConstructEventDropdown(currentDialogue);
            _eventDropdown.RegisterValueChangedCallback(EventSelected);

            var root = view.Q<VisualElement>("root");
            root.styleSheets.Add(AssetProvider.LoadAssetAtAssetName<StyleSheet>("GraphViewStyle"));
            return root;
        }

        private void ConstructDialogueDropdown(DropdownField dropdownField, DialogueContainer currentDialogue)
        {
            dropdownField.choices = _dialogueContainers.Select(c => c.name).ToList();
            dropdownField.index = _dialogueContainers.IndexOf(currentDialogue);
            dropdownField.RegisterValueChangedCallback(DialogueSelected);
        }

        private void DialogueSelected(ChangeEvent<string> evt)
        {
            var newDialogue = _dialogueContainers.Find(c => c.name == evt.newValue);
            var dialogueID = newDialogue.DialogueID;
            _eventObserver.DialogueID = dialogueID;
            _eventDropdown.index = -1;
            ConstructEventDropdown(newDialogue);
        }

        private void ConstructEventDropdown(DialogueContainer currentDialogue)
        {
            if (currentDialogue != null)
            {
                _currentEvents = currentDialogue.EventNodes.GetValues();
                var eventNodes = _currentEvents;
                _eventDropdown.choices = eventNodes.Select(e => e.EventName).ToList();
                _eventDropdown.index = eventNodes.FindIndex(e => e.ID == _eventObserver.EventID);
            }
        }

        private void EventSelected(ChangeEvent<string> evt)
        {
            if (_currentEvents == null)
                return;
            
            var newEvent = _currentEvents.Find(e => e.EventName == evt.newValue);
            _eventObserver.EventID = newEvent.ID;
        }
    }
}