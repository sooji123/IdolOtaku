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
    public class otakuevent_select : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }

        [field: SerializeField] public int select_no { get; internal set; }

        [field: SerializeField] public string key_select { get; internal set; }

        [field: SerializeField] public string rsc_select { get; internal set; }

        [field: SerializeField] public string rsc_result_success { get; internal set; }

        [field: SerializeField] public string rsc_result_fail { get; internal set; }

        public otakuevent_select(string ID, int select_no, string key_select, string rsc_select, string rsc_result_success, string rsc_result_fail)
        {
            this.ID = ID;
            this.select_no = select_no;
            this.key_select = key_select;
            this.rsc_select = rsc_select;
            this.rsc_result_success = rsc_result_success;
            this.rsc_result_fail = rsc_result_fail;
        }
    }
}
