using System;
using System.Collections.Generic;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Localization
{
    public class DummyEditorLocalization : IEditorLocalization
    {
        private const string Message = "unity localization not found. "
                                       + "Install the localization package or provide your own IEditorLocalization implementation";

        public string GetTableId(string tableName)
        {
            throw new InvalidOperationException(Message);
        }

        public List<string> GetTablesName()
        {
            throw new InvalidOperationException(Message);
        }

        public List<string> GetTablesName(List<string> tablesID)
        {
            throw new InvalidOperationException(Message);
        }

        public List<long> GetTableEntriesId(string tableID)
        {
            throw new InvalidOperationException(Message);
        }

        public string GetTableEntryTitle(long entryID, string tableID)
        {
            throw new InvalidOperationException(Message);
        }
    }
}