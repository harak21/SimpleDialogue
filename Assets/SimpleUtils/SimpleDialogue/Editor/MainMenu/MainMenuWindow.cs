using SimpleUtils.SimpleDialogue.Editor.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.MainMenu
{
    internal class MainMenuWindow : EditorWindow
    {
        [MenuItem("Tools/Simple utils/Simple dialogue")]
        public static void ShowWindow()
        {
            var window = GetWindow<MainMenuWindow>();
            window.titleContent = new GUIContent("Simple dialogue");
            window.minSize = new Vector2(500, 400);
            window.Show();
        }

        private void CreateGUI()
        {
            var root = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("SimpleDialogueMainWindow");
            var rootView = root.CloneTree();
            rootView.StretchToParentSize();
            rootVisualElement.Add(rootView);

            var mainMenuHandler = new MainMenuHandler(rootView);
        }
    }
}