using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    [CreateAssetMenu(fileName = "test_qteTableSO", menuName = "TableSO/test_qte")]
    public class test_qteTableSO : TableSO.Scripts.CsvTableSO<string, TableData.test_qte>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "test_qte"; }
    }
}
