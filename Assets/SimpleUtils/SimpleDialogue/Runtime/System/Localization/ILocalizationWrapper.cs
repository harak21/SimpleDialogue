namespace SimpleUtils.SimpleDialogue.Runtime.System.Localization
{
    public interface ILocalizationWrapper
    {
        string GetLocalizedString(string tableKey, long entryKey);
    }
}