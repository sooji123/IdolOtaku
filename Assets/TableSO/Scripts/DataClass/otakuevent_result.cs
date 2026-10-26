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
    public class otakuevent_result : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }

        [field: SerializeField] public string result_type { get; internal set; }

        [field: SerializeField] public int weight { get; internal set; }

        [field: SerializeField] public int mental { get; internal set; }

        [field: SerializeField] public int fanship { get; internal set; }

        public otakuevent_result(string ID, string result_type, int weight, int mental, int fanship)
        {
            this.ID = ID;
            this.result_type = result_type;
            this.weight = weight;
            this.mental = mental;
            this.fanship = fanship;
        }
    }
}
