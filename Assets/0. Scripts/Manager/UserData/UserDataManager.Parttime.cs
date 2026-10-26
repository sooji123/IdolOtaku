using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class UserDataManager
{
    private static readonly string parttimeFileName = "Parttime.json";

    private const int MaxParttimeLevel = 2;

    [UnityEngine.SerializeField] private SavableParttimeData parttimeData = new();

    #region Save/Load
    private void SaveParttimeData()
    {
        Save(parttimeData, parttimeFileName);
    }

    private void LoadParttimeData()
    {
        var data = Load<SavableParttimeData>(parttimeFileName);
        if (data != null) parttimeData = data;
    }
    #endregion

    #region Parttime Level
    public int GetParttimeLevel(string scheduleID)
    {
        if (parttimeData.levels.TryGetValue(scheduleID, out int level))
            return level;
        return 0;
    }

    public int GetParttimeCount(string scheduleID)
    {
        if (parttimeData.counts.TryGetValue(scheduleID, out int count))
            return count;
        return 0;
    }

    // 알바 1회 수행 후 호출. 레벨업 시 true 반환.
    public bool AddParttimeCount(string scheduleID, List<int> levelupCounts)
    {
        int level = GetParttimeLevel(scheduleID);
        if (level >= MaxParttimeLevel) return false;
        if (levelupCounts == null || levelupCounts.Count <= level) return false;

        int count = GetParttimeCount(scheduleID) + 1;
        parttimeData.counts[scheduleID] = count;

        if (count >= levelupCounts[level])
        {
            parttimeData.counts[scheduleID] = 0;
            parttimeData.levels[scheduleID] = level + 1;
            Debug.Log($"[UserDataManager] 알바 레벨업! {scheduleID} → Lv.{level + 1}");
            return true;
        }

        return false;
    }
    #endregion
}

[System.Serializable]
public class SavableParttimeData
{
    [ShowInInspector] public Dictionary<string, int> levels = new();
    [ShowInInspector] public Dictionary<string, int> counts = new();
}
