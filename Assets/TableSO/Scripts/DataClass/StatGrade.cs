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
    public class StatGrade : IIdentifiable<int>
    {
        [field: SerializeField] public int ID { get; internal set; }

        [field: SerializeField] public StatusType Stat { get; internal set; }

        [field: SerializeField] public string Grade { get; internal set; }

        [field: SerializeField] public int Start_Value { get; internal set; }

        [field: SerializeField] public int End_Value { get; internal set; }

        public StatGrade(int ID, StatusType Stat, string Grade, int Start_Value, int End_Value)
        {
            this.ID = ID;
            this.Stat = Stat;
            this.Grade = Grade;
            this.Start_Value = Start_Value;
            this.End_Value = End_Value;
        }
    }
}
