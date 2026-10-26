using UnityEngine;
using TableData;
using System.Collections.Generic;
using System.Linq;
using System;
using TableSO.Scripts;
using System.Threading.Tasks;
using JetBrains.Annotations;

/// <summary>
/// Custom Table - Made by TableSO CustomTableGenerator
/// Key Type: string
/// Referenced Tables: otakuevent_resultTableSO, otakuevent_selectTableSO, otakuevent_typeTableSO
/// </summary>

namespace Table
{
    public class otakueventTableSO : TableSO.Scripts.CustomTableSO<string, TableData.otakuevent>
    {
        public override TableType tableType => TableType.Custom;

        public string fileName => "otakueventTableSO";
        [SerializeField] private otakuevent_resultTableSO otakuevent_resultTable;
        [SerializeField] private otakuevent_selectTableSO otakuevent_selectTable;
        [SerializeField] private otakuevent_typeTableSO otakuevent_typeTable;

        public override List<Type> refTableTypes { get; set; } = new List<Type>()
        {
            typeof(otakuevent_resultTableSO),
            typeof(otakuevent_selectTableSO),
            typeof(otakuevent_typeTableSO),
        };
        
        public override async Task UpdateData()
        {
            ReleaseData();

            foreach (var data in otakuevent_typeTable.dataList)
                dataList.Add(new otakuevent(data));
            base.UpdateData();
            
            foreach (var data in otakuevent_selectTable.dataList)
            {
                if (!dataDict.ContainsKey(data.ID)) continue;
                var target = dataDict[data.ID];
                target.AddSelection(data);
            }

            foreach (var data in otakuevent_resultTable.dataList)
            {
                if (!dataDict.ContainsKey(data.ID)) continue;
                var target = dataDict[data.ID];
                target.AddResult(data);
            }
            
            otakuevent_resultTable.ReleaseData();
            otakuevent_selectTable.ReleaseData();
            otakuevent_typeTable.ReleaseData();
        }

        public override TableData.otakuevent GetData(string key)
        {
            // TODO: Implement GetData logic
            return base.GetData(key);
        }
    }
}
