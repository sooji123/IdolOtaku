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
    public class otakuevent_type : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }

        [field: SerializeField] public MemberType Filter { get; internal set; }

        [field: SerializeField] public string rsc_start { get; internal set; }

        [field: SerializeField] public string key_start { get; internal set; }

        public otakuevent_type(string ID, MemberType Filter, string rsc_start, string key_start)
        {
            this.ID = ID;
            this.Filter = Filter;
            this.rsc_start = rsc_start;
            this.key_start = key_start;
        }
    }
}
