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
    public class test_type : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }

        [field: SerializeField] public int test_no { get; internal set; }

        [field: SerializeField] public string key_test { get; internal set; }

        [field: SerializeField] public string key_loc { get; internal set; }

        [field: SerializeField] public string bad_bubble { get; internal set; }

        public test_type(string ID, int test_no, string key_test, string key_loc, string bad_bubble)
        {
            this.ID = ID;
            this.test_no = test_no;
            this.key_test = key_test;
            this.key_loc = key_loc;
            this.bad_bubble = bad_bubble;
        }
    }
}
