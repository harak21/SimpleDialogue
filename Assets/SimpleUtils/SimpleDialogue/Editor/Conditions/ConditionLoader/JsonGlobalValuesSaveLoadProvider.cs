using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Editor.Conditions.ConditionLoader
{
    internal class JsonGlobalValuesSaveLoadProvider : IGlobalValuesSaveLoadProvider
    {
        private readonly string _dataPath;

        public JsonGlobalValuesSaveLoadProvider()
        {
            _dataPath = Path.Combine(Application.dataPath, "SimpleUtils", "SimpleDialogue", "Resources", "GlobalValues.json");
        }
        
        public List<ConditionValue> Load()
        {
            if (!File.Exists(_dataPath))
            {
                Debug.LogWarning($"file at path \"{_dataPath}\" not found");
                return new List<ConditionValue>();
            }
            using var sr = new StreamReader(_dataPath);
            return JsonConvert.DeserializeObject<List<ConditionValue>>(sr.ReadToEnd());
        }

        public void Save(List<ConditionValue> conditionNodes)
        {
            if (!File.Exists(_dataPath))
            {
                File.Create(_dataPath);
            }
            using var sw = new StreamWriter(_dataPath);
            sw.Write(JsonConvert.SerializeObject(conditionNodes, Formatting.Indented));
        }
    }
}