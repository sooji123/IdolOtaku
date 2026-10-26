using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class test_resultTableSO : TableSO.Scripts.CsvTableSO<string, TableData.test_result>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "test_result"; }
    }
}
