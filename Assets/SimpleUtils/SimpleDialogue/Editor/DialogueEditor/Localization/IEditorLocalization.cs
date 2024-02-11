using System.Collections.Generic;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Localization
{
    public interface IEditorLocalization
    {
        string GetTableId(string tableName);
        List<string> GetTablesName();
        List<string> GetTablesName(List<string> tablesID);
        List<long> GetTableEntriesId(string tableID);
        string GetTableEntryTitle(long entryID, string tableID);
    }
}