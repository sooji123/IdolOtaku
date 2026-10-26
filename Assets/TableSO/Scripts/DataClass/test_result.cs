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
    public class test_result : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }

        [field: SerializeField] public float success_rate { get; internal set; }

        [field: SerializeField] public int score { get; internal set; }

        public test_result(string ID, float success_rate, int score)
        {
            this.ID = ID;
            this.success_rate = success_rate;
            this.score = score;
        }
    }
}
