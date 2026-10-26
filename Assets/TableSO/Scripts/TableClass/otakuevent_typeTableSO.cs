using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class otakuevent_typeTableSO : TableSO.Scripts.CsvTableSO<string, TableData.otakuevent_type>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "otakuevent_type"; }
    }
}
