using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.Utils;
using UnityEditor;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Editor.Conditions
{
    [CreateAssetMenu(fileName = "GlobalValuesContainer", menuName = "SimpleFlow/GlobalValuesContainer")]
    internal class GlobalValues : ScriptableObject
    {
        [SerializeField] private SimpleSerializedDictionary<ConditionValue> conditionNodes = new();

        public SimpleSerializedDictionary<ConditionValue> ConditionNodes => conditionNodes;

        public void AddNewConditionValue(ConditionValue conditionValue)
        {
            Undo.RecordObject(this, "adding new condition values");
            conditionNodes.Add(conditionValue);
            EditorUtility.SetDirty(this);
        }
    }
}