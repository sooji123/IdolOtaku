using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.Utilities;

[Serializable]
public class ScheduleData
{
    public string scheduleID;
    public ScheduleType scheduleType;
    public SeasonType seasonType;
    public string scheduleIcon;
    public string key;
    public StatusType maxStatusType;
    public int maxStatValue;
    public string scheduleName;
    public List<ScheduleResultData> scheduleResults = new();

    public List<ParttimeLevelData> parttimeLevels = new();      // 비사용

    public ParttimeLevelData parttimeLevelData;

    public ScheduleData()
    {
    }

    public ScheduleData(string scheduleType, string scheduleID, string seasonType, string key,
        string maxStatType, int maxStatValue)
    {
        switch (scheduleType)
        {
            case "study": this.scheduleType = ScheduleType.Study; break;
            case "parttime": this.scheduleType = ScheduleType.ParttimeJob; break;
            case "fangirling": this.scheduleType = ScheduleType.Fanship; break;
            case "rest": this.scheduleType = ScheduleType.Rest; break;
            default: Debug.LogError($"[ScheduleData] Invalide ScheduleType : {scheduleType}"); break;
        }

        this.scheduleID = scheduleID;

        if (!seasonType.IsNullOrWhitespace())
        {
            switch (seasonType)
            {
                case "spring": this.seasonType = SeasonType.Spring; break;
                case "summer": this.seasonType = SeasonType.Summer; break;
                case "fall": this.seasonType = SeasonType.Autumn; break;
                case "winter": this.seasonType = SeasonType.Winter; break;
                case "not_comeback": this.seasonType = SeasonType.NotComeback; break;
                case "comeback": this.seasonType = SeasonType.Comeback; break;
                default: Debug.LogError($"[ScheduleData] Invalide SeasonType : {seasonType}"); break;
            }
        }
        else
        {
            this.seasonType = SeasonType.None;
        }

        this.key = key;

        if (!maxStatType.IsNullOrWhitespace())
        {
            switch (maxStatType)
            {
                case "mental": this.maxStatusType = StatusType.Mental; break;
                case "health": this.maxStatusType = StatusType.Health; break;
                case "study": this.maxStatusType = StatusType.Study; break;
                case "fanship": this.maxStatusType = StatusType.Fanship; break;
                case "money": this.maxStatusType = StatusType.Money; break;
                case "favorability": this.maxStatusType = StatusType.Favorability; break;
                default: Debug.LogError($"[ScheduleStatReward] Invalid StatType : {maxStatType}"); break;
            }
        }
        else
        {
        }
        
        this.maxStatValue = maxStatValue;
    }
    
    public void AddScheduleResult(ScheduleResultData scheduleResultData)
    {
        scheduleResults.Add(scheduleResultData);
    }
    
    public void AddParttimeLevel(ParttimeLevelData parttimeLevelData)
    {
        parttimeLevels.Add(parttimeLevelData);
    }
}

[Serializable]
public class ScheduleResultData
{
    public ResultType resultType;
    public int weight;
    public int mental;
    public List<ScheduleStatReward> statRewards = new();
    
    public ScheduleResultData(string resultType, int weight, int mental,
                              string stat1Type, int stat1Value,
                              string stat2Type, int stat2Value)
    {
        switch (resultType)
        {
            case "perfect": this.resultType = ResultType.Perfect; break;
            case "success": this.resultType = ResultType.Success; break;
            case "fail": this.resultType = ResultType.Fail; break;
            case "worst":  this.resultType = ResultType.Worst; break;
            default: Debug.LogError($"[ScheduleResultData] Invalid ResultType : {resultType}"); break;
        }
        
        this.weight = weight;

        this.mental = mental;
        
        statRewards.Add(new ScheduleStatReward(stat1Type, stat1Value));
        statRewards.Add(new ScheduleStatReward(stat2Type, stat2Value));
    }

    public ScheduleResultData(ResultType resultType, int weight, int mental, List<ScheduleStatReward> statRewards)
    {
        this.resultType = resultType;
        this.weight = weight;
        this.mental = mental;
        this.statRewards = new List<ScheduleStatReward>(statRewards);
    }
}

