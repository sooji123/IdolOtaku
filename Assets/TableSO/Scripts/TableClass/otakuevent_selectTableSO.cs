using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class otakuevent_selectTableSO : TableSO.Scripts.CsvTableSO<string, TableData.otakuevent_select>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "otakuevent_select"; }
    }
}
