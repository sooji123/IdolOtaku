using Sirenix.OdinInspector;
using Table;
using TableData;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScheduleUnit : MonoBehaviour, IPointerClickHandler
{
    public ScheduleData scheduleData;
    [SerializeField] private Image scheduleImage;
    [SerializeField] private TextMeshProUGUI scheduleName;
    [SerializeField] private TextMeshProUGUI scheduleProcess;

    public void Init(ScheduleData scheduleData)
    {
        this.scheduleData = scheduleData;
        UpdateUnit();
    }

    public void UpdateUnit()
    {
        if (scheduleData == null) return;
        
        string name = LocalizationManager.Instance.GetString(scheduleData.scheduleName);
        Debug.Log(name);
        
        Debug.Log($"{scheduleData.scheduleIcon}");
        scheduleImage.sprite = TableManager.Instance.GetTable<IconAssetTableSO>().GetData(scheduleData.scheduleIcon).Asset;
        scheduleName.text = name;
        
        scheduleProcess.text = $"--%";
    }

    #region IPointer Implements
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Click Detected");
        ScheduleManager.Instance.SetCurrentSelector(scheduleData);
    }

    #endregion
}
