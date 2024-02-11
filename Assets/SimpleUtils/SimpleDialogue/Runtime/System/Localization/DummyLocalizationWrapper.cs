using NotImplementedException = System.NotImplementedException;

namespace SimpleUtils.SimpleDialogue.Runtime.System.Localization
{
    public class DummyLocalizationWrapper : ILocalizationWrapper
    {
        public string GetLocalizedString(string tableKey, long entryKey)
        {
            return "unity localization not found. " +
                   "Install the localization package or provide your own ILocalizationWrapper implementation";
        }
    }
}