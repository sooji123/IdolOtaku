using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleSelector : Selector
{
    [SerializeField, ReadOnly] public ScheduleData scheduleData;
    [SerializeField] private Image image;

    #region Public Method
    public void SetSchedule(ScheduleData scheduleData)
    {
        this.scheduleData = scheduleData;
        UpdateImage();
    }

    public void ResetSchedule()
    {
        scheduleData = new ScheduleData();
        UpdateImage();
        this.HideContour();
    }

    public bool IsEmpty()
    {
        return scheduleData.scheduleID.IsNullOrWhitespace();
    }

    #endregion

    #region Private Method
    private void UpdateImage()
    {
        if (scheduleData == null || string.IsNullOrEmpty(scheduleData.scheduleIcon))
        {
            image.sprite = null;
            isContains = false;
        }
        else
        {
            var iconData = TableManager.Instance.GetTable<IconAssetTableSO>().GetData(scheduleData.scheduleIcon);
            image.sprite = iconData?.Asset;
            isContains = true;
        }
    }
    #endregion
}