using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SimpleUtils.SimpleDialogue.Runtime.DialogueNodes;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Editor.Conditions.ConditionLoader
{
    internal class JsonConditionLoader : IConditionLoader
    {
        public List<DialogueConditionNode> Load()
        {
            var dataPath = Path.Combine(Application.dataPath, "SimpleUtils", "SimpleDialogue", "Resources", ".json");
            using var sr = new StreamReader(dataPath);
            return JsonConvert.DeserializeObject<List<DialogueConditionNode>>(sr.ReadToEnd());
        }
    }
}