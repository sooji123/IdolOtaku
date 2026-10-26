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
    public class test_qte : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }

        [field: SerializeField] public int bg_width { get; internal set; }

        [field: SerializeField] public int goal_width { get; internal set; }

        [field: SerializeField] public int goal_start_pos { get; internal set; }

        [field: SerializeField] public int goal_end_pos { get; internal set; }

        public test_qte(string ID, int bg_width, int goal_width, int goal_start_pos, int goal_end_pos)
        {
            this.ID = ID;
            this.bg_width = bg_width;
            this.goal_width = goal_width;
            this.goal_start_pos = goal_start_pos;
            this.goal_end_pos = goal_end_pos;
        }
    }
}