[Serializable]
public class ScheduleStatReward
{
    public bool isAvailable = false;
    public StatusType statusType;
    public float value;

    public ScheduleStatReward(string statType, float value)
    {
        if (!statType.IsNullOrWhitespace())
        {
            switch (statType)
            {
                case "mental": this.statusType = StatusType.Mental; break;
                case "health": this.statusType = StatusType.Health; break;
                case "study": this.statusType = StatusType.Study; break;
                case "fanship": this.statusType = StatusType.Fanship; break;
                case "money": this.statusType = StatusType.Money; break;
                case "favorability": this.statusType = StatusType.Favorability; break;
                default: Debug.LogError($"[ScheduleStatReward] Invalid StatType : {statType}"); break;
            }

            isAvailable = true;
        }
        else
        {

        }

        this.value = value;
    }

    public ScheduleStatReward(StatusType statType, float value)
    {
        this.statusType = statType;
        this.value = value;
        this.isAvailable = true;
    }
}

[Serializable]
public class ScheduleItemReward
{
    public bool isAvailable = false;
    public string ItemID;
    public float rate;

    public ScheduleItemReward(string item_ID, float rate)
    {
        if (!item_ID.IsNullOrWhitespace()) isAvailable = true;
        
        this.ItemID = item_ID;
        this.rate = rate;
    }
}

[Serializable]
public class ParttimeLevelData
{
    public string partimeID;                // 비사용
    public int count;                       // 비사용
    
    public List<int> payDataList = new();   // 비사용
    public List<int> levelupCounts = new();
    public List<float> healthCosts = new();
    public Dictionary<ResultType, List<int>> payData = new();

    public float health;                    // 비사용
    public List<ScheduleItemReward> itemRewards = new();    // 비사용

    // 레벨별 아이템 보상 (시트 Lv1/Lv2/Lv3 대응, 코드 레벨 0/1/2)
    public List<ScheduleItemReward> lv0ItemRewards = new();
    public List<ScheduleItemReward> lv1ItemRewards = new();
    public List<ScheduleItemReward> lv2ItemRewards = new();

    public List<ScheduleItemReward> GetItemRewardsForLevel(int level)
    {
        switch (level)
        {
            case 0: return lv0ItemRewards;
            case 1: return lv1ItemRewards;
            case 2: return lv2ItemRewards;
            default: return null;
        }
    }

    public ParttimeLevelData(string partimeID, int count,
                            int perfect, int success, int fail, int worst,
                            float health,
                            string item1ID, float item1Rate,
                            string item2ID, float item2Rate,
                            string item3ID, float item3Rate)
    {
        this.partimeID = partimeID;
        this.count = count;
        this.health = health;
        payDataList.Add(perfect);
        payDataList.Add(success);
        payDataList.Add(fail);
        payDataList.Add(worst);
        
        if (!item1ID.IsNullOrWhitespace()) itemRewards.Add(new ScheduleItemReward(item1ID, item1Rate));
        if (!item2ID.IsNullOrWhitespace()) itemRewards.Add(new ScheduleItemReward(item2ID, item2Rate));
        if (!item3ID.IsNullOrWhitespace()) itemRewards.Add(new ScheduleItemReward(item3ID, item3Rate));
    }

    public ParttimeLevelData(string parttimeId,
        Dictionary<ResultType,List<int>> payData, List<int> levelupCounts, List<float> healthCosts)
    {
        this.partimeID = parttimeId;
        this.payData = payData;
        this.levelupCounts = levelupCounts;
        this.healthCosts = healthCosts;
    }
}

[Serializable]
public class ScheduleAnimationData
{
    public string Key;
    public List<Sprite> Clips;
    
    public ScheduleAnimationData(string key, List<Sprite> clips)
    {
        Key = key;
        Clips = clips;
    }
}

public enum ResultType
{
    Perfect,
    Success,
    Fail,
    Worst
}

public enum SeasonType
{
    None,
    Spring,
    Summer,
    Autumn,
    Winter,
    NotComeback,
    Comeback
}