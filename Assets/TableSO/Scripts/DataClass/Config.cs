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
    public class Config : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }

        [field: SerializeField] public double Value { get; internal set; }

        [field: SerializeField] public string Desc { get; internal set; }

        public Config(string ID, double Value, string Desc)
        {
            this.ID = ID;
            this.Value = Value;
            this.Desc = Desc;
        }
    }
}
