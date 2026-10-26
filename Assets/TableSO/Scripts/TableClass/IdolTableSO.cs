using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class IdolTableSO : TableSO.Scripts.CsvTableSO<int, TableData.Idol>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "Idol"; }
    }
}
