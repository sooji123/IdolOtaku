using System;
using System.Collections.Generic;
using UnityEngine;
using TableSO.Scripts;

/// <summary>
/// Made by TableSO CsvTableGenerator
/// </summary>

namespace TableData
{
    [System.Serializable]
    public class Localization : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }

        [field: SerializeField] public string ko { get; internal set; }

        [field: SerializeField] public string en { get; internal set; }

        [field: SerializeField] public string ja { get; internal set; }

        [field: SerializeField] public string zh { get; internal set; }

        public Localization(string ID, string ko, string en, string ja, string zh)
        {
            this.ID = ID;
            this.ko = ko;
            this.en = en;
            this.ja = ja;
            this.zh = zh;
        }
    }
}
