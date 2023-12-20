using SimpleUtils.SimpleDialogue.Runtime.Utils;

namespace SimpleUtils.SimpleDialogue.Runtime.Conditions
{
    public interface IConditionValuesProvider
    {
        public SimpleSerializedDictionary<ConditionValue> ConditionValues { get; }
    }
}