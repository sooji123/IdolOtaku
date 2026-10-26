using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ScheduleDBSO", menuName = "DB/ScheduleDBSO")]
public class ScheduleDBSO : ScriptableObject
{
    public List<ScheduleData> schedules = new();
    public List<ScheduleAnimationData> scheduleAnimations = new();

    public void UpdateDB(List<ScheduleData> scheduleList, List<ScheduleAnimationData> scheduleAnimation)
    {
        schedules = new List<ScheduleData>(scheduleList);
        scheduleAnimations = new List<ScheduleAnimationData>(scheduleAnimation);
    }

    public Dictionary<ScheduleType, List<ScheduleData>> GetScheduleDataByDict()
    {
        Dictionary<ScheduleType, List<ScheduleData>> pairs = new Dictionary<ScheduleType, List<ScheduleData>>();
        
        foreach (var data in schedules)
        {
            if (data.scheduleType == ScheduleType.None)
            {
                Debug.LogError("[ScheduleDBSO] 'ScheduleType.None'인 ScheduleSO를 발견했습니다.");
                continue;
            }
            
            if (!pairs.ContainsKey(data.scheduleType)) pairs[data.scheduleType] = new List<ScheduleData>();
            pairs[data.scheduleType].Add(data);

        }
        
        return pairs;
    }
}

public enum ScheduleType
{
    None,
    Study,
    ParttimeJob,
    Fanship,
    Rest,
}