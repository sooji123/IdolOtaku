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
    public class test_progress : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }

        [field: SerializeField] public float progress { get; internal set; }

        public test_progress(string ID, float progress)
        {
            this.ID = ID;
            this.progress = progress;
        }
    }
}
