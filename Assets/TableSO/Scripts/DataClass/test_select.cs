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
    public class test_select : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }

        [field: SerializeField] public int select_no { get; internal set; }

        [field: SerializeField] public string key_select { get; internal set; }

        [field: SerializeField] public string key_loc { get; internal set; }

        public test_select(string ID, int select_no, string key_select, string key_loc)
        {
            this.ID = ID;
            this.select_no = select_no;
            this.key_select = key_select;
            this.key_loc = key_loc;
        }
    }
}
