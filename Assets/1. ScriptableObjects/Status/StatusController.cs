using System;
using UnityEngine;
using System.Collections.Generic;
using TableData;

public class StatusController : Singleton<StatusController>
{
    [SerializeField] public StatusSO statusSO;

    [SerializeField] private StatusUI statusUI;

    [SerializeField] private List<StatusHightLightUI> hightLightUIList = new();
    
    [SerializeField] private StatusHightLightUI mentalHighlight;
    [SerializeField] private StatusHightLightUI healthHighlight;
    [SerializeField] private StatusHightLightUI studyHighlight;
    [SerializeField] private StatusHightLightUI fanshipHighlight;
    [SerializeField] private StatusHightLightUI moneyHighlight;

    private void Start()
    {
        ResetAllStatusHightlights();
    }

    public void ResetAllStatusHightlights()
    {
        foreach (var highlight in hightLightUIList)
            highlight.ResetHightLight();
    }
    
    public void ApplyResultData(otakuevent otakuevent, ResultType type) => ApplyResultData(new ResultData(otakuevent, type));

    public void ApplyResultData(ResultData resultData)
    {
        ResetAllStatusHightlights();
        AddSchedulePerHP(resultData);
        ApplyStatusChangeDataList(resultData.statusChangeDataList);
        statusUI.UpdateStatusUI();
    }

    private void AddSchedulePerHP(ResultData resultData)
    {
        if (resultData.scheduleResultData == null)
        {
            return;
        }
        resultData.AddStatusChangeData(new StatusChangeData(StatusType.Health,
            resultData.scheduleResultData.resultType, -1 * (int)statusSO.status.schedulePerHp));
    }

    private void ApplyStatusChangeDataList(List<StatusChangeData> statusChangeDataList)
    {
        Status status = statusSO.status;
        
        foreach (var chageData in statusChangeDataList)
            ChangeStatus(status, chageData);
    }

    private void ChangeStatus(Status status, StatusChangeData changeData)
    {
        switch (changeData.statusType)
        {
            case StatusType.Mental:
            {
                status.AddMental(changeData.value);;
                mentalHighlight.SetHighLight(changeData.resultType);
                break;
            }
            case StatusType.Health:
            {
                status.AddHP(changeData.value);
                healthHighlight.SetHighLight(changeData.resultType);
                break;
            }
            case StatusType.Study:
            {
                status.AddStudy(changeData.value);
                studyHighlight.SetHighLight(changeData.resultType);
                break;
            }
            case StatusType.Fanship:
            {
                status.AddFanship(changeData.value);
                fanshipHighlight.SetHighLight(changeData.resultType);
                break;
            }
            case StatusType.Money:
            {
                status.AddMoney(changeData.value);
                moneyHighlight.SetHighLight(changeData.resultType);
                break;
            }
            case StatusType.Favorability:
            {
                status.Favorability += changeData.value;
                break;
            }
            default: break;
        }
    }
}

public class ResultData
{
    public ResultType resultType;
    public ScheduleResultData scheduleResultData;
    public List<StatusChangeData> statusChangeDataList = new();
    public List<ScheduleItemReward> itemRewardList = new();

    public ResultData(ScheduleResultData scheduleResultData)
    {
        resultType = scheduleResultData.resultType;
        this.scheduleResultData = scheduleResultData;
        this.statusChangeDataList = new List<StatusChangeData>();
        this.itemRewardList = new List<ScheduleItemReward>(itemRewardList);
    }

    public ResultData(otakuevent data, ResultType resultType)
    {
        if (data.resultData.Count == 0) return;

        this.resultType = resultType;

        OtakuEventResult result = null;
        foreach (var d in data.resultData)
            if (d.resultType == resultType)
                result = d;
        
        statusChangeDataList.Add(new StatusChangeData(StatusType.Mental, resultType, result.mental));
        statusChangeDataList.Add(new StatusChangeData(StatusType.Fanship, resultType, result.fanship));
    }

    public void AddStatusChangeData(StatusChangeData statusChangeData)
    {
        foreach (var data in statusChangeDataList)
        {
            if (data.statusType == statusChangeData.statusType)
            {
                data.value += statusChangeData.value;
                return;
            }
        }
        
        statusChangeDataList.Add(statusChangeData);
    }
}
public class StatusChangeData
{
    public StatusType statusType;
    public ResultType resultType;
    public int value;

    public StatusChangeData(StatusType statusType, ResultType resultType, int value)
    {
        this.statusType = statusType;
        this.resultType = resultType;
        this.value = value;
    }
}

public enum StatusType
{
    Health,
    Mental,
    Study,
    Fanship,
    Money,
    Favorability
}