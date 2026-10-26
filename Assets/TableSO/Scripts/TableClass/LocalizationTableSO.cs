using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class LocalizationTableSO : TableSO.Scripts.CsvTableSO<string, TableData.Localization>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "Localization"; }

        [ShowInInspector] private Dictionary<string, string> localizedStrings;
        
        public string GetData(string key)
        {
            Debug.Log($"[LocalizationTableSO] Key   : {key}");
            Debug.Log($"[LocalizationTableSO] Count : {localizedStrings.Count}");
         
            // TODO : 추후 삭제
            if (localizedStrings.Count == 0) UpdateDataByLanguageType(LanguageType.ko);
            
            if (!localizedStrings.TryGetValue(key, out string value))
            {
                Debug.LogWarning($"[LocalizationTableSO] Key not found: {key}");
                return key;
            }

            value.Replace("\"", "");
            
            return value;
        }

        public void SetLanguage(LanguageType languageType)
        {
            UpdateData();
            UpdateDataByLanguageType(languageType);
            ReleaseData();
        }

        private void UpdateDataByLanguageType(LanguageType languageType)
        {
            string GetString(Localization localization)
            {
                switch (languageType)
                {
                    case LanguageType.ko: return localization.ko.Replace("\"", "");
                    case LanguageType.en: return localization.en.Replace("\"", "");
                    case LanguageType.ja: return localization.ja.Replace("\"", "");
                    case LanguageType.zh: return localization.zh.Replace("\"", "");
                }

                return "";
            }

            localizedStrings = new();
            foreach (var data in dataList)
                localizedStrings.Add(data.ID, GetString(data));
        }
    }
}
