using System;
using System.Collections.Generic;

[Serializable]
public class ItemData
{
    public string ItemID;
    public string Name;
    public string SpriteName;
    public ItemType Type;
    public List<ItemStatData> itemStats;
    public string Description;

    public ItemData(string itemID, string name,
                    string spriteName, ItemType type,
                    string stat1, float value1, int duration1, bool isPercent1,
                    string stat2, float value2, int duration2, bool isPercent2,
                    string description)
    {
        ItemID = itemID;
        Name = name;
        SpriteName = spriteName;
        Type = type;
        Description = description;
    }

    public ItemData(string itemID, ItemType itemType,
        string iconName, string itemName, string itemDescription,
        List<ItemStatData> itemStats)
    {
        ItemID = itemID;
        Type = itemType;
        SpriteName = iconName;
        Name = itemName;
        Description = itemDescription;
        this.itemStats = itemStats;
    }
}

[Serializable]
public class ItemStatData
{
    public StatusType statusType;
    public float value;
    public int duration;
    
    public ItemStatData(StatusType statusType, float value, int duration)
    {
        this.statusType = statusType;
        this.value = value;
        this.duration = duration;
    }
}
