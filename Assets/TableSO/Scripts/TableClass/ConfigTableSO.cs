using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class ConfigTableSO : TableSO.Scripts.CsvTableSO<string, TableData.Config>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "Config"; }
    }
}

