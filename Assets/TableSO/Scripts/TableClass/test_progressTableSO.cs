using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    [CreateAssetMenu(fileName = "test_progressTableSO", menuName = "TableSO/test_progressTableSO")]
    public class test_progressTableSO : TableSO.Scripts.CsvTableSO<string, TableData.test_progress>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "test_progress"; }
    }
}
