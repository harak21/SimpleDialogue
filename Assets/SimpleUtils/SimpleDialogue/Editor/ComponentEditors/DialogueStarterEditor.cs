using System.Collections.Generic;
using System.Linq;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Components;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using UnityEditor;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.ComponentEditors
{
    [CustomEditor(typeof(DialogueStarter))]
    public class DialogueStarterEditor : UnityEditor.Editor
    {
        private List<DialogueContainer> _dialogueContainers;

        public override VisualElement CreateInspectorGUI()
        {
            var dialogStarter = (DialogueStarter)target;
            _dialogueContainers = AssetProvider.FindAllAssets<DialogueContainer>();
            var treeAsset = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("DialogStarterView");
            var view = treeAsset.CloneTree();

            var dropdownField = view.Q<DropdownField>("dialogue");
            dropdownField.choices = _dialogueContainers.Select(c => c.name).ToList();
            dropdownField.index = _dialogueContainers.FindIndex(c => c.DialogueID == dialogStarter.DialogueID);
            dropdownField.RegisterValueChangedCallback(evt =>
            {
                var dialogueID = _dialogueContainers.Find(c => c.name == evt.newValue).DialogueID;
                dialogStarter.DialogueID = dialogueID;
            });

            var root = view.Q<VisualElement>("root");
            root.styleSheets.Add(AssetProvider.LoadAssetAtAssetName<StyleSheet>("GraphViewStyle"));
            return root;
        }
    }
}