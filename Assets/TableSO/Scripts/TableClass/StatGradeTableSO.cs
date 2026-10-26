using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class StatGradeTableSO : TableSO.Scripts.CsvTableSO<int, TableData.StatGrade>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "StatGrade"; }
        
        [ShowInInspector] Dictionary<StatusType, List<StatGrade>> statGrades = new();

        public string GetGrade(StatusType type, int value)
        {
            SetDictionary();
            
            var grades = statGrades[type];
            
            foreach (var grade in grades)
                if (grade.Start_Value <= value)
                    return grade.Grade;
            
            return grades[grades.Count - 1].Grade;
        }

        private void SetDictionary()
        {
            if (statGrades.Count != 0) return;

            foreach (var data in dataList)
            {
                if (!statGrades.ContainsKey(data.Stat))
                    statGrades.Add(data.Stat, new List<StatGrade>());
                
                statGrades[data.Stat].Add(data);
            }

            foreach (var (type, list) in statGrades)
                list.Sort((a, b) => b.Start_Value.CompareTo(a.Start_Value));
        }
    }
}
