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
    public class calendar : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }

        [field: SerializeField] public EventType event_type { get; internal set; }

        [field: SerializeField] public int year { get; internal set; }

        [field: SerializeField] public int month { get; internal set; }

        [field: SerializeField] public int day { get; internal set; }

        [field: SerializeField] public string week { get; internal set; }

        public calendar(string ID, EventType event_type, int year, int month, int day, string week)
        {
            this.ID = ID;
            this.event_type = event_type;
            this.year = year;
            this.month = month;
            this.day = day;
            this.week = week;
        }
    }
}
