using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "ItemDBSO", menuName = "DB/ItemDBSO")]
public class ItemDBSO : ScriptableObject
{
    public List<ItemData> itemDataList = new List<ItemData>();
    [ShowInInspector] public Dictionary<ItemType, List<ItemData>> itemDict = new Dictionary<ItemType, List<ItemData>>();
    
    public void UpdateDB(List<ItemData> itemDataList)
    {
        this.itemDataList = new List<ItemData>(itemDataList);
    }
    
    public Dictionary<ItemType, List<ItemData>> GetItemsByDict()
    {
        if (itemDict.Count != 0)
            return itemDict;
        
        foreach (var itemData in itemDataList)
        {
            if (itemData.Type == ItemType.None)
            {
                Debug.LogError("[ScheduleDBSO] 'ItemType.None'");
                continue;
            }

            foreach (ItemType flag in System.Enum.GetValues(typeof(ItemType)))
            {
                if (flag == ItemType.None) continue;

                bool isSingleFlag = ((int)flag & ((int)flag - 1)) == 0;
                if (!isSingleFlag) continue;

                if ((itemData.Type & flag) == flag)
                {
                    if (!itemDict.ContainsKey(flag)) itemDict[flag] = new List<ItemData>();
                    itemDict[flag].Add(itemData);
                }
            }
        }
        
        return itemDict;
    }
}
