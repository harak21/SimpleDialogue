using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Editor.Conditions.ConditionLoader
{
    internal class JsonGlobalValuesSaveLoadProvider : IGlobalValuesSaveLoadProvider
    {
        public List<ConditionValue> Load()
        {
            var dataPath = Path.Combine(Application.dataPath, "SimpleUtils", "SimpleDialogue", "Resources", "GlobalValues.json");
            using var sr = new StreamReader(dataPath);
            return JsonConvert.DeserializeObject<List<ConditionValue>>(sr.ReadToEnd());
        }

        public void Save(List<ConditionValue> conditionNodes)
        {
            var dataPath = Path.Combine(Application.dataPath, "SimpleUtils", "SimpleDialogue", "Resources", "GlobalValues.json");
            using var sw = new StreamWriter(dataPath);
            sw.Write(JsonConvert.SerializeObject(conditionNodes, Formatting.Indented));
        }
    }
}