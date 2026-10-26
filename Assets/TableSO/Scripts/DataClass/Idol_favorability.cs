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
    public class Idol_favorability : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }

        [field: SerializeField] public int favorability_level { get; internal set; }

        [field: SerializeField] public int Nareq_favorabilityme { get; internal set; }

        public Idol_favorability(string ID, int favorability_level, int Nareq_favorabilityme)
        {
            this.ID = ID;
            this.favorability_level = favorability_level;
            this.Nareq_favorabilityme = Nareq_favorabilityme;
        }
    }
}
