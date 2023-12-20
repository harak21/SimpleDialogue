using UnityEditor;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Editor.Settings
{
    public class SimpleDialogueSettings : ScriptableObject
    {
        [SerializeField, HideInInspector] private string conditionLoaderTypeName;

        public string ConditionLoaderTypeName
        {
            get => conditionLoaderTypeName;
            set
            {
                Undo.RecordObject(this, "Changed condition values loader");
                conditionLoaderTypeName = value;
                EditorUtility.SetDirty(this);
            }
        }
    }
}