using System.Collections.Generic;
using Sirenix.OdinInspector;
using Table;
using UnityEngine;

public partial class UserDataManager
{
    [SerializeField] private SavableItemData itemData = new();

    #region Item Initialize
    [Button]
    public void InitializeDefaultItems(int defaultQuantity = 1)
    {
        itemData.inventory.Clear();

        var itemDict = TableManager.Instance.GetTable<ItemTableSO>().GetItemsByDict();
        foreach (var itemList in itemDict.Values)
        {
            foreach (var item in itemList)
            {
                if (!itemData.inventory.ContainsKey(item.ItemID))
                    itemData.inventory[item.ItemID] = defaultQuantity;
            }
        }

        Debug.Log($"[UserDataManager] Initialized {itemData.inventory.Count} items with quantity {defaultQuantity}");
    }
    #endregion

    #region Item Save/Load
    private void SaveItemData()
    {
        Save(itemData, itemFileName);
    }

    private void LoadItemData()
    {
        var data = Load<SavableItemData>(itemFileName);
        if (data != null) itemData = data;
    }
    #endregion

    #region Item Inventory
    public int GetItemQuantity(string itemID)
    {
        if (itemData.inventory.TryGetValue(itemID, out int quantity))
            return quantity;
        return 0;
    }

    public void AddItem(string itemID, int amount = 1)
    {
        if (itemData.inventory.ContainsKey(itemID))
            itemData.inventory[itemID] += amount;
        else
            itemData.inventory[itemID] = amount;
    }

    public bool UseItem(string itemID, int amount = 1)
    {
        if (!itemData.inventory.ContainsKey(itemID))
            return false;

        if (itemData.inventory[itemID] < amount)
            return false;

        itemData.inventory[itemID] -= amount;

        if (itemData.inventory[itemID] <= 0)
            itemData.inventory.Remove(itemID);

        return true;
    }

    public bool HasItem(string itemID, int amount = 1)
    {
        return GetItemQuantity(itemID) >= amount;
    }

    public Dictionary<string, int> GetAllItems()
    {
        return new Dictionary<string, int>(itemData.inventory);
    }
    #endregion
}

[System.Serializable]
public class SavableItemData
{
    public Dictionary<string, int> inventory = new();
}
