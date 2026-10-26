using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class test_typeTableSO : TableSO.Scripts.CsvTableSO<string, TableData.test_type>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "test_type"; }
    }
}
