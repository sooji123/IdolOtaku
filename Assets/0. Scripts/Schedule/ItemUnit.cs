using Table;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUnit : MonoBehaviour, IPointerClickHandler
{
    private ItemController controller;
    public ItemData itemData;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image contour;
    [SerializeField] private TextMeshProUGUI itemQuantity;

    public void Init(ItemController itemContoller, ItemData itemData)
    {
        UnSelect();
        this.controller = itemContoller;
        this.itemData = itemData;
        UpdateUnit();
    } 

    public void UpdateUnit()
    {
        var iconData = TableManager.Instance.GetTable<IconAssetTableSO>().GetData(itemData.SpriteName);
        itemImage.sprite = iconData?.Asset;

        int quantity = UserDataManager.Instance.GetItemQuantity(itemData.ItemID);
        itemQuantity.text = $"x{quantity}";
    }

    #region Selection
    public void Select()
    {
        contour.gameObject.SetActive(true);
    }

    public void UnSelect()
    {
        contour.gameObject.SetActive(false);
    }

    public void OnSelect()
    {
        controller.Select(this);
    }
    #endregion

    #region IPointer Implements
    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelect();
        
        if (!UserDataManager.Instance.HasItem(itemData.ItemID))
            return;
        
        ScheduleManager.Instance.SetCurrentSelector(itemData);
    }
    #endregion
}
