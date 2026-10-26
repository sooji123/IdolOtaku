using System.Collections.Generic;
using System.Linq;

public static class ScheduleResultLogic
{
    // 파트 타임인 경우 아이템을 얻을 수 있음.
    public static ResultData GetResult(ScheduleData scheduleData, ItemData itemData)
    {
        // Get Result By Weighted Random
        ScheduleResultData scheduleResultData = GetRandomResult(scheduleData.scheduleResults);
        ResultType resultType = scheduleResultData.resultType;

        ResultData resultData = new ResultData(scheduleResultData);
        
        // Get Status Result
        if (scheduleResultData.mental != 0)
            resultData.AddStatusChangeData(new StatusChangeData(StatusType.Mental, resultType, scheduleResultData.mental));
        foreach (var statReward in scheduleResultData.statRewards)
            if (statReward.isAvailable)
                resultData.AddStatusChangeData(new StatusChangeData(statReward.statusType, resultType, (int)statReward.value));

        if (scheduleData.scheduleType == ScheduleType.ParttimeJob)
        {
            var levelData = scheduleData.parttimeLevelData;
            if (levelData != null)
            {
                int level = UserDataManager.Instance.GetParttimeLevel(scheduleData.scheduleID);

                // Get Health
                if (levelData.healthCosts != null && levelData.healthCosts.Count > level)
                {
                    int health = -(int)levelData.healthCosts[level];
                    resultData.AddStatusChangeData(new StatusChangeData(StatusType.Health, resultType, health));
                }

                // Get Pay
                if (levelData.payData != null && levelData.payData.ContainsKey(resultType))
                {
                    var payList = levelData.payData[resultType];
                    if (payList != null && payList.Count > level)
                        resultData.AddStatusChangeData(new StatusChangeData(StatusType.Money, resultType, payList[level]));
                }

                // Get Item
                var itemRewards = levelData.GetItemRewardsForLevel(level);
                if (itemRewards != null && itemRewards.Count > 0)
                {
                    var picked = PickWeightedRandomItem(itemRewards);
                    if (picked != null)
                        resultData.itemRewardList.Add(picked);
                }
            }
        }

        // Apply Item Effect
        ApplyItemEffect(itemData, resultData, resultType);

        // Apply Active Buffs
        ApplyActiveBuffs(resultData, resultType);

        return resultData;
    }

    private static void ApplyActiveBuffs(ResultData resultData, ResultType resultType)
    {
        var activeBuffs = UserDataManager.Instance.GetActiveBuffs();

        foreach (var buff in activeBuffs)
        {
            int effectValue = buff.GetEffectValue();
            resultData.AddStatusChangeData(new StatusChangeData(buff.statusType, resultType, effectValue));
        }
    }

    private static void ApplyItemEffect(ItemData itemData, ResultData resultData, ResultType resultType)
    {
        if (itemData == null)
        {
            UnityEngine.Debug.Log("[ScheduleResultLogic] itemData is null");
            return;
        }

        if (itemData.itemStats == null || itemData.itemStats.Count == 0)
        {
            UnityEngine.Debug.Log($"[ScheduleResultLogic] itemStats is null or empty for item {itemData.ItemID}");
            return;
        }

        UnityEngine.Debug.Log($"[ScheduleResultLogic] Applying {itemData.itemStats.Count} stats from item {itemData.ItemID}");

        foreach (var stat in itemData.itemStats)
        {
            // 버프 등록 (duration > 1인 경우 다음 턴에도 적용)
            if (stat.duration > 1)
            {
                var buff = new BuffData(itemData.ItemID, stat.statusType, stat.value, stat.duration - 1);
                UserDataManager.Instance.AddBuff(buff);
            }

            // 현재 턴에 즉시 적용
            int effectValue = CalculateEffectValue(stat);
            UnityEngine.Debug.Log($"[ScheduleResultLogic] Item Effect: {stat.statusType} += {effectValue} (raw value: {stat.value})");
            resultData.AddStatusChangeData(new StatusChangeData(stat.statusType, resultType, effectValue));
        }
    }

    private static int CalculateEffectValue(ItemStatData stat)
    {
        float value = stat.value;

        // Health는 10배
        if (stat.statusType == StatusType.Health)
            value *= 10;

        // 음수면 퍼센트 (TODO: baseValue 연동 필요 시 수정)
        // 현재는 절대값으로 처리
        return (int)value;
    }

    private static ScheduleItemReward PickWeightedRandomItem(List<ScheduleItemReward> itemRewards)
    {
        int totalWeight = 0;
        foreach (var r in itemRewards) totalWeight += (int)r.rate;
        if (totalWeight <= 0) return null;

        int random = UnityEngine.Random.Range(0, totalWeight);
        int accumulated = 0;
        foreach (var r in itemRewards)
        {
            accumulated += (int)r.rate;
            if (random < accumulated)
                return r;
        }

        return itemRewards[itemRewards.Count - 1];
    }

    private static ScheduleResultData GetRandomResult(List<ScheduleResultData> scheduleResults)
    {
        int totalWeight = scheduleResults.Sum(result => result.weight);
    
        int randomValue = UnityEngine.Random.Range(0, totalWeight);
    
        int accumulatedWeight = 0;
        foreach (var result in scheduleResults)
        {
            accumulatedWeight += result.weight;
            if (randomValue < accumulatedWeight)
            {
                return result;
            }
        }
    
        // 혹시 모를 예외 상황을 위해 마지막 결과 반환
        return scheduleResults[scheduleResults.Count - 1];
    }

}
