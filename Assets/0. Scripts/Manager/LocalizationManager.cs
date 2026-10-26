using System;
using Sirenix.OdinInspector;
using Table;
using TableSO.Scripts;
using UnityEngine;

public class LocalizationManager : Singleton<LocalizationManager>
{
    [SerializeField] private TableCenter tableCenter;
    private LocalizationTableSO localizationTable;

    private LanguageType languageType = LanguageType._;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Initialize()
    {
        localizationTable = tableCenter.GetTable<LocalizationTableSO>();
        SetLaguage(LanguageType.ko);
    }

    [Button]
    public void SetLaguage(LanguageType language)
    {
        if (language == languageType) return;
        
        languageType = language;
        localizationTable.SetLanguage(language);
    }

    public string GetString(string key) => localizationTable.GetData(key);
}

public enum LanguageType
{
    _,      // 더미
    ko,     // 한국어
    en,     // 영어
    ja,     // 일본어
    zh      // 중국어
}