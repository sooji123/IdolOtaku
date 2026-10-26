using Sirenix.OdinInspector;
using System.Collections.Generic;
using Table;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [BoxGroup("Content"), SerializeField] public GameObject itemUnit;
    [BoxGroup("Content"), SerializeField] private Transform itemContent;

    [BoxGroup("Tooltip"), SerializeField] private Image itemIcon;
    [BoxGroup("Tooltip"), SerializeField] private TextMeshProUGUI itemName;
    [BoxGroup("Tooltip"), SerializeField] private TextMeshProUGUI itemDescription;

    public List<ItemUnit> itemUnits = new();

    public Dictionary<ItemType, List<ItemData>> itemDictionary = new();

    private ItemTableSO table;

    [BoxGroup("Button")] private Button equipButton;
    
    #region Built in Event
    private void Start()
    {
        table = TableManager.Instance.GetTable<ItemTableSO>();
        
        SetItemPanel(ItemType.Study);
        itemDictionary = table.GetItemsByDict();
    }
    #endregion

    #region Content
    public void SetItemPanelByIndex(int index)
    {
        if (index < 0 || index >= 4) return;

        switch (index)
        {
            case 0: SetItemPanel(ItemType.Study); break;
            case 1: SetItemPanel(ItemType.ParttimeJob); break;
            case 2: SetItemPanel(ItemType.Fanship); break;
            case 3: SetItemPanel(ItemType.Rest); break;
        }
    }

    private void SetItemPanel(ItemType itemType)
    {
        itemUnits.Clear();
        ClearContent();
        ResetTooltip();

        if (itemDictionary == null) itemDictionary = table.GetItemsByDict();

        if (!itemDictionary.ContainsKey(itemType)) return;
        
        var items = itemDictionary[itemType];
       
        if (items == null) return;
        
        foreach (var i in items)
        {
            itemUnits.Add(GetItemUnit(itemContent, i));
        }
    }

    private void ClearContent()
    {
        for (int i = itemContent.childCount - 1; i >= 0; i--)
        {
            Transform child = itemContent.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    private ItemUnit GetItemUnit(Transform parent, ItemData itemData)
    {
        GameObject go = Instantiate(itemUnit, parent);
        ItemUnit co = go.GetComponent<ItemUnit>();
        co.Init(this, itemData);
        return co;
    }
    #endregion

    #region Selection
    public void Select(ItemUnit selectedUnit)
    {
        foreach (ItemUnit itemUnit in itemUnits)
            itemUnit.UnSelect();

        selectedUnit.Select();
        SetTooltip(selectedUnit.itemData);
    }
    #endregion

    #region Quantity
    public void RefreshAllQuantity()
    {
        foreach (var unit in itemUnits)
            unit.UpdateUnit();
    }
    #endregion

    #region ItemTooltip
    private void ResetTooltip()
    {
        if (itemIcon != null) itemIcon.sprite = null;
        itemName.text = "";
        itemDescription.text = "";
    }

    private void SetTooltip(ItemData itemData)
    {
        if (itemIcon != null)
        {
            var iconData = TableManager.Instance.GetTable<IconAssetTableSO>().GetData(itemData.SpriteName);
            itemIcon.sprite = iconData?.Asset;
        }

        itemName.text = LocalizationManager.Instance.GetString(itemData.Name);
        itemDescription.text = LocalizationManager.Instance.GetString(itemData.Description);
    }
    #endregion
}
