using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SimpleUtils.SimpleDialogue.Editor.Conditions
{
    [CreateAssetMenu(fileName = "GlobalValuesContainer", menuName = "SimpleFlow/GlobalValuesContainer")]
    internal class GlobalValues : ScriptableObject, IConditionValuesProvider
    {
        [FormerlySerializedAs("conditionNodes")] [SerializeField] private SimpleSerializedDictionary<ConditionValue> conditionValues = new();

        public Dictionary<int, ConditionValue> ConditionValues => conditionValues;

        public void AddNewConditionValue(ConditionValue conditionValue)
        {
            Undo.RecordObject(this, "Adding new condition values");
            conditionValues.Add(conditionValue);
            EditorUtility.SetDirty(this);
        }
    }
}