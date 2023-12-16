using SimpleUtils.SimpleDialogue.Editor.Utils;
using SimpleUtils.SimpleDialogue.Runtime.Containers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor
{
    internal class DialogueEditorWindow : EditorWindow
    {
        private static DialogueContainer _dialogueContainer;
        private DialogueEditorHandler _graphHandler;

        private static void Open()
        {
            //if (HasOpenInstances<SimpleDialogGraphWindow>())
            //{
            //    GetWindow<SimpleDialogGraphWindow>("SceneFlow").Close();
            //}

            var window = CreateInstance<DialogueEditorWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            var treeAsset = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("SimpleDialogGraphView");
            var rootView = treeAsset.CloneTree();
            rootView.StretchToParentSize();
            rootVisualElement.Add(rootView);
            titleContent = new GUIContent(_dialogueContainer.name);

            _graphHandler = new DialogueEditorHandler(this, rootView, _dialogueContainer);
        }

        private void OnDisable()
        {
            _graphHandler?.Clear();
        }


        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject as DialogueContainer != null)
            {
                _dialogueContainer = Selection.activeObject as DialogueContainer;
                Open();
                return true;
            }

            return false;
        }
    }
}