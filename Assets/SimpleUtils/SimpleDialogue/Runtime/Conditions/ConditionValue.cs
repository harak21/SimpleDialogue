using System;
using SimpleUtils.SimpleDialogue.Runtime.Utils;
using UnityEngine;

namespace SimpleUtils.SimpleDialogue.Runtime.Conditions
{
    [Serializable]
    public class ConditionValue : ISerializedDictionaryValue
    {
        [field:SerializeField] public int ID { get; set; }
        [field:SerializeField] public string Description { get; set; }
        [field:SerializeField] public int Value { get; set; }
    }
}