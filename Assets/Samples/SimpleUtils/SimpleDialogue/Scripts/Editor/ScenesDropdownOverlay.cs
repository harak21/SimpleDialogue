using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Editor
{
    [Overlay(typeof(SceneView), ID, DisplayName)]
    internal class ScenesDropdownOverlay : Overlay
    {
        private const string ID = "Scenes";
        private const string DisplayName = "Scenes";

        private readonly List<string> _fullNames = new();
        private readonly List<string> _displayNames = new();
        private int _currentIndex;
        private static DropdownField _scenesDropdown;

        public override VisualElement CreatePanelContent()
        {
            GetBuildScenesInfo();
            ConstructDropdown();

            EditorSceneManager.sceneOpened += (scene, mode) =>
            {
                var sceneName = _scenesDropdown?.choices.Find(s => s.Equals(scene.name));
                _scenesDropdown?.SetValueWithoutNotify(sceneName);
            };

            SceneManager.sceneLoaded += (scene, mode) =>
            {
                var sceneName = _scenesDropdown?.choices.Find(s => s.Equals(scene.name));
                _scenesDropdown?.SetValueWithoutNotify(sceneName);
            };

            EditorApplication.playModeStateChanged += change =>
            {
                if (change != PlayModeStateChange.EnteredEditMode) 
                    return;
                
                var sceneName =
                    _scenesDropdown?.choices.Find(s => s.Equals(EditorSceneManager.GetActiveScene().name));
                _scenesDropdown?.SetValueWithoutNotify(sceneName);
            };
            
            EditorBuildSettings.sceneListChanged += () =>
            {
                GetBuildScenesInfo(); 

                if (_currentIndex != -1)
                {
                    _scenesDropdown?.SetValueWithoutNotify(_displayNames[_currentIndex]);
                }
            };
            
            return _scenesDropdown;
        }

        private void GetBuildScenesInfo()
        {
            _fullNames.Clear();
            _displayNames.Clear();
            _currentIndex = -1;
            var activeScene = EditorSceneManager.GetActiveScene().path;
            for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                _fullNames.Add(SceneUtility.GetScenePathByBuildIndex(i));
                _displayNames.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
                if (activeScene.Equals(_fullNames[i]))
                {
                    _currentIndex = i;
                }
            }
        }


        private void ConstructDropdown()
        {
            _scenesDropdown = new DropdownField()
            {
                label = String.Empty,
                index = _currentIndex,
                choices = _displayNames
            };
            RegisterValueChangedCallback();
        }

        private void RegisterValueChangedCallback()
        {
            _scenesDropdown.RegisterValueChangedCallback(evt =>
            {
                if (!EditorUtility.DisplayDialog("Warning!",
                        "All unsaved changes to the scene will be lost!", "Ok", "Cancel"))
                {
                    _scenesDropdown.SetValueWithoutNotify(evt.previousValue);
                    return;
                }

                _currentIndex = _displayNames.FindIndex(n => n.Equals(evt.newValue));
                EditorSceneManager.OpenScene(_fullNames[_currentIndex]);
            });
        }

        [DidReloadScripts]
        private static void ScriptReloaded()
        {
            var sceneName =
                _scenesDropdown?.choices.Find(s => s.Equals(EditorSceneManager.GetActiveScene().name));
            _scenesDropdown?.SetValueWithoutNotify(sceneName);
        }
    }
}