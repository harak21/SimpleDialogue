using System;
using System.Linq;
using SimpleUtils.SimpleDialogue.Editor.Conditions;
using SimpleUtils.SimpleDialogue.Editor.Conditions.ConditionLoader;
using SimpleUtils.SimpleDialogue.Editor.Settings;
using SimpleUtils.SimpleDialogue.Editor.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleUtils.SimpleDialogue.Editor.MainMenu
{
    public class SettingsTab : IMainMenuTabView
    {
        public event Action<IMainMenuTabView> OnViewSelected;
        public event Action OnGlobalValuesChanged; 
        
        private TemplateContainer _root;
        private IGlobalValuesSaveLoadProvider _saveLoadProvider;
        private readonly GlobalValues _globalValues;
        private readonly SimpleDialogueSettings _settings;

        public SettingsTab()
        {
            _globalValues = AssetProvider.FindSingleAsset<GlobalValues>();
            _settings = AssetProvider.FindSingleAsset<SimpleDialogueSettings>();
        }

        public void Show()
        {
            _root.RemoveFromClassList("hidden");
        }

        public void Hide()
        {
            _root.AddToClassList("hidden");
        }

        public void Update()
        {
        }

        public VisualElement GetContentTree(Button tabButton)
        {
            
            var loaderTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type != typeof(IGlobalValuesSaveLoadProvider) 
                               && typeof(IGlobalValuesSaveLoadProvider).IsAssignableFrom(type)).ToList();


            var treeAsset = AssetProvider.LoadAssetAtAssetName<VisualTreeAsset>("SettingsTab");
            _root = treeAsset.CloneTree();

            var loaderField = _root.Q<DropdownField>("loader");
            loaderField.choices = loaderTypes.Select(t => t.Name).ToList();
            
            loaderField.RegisterValueChangedCallback(evt =>
            {
                var loader = loaderTypes.First(t => t.Name == evt.newValue);
                _settings.ConditionLoaderTypeName = loader.Name;
                _saveLoadProvider = (IGlobalValuesSaveLoadProvider)Activator.CreateInstance(loader);
                Debug.Log(loader.Name);
            });
            
            var selectedLoaderIndex = loaderTypes
                .FindIndex(l => l.Name == _settings.ConditionLoaderTypeName);
            if (selectedLoaderIndex != -1)
            {
                loaderField.index = selectedLoaderIndex;
                var type = loaderTypes[selectedLoaderIndex];
                _saveLoadProvider = (IGlobalValuesSaveLoadProvider)Activator.CreateInstance(type); 
            }

            ConstructButtons(tabButton);
            _root.StretchToParentSize();
            return _root;
        }

        private void ConstructButtons(Button tabButton)
        {
            var save = _root.Q<Button>("save");
            save.clicked += () => _saveLoadProvider.Save(_globalValues.ConditionNodes.GetValues());
            
            var load = _root.Q<Button>("load");
            load.clicked += () =>
            {
                var conditionValues = _saveLoadProvider.Load();
                _globalValues.ConditionNodes.Clear();
                foreach (var conditionValue in conditionValues)
                {
                    _globalValues.ConditionNodes.Add(conditionValue);
                }
                OnGlobalValuesChanged?.Invoke();
            };
            tabButton.clicked += () => OnViewSelected?.Invoke(this); 
        }
    }
}