using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class test_selectTableSO : TableSO.Scripts.CsvTableSO<string, TableData.test_select>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "test_select"; }
    }
}
