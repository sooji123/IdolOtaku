using Sirenix.OdinInspector;
using Table;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSelector : Selector
{
    [SerializeField, ReadOnly] public ItemData itemData;
    [SerializeField] private Image image;

    [SerializeField, ReadOnly] public bool isLockedByDuration = false;

    #region Public Method
    public void SetItem(ItemData itemData)
    {
        this.itemData = itemData;
        UpdateImage();
    }

    public void SetItemLocked(ItemData itemData)
    {
        this.itemData = itemData;
        isLockedByDuration = true;
        UpdateImage();
    }

    public void ResetItem()
    {
        itemData = null;
        isLockedByDuration = false;
        UpdateImage();
    }
    #endregion

    #region Lock
    public void Unlock()
    {
        isLockedByDuration = false;
    }
    #endregion

    #region Override
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (isLockedByDuration) return;
        base.OnPointerClick(eventData);
    }
    #endregion

    #region Private Method
    private void UpdateImage()
    {
        if (itemData == null)
        {
            image.gameObject.SetActive(false);
            image.sprite = null;
            isContains = false;
        }
        else
        {
            image.gameObject.SetActive(true);
            var iconData = TableManager.Instance.GetTable<IconAssetTableSO>().GetData(itemData.SpriteName);
            image.sprite = iconData?.Asset;
            isContains = true;

            // 잠긴 슬롯은 반투명 처리
            var color = image.color;
            color.a = isLockedByDuration ? 0.5f : 1f;
            image.color = color;
        }
    }
    #endregion
}
