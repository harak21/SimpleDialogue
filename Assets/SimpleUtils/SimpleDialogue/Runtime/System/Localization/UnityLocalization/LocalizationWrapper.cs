#if USE_UNITY_LOCALIZATION 
using System;
using UnityEngine.Localization.Settings;

namespace SimpleUtils.SimpleDialogue.Runtime.System.Localization.UnityLocalization
{
    public class LocalizationWrapper : ILocalizationWrapper
    {
        private readonly LocalizedStringDatabase _stringDatabase;

        public LocalizationWrapper()
        {
            _stringDatabase = LocalizationSettings.Instance.GetStringDatabase();
        }
        
        public string GetLocalizedString(string tableKey, long entryKey)
        {
            return _stringDatabase.GetLocalizedString(
                new Guid(tableKey), 
                entryKey);
        }
    }
}
#endif