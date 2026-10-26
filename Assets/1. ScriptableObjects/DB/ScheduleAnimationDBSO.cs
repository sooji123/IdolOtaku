using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScheduleAnimationDBSO", menuName = "DB/ScheduleAnimationDBSO")]
public class ScheduleAnimationDBSO : ScriptableObject
{
    public List<ScheduleAnimationData> scheduleAnimationDataList = new();
    
    public void UpdateDB(List<ScheduleAnimationData> scheduleAnimationDataList)
    {
        this.scheduleAnimationDataList = new List<ScheduleAnimationData>(scheduleAnimationDataList);
    }
}