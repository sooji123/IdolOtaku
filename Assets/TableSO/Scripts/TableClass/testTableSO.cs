using UnityEngine;
using TableData;
using System.Collections.Generic;
using System.Linq;
using System;
using TableSO.Scripts;
using System.Threading.Tasks;

/// <summary>
/// Custom Table - Made by TableSO CustomTableGenerator
/// Key Type: string
/// Referenced Tables: test_resultTableSO, test_selectTableSO, test_typeTableSO
/// </summary>

namespace Table
{
    public class testTableSO : TableSO.Scripts.CustomTableSO<string, TableData.test>
    {
        public override TableType tableType => TableType.Custom;

        public string fileName => "testTableSO";
        [SerializeField] private test_resultTableSO test_resultTable;
        [SerializeField] private test_selectTableSO test_selectTable;
        [SerializeField] private test_typeTableSO test_typeTable;
        [SerializeField] private test_qteTableSO test_qteTable;
        [SerializeField] private test_progressTableSO test_progressTable;

        public override List<Type> refTableTypes { get; set; } = new List<Type>()
        {
            typeof(test_resultTableSO),
            typeof(test_selectTableSO),
            typeof(test_typeTableSO),
            typeof(test_qteTableSO),
            typeof(test_progressTableSO),
        };

        public override async Task UpdateData()
        {
            ReleaseData();

            var types = InitType();         // Dictionary<testID, List<TestSequence>>
            var selections = InitSelect();  // Dictionary<key_test, List<TestSelection>>
            var results = InitResult();     // Dictionary<key_select, TestResult>
            var qtes = InitQte();
            var progress = InitProgress();

            // 1. test 객체 생성 및 sequences 할당
            foreach (var kvp in types)
            {
                var testData = new test(kvp.Key);

                if(progress.TryGetValue(kvp.Key, out float progressValue))
                {
                    testData.progress = progressValue;
                }

                testData.SetSequences(kvp.Value);
                dataList.Add(testData);
            }

            // 2. CacheData (dataDict 생성)
            await base.UpdateData();

            // 3. TestSequence에 TestSelection 연결 (key_test로 매칭)
            foreach (var testData in dataList)
            {
                foreach (var sequence in testData.sequences)
                {
                    if (sequence.key_test.Contains("quiz"))
                        sequence.testType = testType.quiz;
                    else if (sequence.key_test.Contains("cheat"))
                        sequence.testType = testType.cheat;
                    else if (sequence.key_test.Contains("qte"))
                        sequence.testType = testType.qte;

                    if (selections.TryGetValue(sequence.key_test, out var selectionList))
                    {
                        sequence.selections = selectionList;
                    }

                    if (qtes.TryGetValue(sequence.key_test, out var qteData))
                    {
                        sequence.qte = qteData;
                    }
                }
            }

            // 4. TestSelection에 TestResult 연결 (key_select로 매칭)
            foreach (var testData in dataList)
            {
                foreach (var sequence in testData.sequences)
                {
                    if (sequence.selections == null) continue;
                    foreach (var selection in sequence.selections)
                    {
                        if (results.TryGetValue(selection.key_select, out var result))
                        {
                            selection.result = result;
                        }
                    }
                }
            }

            // 참조 테이블 해제
            test_resultTable.ReleaseData();
            test_selectTable.ReleaseData();
            test_typeTable.ReleaseData();
            test_qteTable.ReleaseData();
            test_progressTable.ReleaseData();
        }
        
        #region Data Init Logics

        private Dictionary<string, List<TestSequence>> InitType()
        {
            Dictionary<string, List<TestSequence>> result = new Dictionary<string, List<TestSequence>>();

            foreach (var data in test_typeTable.dataList)
            {
                if (!result.ContainsKey(data.ID)) result.Add(data.ID, new List<TestSequence>());

                result[data.ID].Add(new TestSequence(data.ID,
                                                     data.test_no,
                                                     data.key_test,
                                                     data.key_loc,
                                                     data.bad_bubble));
            }

            // test_no로 정렬
            foreach (var key in result.Keys.ToList())
            {
                result[key] = result[key].OrderBy(x => x.test_no).ToList();
            }

            return result;
        }

        private Dictionary<string, List<TestSelection>> InitSelect()
        {
            Dictionary<string, List<TestSelection>> result = new Dictionary<string, List<TestSelection>>();

            foreach (var data in test_selectTable.dataList)
            {
                if (!result.ContainsKey(data.ID)) result.Add(data.ID, new List<TestSelection>());

                result[data.ID].Add(new TestSelection(data.ID,
                    data.select_no,
                    data.key_select,
                    data.key_loc));
            }

            // select_no로 정렬
            foreach (var key in result.Keys.ToList())
            {
                result[key] = result[key].OrderBy(x => x.select_no).ToList();
            }

            return result;
        }

        private Dictionary<string, TestResult> InitResult()
        {
            Dictionary<string, TestResult> result = new Dictionary<string, TestResult>();
            
            foreach (var data in test_resultTable.dataList)
                result.Add(data.ID, new TestResult(data.ID,
                                                   data.success_rate,
                                                   data.score));

            return result;
        }

        private Dictionary<string, Testqte> InitQte()
        {
            Dictionary<string, Testqte> result = new Dictionary<string, Testqte>();

            foreach (var data in test_qteTable.dataList)
            {
                if (!result.ContainsKey(data.ID))
                {
                    result.Add(data.ID, new Testqte(
                        data.ID,
                        data.bg_width,
                        data.goal_width,
                        data.goal_start_pos,
                        data.goal_end_pos));
                }
            }

            return result;
        }

        private Dictionary<string, float> InitProgress()
        {
            Dictionary<string, float> result = new Dictionary<string, float>();

            foreach (var data in test_progressTable.dataList)
            {
                Debug.Log($"[ProgressTable] Key: [{data.ID}], Value: {data.progress}");
                if (!result.ContainsKey(data.ID))
                {
                    result.Add(data.ID, data.progress);
                }
            }

            return result;
        }

        #endregion

        public override TableData.test GetData(string key)
        {
            // TODO: Implement GetData logic
            return base.GetData(key);
        }
    }
}
