using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleUtils.SimpleDialogue.Runtime.System;
using SimpleUtils.SimpleDialogue.Runtime.System.Conditions;
using UnityEngine;

namespace Samples.SimpleUtils.SimpleDialogue.Scripts.Services.Dialogue
{
    internal class JsonConditionSaveLoadService : IConditionSaveLoadService
    {
        private static string _dataPath;
        private const string Folder = "Saves";
        private const string FileName = "Conditions.json";

        public JsonConditionSaveLoadService()
        {
            var directoryPath = Path.Combine(Application.persistentDataPath, Folder);
            _dataPath = Path.Combine(Application.persistentDataPath, Folder, FileName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (!File.Exists(_dataPath))
            {
                File.Create(_dataPath);
            }
        }
        
        public async Task<Dictionary<int, int>> LoadSavedConditionsStates()
        {
            using var sr = new StreamReader(_dataPath);
            return JsonConvert.DeserializeObject<Dictionary<int,int>>(await sr.ReadToEndAsync());
        }

        public async Task SaveConditionsState(Dictionary<int, int> states)
        {
            await using var sw = new StreamWriter(_dataPath);
            await sw.WriteAsync(JsonConvert.SerializeObject(states, Formatting.Indented));
        }
    }
}