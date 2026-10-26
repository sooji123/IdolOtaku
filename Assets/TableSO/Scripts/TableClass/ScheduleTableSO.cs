using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using TableData;
using TableSO.Scripts;

namespace Table
{
    public class ScheduleTableSO : TableSO.Scripts.CsvTableSO<int, TableData.Schedule>
    {
        public override TableType tableType => TableType.Csv;

        public override string csvPath { get => "Schedule"; }

        [ShowInInspector] private List<ScheduleData> scheduleDataList = new();
        [ShowInInspector] private Dictionary<ScheduleType, List<ScheduleData>> scheduleDataDict = new Dictionary<ScheduleType, List<ScheduleData>>();
        
        public override async Task UpdateData()
        {
            await base.UpdateData();
            
            ParseToScheduleList();
            
            ReleaseData();
        }

        private void ParseToScheduleList()
        {
            scheduleDataList.Clear();
            
            foreach (var data in dataList)
                scheduleDataList.Add(ParseToScheduleData(data));
        }

        private ScheduleData ParseToScheduleData(Schedule schedule)
        {
            ScheduleData scheduleData = new ScheduleData()
            {
                key = schedule.ID.ToString(),
                scheduleID = schedule.ID.ToString(),
                scheduleType = schedule.ScheduleType,
                scheduleIcon = schedule.ScheduleIconPath,
                scheduleName = schedule.ScheduleName,
                seasonType = schedule.Season,
                maxStatValue = schedule.MaxStatValue,
                scheduleResults = ParseToScheduleResultData(schedule),
                parttimeLevelData = ParseToParttimeLevelData(schedule),
            };
            
            return scheduleData;
        }

        private List<ScheduleResultData> ParseToScheduleResultData(Schedule schedule)
        {
            List<ScheduleResultData> list = new();

            if (schedule.Results.Length == schedule.Weights.Length &&
                schedule.Weights.Length == schedule.Mental.Length &&
                schedule.Mental.Length == 4)
            {
            }
            else
            {
                Debug.LogError("[ScheduleTableSO] Cannot Parse ScheduleResultData");
                return list;
            }

            (StatusType[], int[]) GetStatusAndValues(ResultType resultType)
            {
                switch (resultType)
                {
                    case ResultType.Perfect: return (schedule.PerfectStats, schedule.PerfectValues);
                    case ResultType.Success: return (schedule.SuccessStats, schedule.SuccessValues);
                    case ResultType.Fail: return (schedule.FailStats, schedule.FailValues);
                    case ResultType.Worst: return (schedule.WorstStats, schedule.WorstValues);
                    
                    default: return (null, null);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                ResultType resultType = schedule.Results[i];
                
                (StatusType[] stats, int[] values) = GetStatusAndValues(resultType);
                
                List<ScheduleStatReward> rewards = new();
                int count = Mathf.Min(stats.Length, values.Length);
                
                for (int j = 0; j < count; j++)
                    rewards.Add(new ScheduleStatReward(stats[j], values[j]));

                list.Add(new ScheduleResultData(resultType, schedule.Weights[i], schedule.Mental[i], rewards));
            }

            return list;
        }

        private ParttimeLevelData ParseToParttimeLevelData(Schedule schedule)
        {
            Dictionary<ResultType, List<int>> payData = new();
            payData.Add(ResultType.Perfect, schedule.PerfectPay.ToList());
            payData.Add(ResultType.Success, schedule.SuccessPay.ToList());
            payData.Add(ResultType.Fail, schedule.FailPay.ToList());
            payData.Add(ResultType.Worst, schedule.WorstPay.ToList());

            ParttimeLevelData data = new ParttimeLevelData(schedule.ID.ToString(), payData,
                schedule.LevelCount.ToList(), schedule.LevelHealth.ToList());

            data.lv0ItemRewards = ParseItemRewards(schedule.Lv1ItemIDs, schedule.Lv1ItemRates);
            data.lv1ItemRewards = ParseItemRewards(schedule.Lv2ItemIDs, schedule.Lv2ItemRates);
            data.lv2ItemRewards = ParseItemRewards(schedule.Lv3ItemIDs, schedule.Lv3ItemRates);

            return data;
        }

        private List<ScheduleItemReward> ParseItemRewards(int[] ids, int[] rates)
        {
            var list = new List<ScheduleItemReward>();
            if (ids == null || rates == null) return list;

            int count = Mathf.Min(ids.Length, rates.Length);
            for (int i = 0; i < count; i++)
            {
                if (ids[i] == 0) continue;  // 0은 빈 슬롯
                list.Add(new ScheduleItemReward(ids[i].ToString(), rates[i]));
            }

            return list;
        }
        
        public Dictionary<ScheduleType, List<ScheduleData>> GetScheduleDataByDict()
        {
            if (scheduleDataDict.Count != 0)
                return scheduleDataDict; 
            
            scheduleDataDict = new Dictionary<ScheduleType, List<ScheduleData>>();
        
            foreach (var data in scheduleDataList)
            {
                if (data.scheduleType == ScheduleType.None)
                {
                    Debug.LogError("[ScheduleDBSO] 'ScheduleType.None'인 ScheduleSO를 발견했습니다.");
                    continue;
                }
            
                if (!scheduleDataDict.ContainsKey(data.scheduleType)) scheduleDataDict[data.scheduleType] = new List<ScheduleData>();
                scheduleDataDict[data.scheduleType].Add(data);

            }
        
            return scheduleDataDict;
        }

    }
}
