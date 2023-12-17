using System.Collections.Generic;
using SimpleUtils.SimpleDialogue.Runtime.Conditions;

namespace SimpleUtils.SimpleDialogue.Editor.Conditions.ConditionLoader
{
    public interface IGlobalValuesSaveLoadProvider
    {
        List<ConditionValue> Load();
        void Save(List<ConditionValue> conditionNodes);
    }
}