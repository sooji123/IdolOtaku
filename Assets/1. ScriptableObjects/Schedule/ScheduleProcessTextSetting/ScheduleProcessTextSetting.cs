using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ScheduleProcessTextSetting", menuName = "Scriptable Objects/ScheduleProcessTextSetting")]
public class ScheduleProcessTextSetting : ScriptableObject
{
    public string studyStart;
    public string parttimeStart;
    public string fanGirlingStart;
    public string restStart;

    public string scheduling = ".........";
    public List<string> studyResults = new List<string>();
    public List<string> parttimeResults = new List<string>();
    public List<string> fanGirlingResults = new List<string>();
    public List<string> restResults = new List<string>();

    public string GetScheduleStartString(ScheduleType scheduleType)
    {
        switch (scheduleType)
        {
            case ScheduleType.Study: return studyStart;
            case ScheduleType.ParttimeJob: return parttimeStart;
            case ScheduleType.Fanship: return fanGirlingStart;
            case ScheduleType.Rest: return restStart;
            default:
                Debug.LogError($"[ScheduleProcessTextSetting] �̻��� Ÿ���� ScheduleType ��û");
                return "";
        }
    }

    public string GetSchedulingText()
    {
        return scheduling;
    }

    public string GetScheduleResultString(ScheduleType scheduleType,
                                          ResultType scheduleResult)
    {
        switch (scheduleType)
        {
            case ScheduleType.Study:        return studyResults[(int)scheduleResult];
            case ScheduleType.ParttimeJob:  return parttimeResults[(int)scheduleResult];
            case ScheduleType.Fanship:      return fanGirlingResults[(int)scheduleResult];
            case ScheduleType.Rest:         return restResults[(int)scheduleResult];
            default:
                Debug.LogError($"[ScheduleProcessTextSetting] �̻��� Ÿ���� ScheduleType ��û");
                return "";
        }
    }

    public string GetScheduleResultString(ResultData resultData)
    {
        string result = "";
        
        foreach (var status in resultData.statusChangeDataList)
            result += GetStatusName(status.statusType) + " " + status.value + "\n";

        return result;
    }
    
    public string GetStatusName(StatusType statusType)
    {
        switch (statusType)
        {
            case StatusType.Mental: return "멘탈";
            case StatusType.Money: return "돈";
            case StatusType.Fanship: return "덕질";
            case StatusType.Favorability: return "호감도";
            case StatusType.Health: return "체력";
            case StatusType.Study: return "학업";
            default: return "";
        }
    }
}
