using System.Collections.Generic;

namespace SimpleUtils.SimpleDialogue.Runtime.Conditions
{
    public interface IConditionValuesProvider
    {
        /// <summary>
        /// provides a dictionary of condition values, where the key is the condition id
        /// </summary>
        public Dictionary<int, ConditionValue> ConditionValues { get; }
    }
}