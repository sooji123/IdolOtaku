using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class otakuevent_resultTableSO : TableSO.Scripts.CsvTableSO<string, TableData.otakuevent_result>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "otakuevent_result"; }
    }
}
