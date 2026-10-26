using UnityEngine;
using TableData;
using TableSO.Scripts;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using static UnityEngine.Analytics.IAnalytic;

namespace Table
{
    public class DialogueStringTableSO : TableSO.Scripts.CsvTableSO<string, TableData.DialogueString>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "DialogueString"; }

        private Dictionary<string, string> dialogueStrings;

        public string GetString(string key)
        {
            if (!dialogueStrings.TryGetValue(key, out string value))
            {
                Debug.LogWarning($"[DialogueStringTableSO] Key not found: {key}");
                return key;
            }

            return value;
        }

        public void SetLanguage(LanguageType languageType)
        {
            UpdateDataByLanguageType(languageType);
        }

        private void UpdateDataByLanguageType(LanguageType languageType)
        {
            string GetString(DialogueString dialogueString)
            {
                switch (languageType)
                {
                    case LanguageType.ko: return dialogueString.ko.Replace("\"", "");
                    case LanguageType.en: return dialogueString.en.Replace("\"", "");
                    case LanguageType.ja: return dialogueString.ja.Replace("\"", "");
                    case LanguageType.zh: return dialogueString.zh.Replace("\"", "");
                }

                return "";
            }

            dialogueStrings = new();
            foreach (var data in dataList)
                dialogueStrings.Add(data.ID, GetString(data));
        }
    }
    
}
