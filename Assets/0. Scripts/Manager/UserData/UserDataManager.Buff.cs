using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class UserDataManager
{
    [SerializeField] private List<BuffData> activeBuffs = new();

    #region Buff Management
    public void AddBuff(BuffData buff)
    {
        activeBuffs.Add(buff);
        Debug.Log($"[Buff] Added: {buff.statusType} {buff.value} for {buff.remainingTurns} turns");
    }

    public void AddBuffsFromItem(ItemData itemData)
    {
        if (itemData?.itemStats == null) return;

        foreach (var stat in itemData.itemStats)
        {
            var buff = new BuffData(
                itemData.ItemID,
                stat.statusType,
                stat.value,
                stat.duration
            );
            AddBuff(buff);
        }
    }

    public List<BuffData> GetActiveBuffs()
    {
        return new List<BuffData>(activeBuffs);
    }

    public List<BuffData> GetActiveBuffsByType(StatusType statusType)
    {
        return activeBuffs.FindAll(b => b.statusType == statusType);
    }

    public int GetTotalBuffValue(StatusType statusType, int baseValue = 0)
    {
        int total = 0;
        foreach (var buff in activeBuffs)
        {
            if (buff.statusType == statusType)
                total += buff.GetEffectValue(baseValue);
        }
        return total;
    }

    [Button]
    public void ProcessTurnEnd()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            if (activeBuffs[i].Tick())
            {
                Debug.Log($"[Buff] Expired: {activeBuffs[i].statusType}");
                activeBuffs.RemoveAt(i);
            }
        }
    }

    [Button]
    public void ClearAllBuffs()
    {
        activeBuffs.Clear();
        Debug.Log("[Buff] All buffs cleared");
    }
    #endregion
}
