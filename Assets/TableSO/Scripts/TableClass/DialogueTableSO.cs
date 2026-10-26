using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class DialogueTableSO : TableSO.Scripts.CsvTableSO<int, TableData.Dialogue>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "Dialogue"; }
    }
}
