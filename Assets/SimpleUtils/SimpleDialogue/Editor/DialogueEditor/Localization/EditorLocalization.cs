#if USE_UNITY_LOCALIZATION
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Localization;

namespace SimpleUtils.SimpleDialogue.Editor.DialogueEditor.Localization
{
    public class EditorLocalization : IEditorLocalization
    {
        public string GetTableId(string tableName)
        {
            return LocalizationEditorSettings.GetStringTableCollection(tableName)
                .SharedData.TableCollectionNameGuid.ToString();
        }

        public List<string> GetTablesName()
        {
            var stringTableCollections = 
                LocalizationEditorSettings.GetStringTableCollections();
            return stringTableCollections.Select(t => t.SharedData.TableCollectionName).ToList();
        }

        public List<string> GetTablesName(List<string> tablesID)
        {
            HashSet<string> tableNames = new HashSet<string>();
            foreach (var guid in tablesID)
            {
                var table = LocalizationEditorSettings.GetStringTableCollection(new Guid(guid));
                tableNames.Add(table.TableCollectionName);
            }

            return tableNames.ToList();
        }

        public List<long> GetTableEntriesId(string tableID)
        {
            List<long> ids = new();
            var table = LocalizationEditorSettings.GetStringTableCollection(new Guid(tableID));
            foreach (var entry in table.SharedData.Entries)
            {
                ids.Add(entry.Id);
            }

            return ids;
        }

        public string GetTableEntryTitle(long entryID, string tableID)
        {
            var stringTableCollection = LocalizationEditorSettings.GetStringTableCollection(new Guid(tableID));
            return stringTableCollection.SharedData.GetEntry(entryID).Key;
            
        }
    }
}
#endif