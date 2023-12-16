using System.IO;
using Newtonsoft.Json;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using SimpleUtils.SimpleDialogue.Runtime.Utils;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Editor.Conditions
{
    [CreateAssetMenu(fileName = "GlobalValuesContainer", menuName = "SimpleFlow/GlobalValuesContainer")]
    internal class GlobalValues : ScriptableObject
    {
        [SerializeField] private SimpleSerializedDictionary<ConditionValue> conditionNodes = new();

        public SimpleSerializedDictionary<ConditionValue> ConditionNodes => conditionNodes;

        [ContextMenu("Save as JSON")]
        private void SaveCondition()
        {
            var dataPath = Path.Combine(Application.dataPath, "SimpleUtils", "SimpleDialogue", "Resources",
                "GlobalConditions.json");
            using StreamWriter sw = new StreamWriter(dataPath, false);
            var json = JsonConvert.SerializeObject(conditionNodes.GetValues(), Formatting.Indented);
            sw.Write(json);
            sw.Close();
        }
    }
}