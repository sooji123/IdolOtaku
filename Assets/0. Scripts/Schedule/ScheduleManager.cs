using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class ScheduleManager : Singleton<ScheduleManager>
{
    [SerializeField] private StatusSO statusSO;

    public bool isEventSequence = false;
    public bool isTest = false;
    public bool isScheduleProcessing = false;
    
    #region Controller Variable
    [Header("Controller")]
    [SerializeField] private ScheduleController scheduleController;
    [SerializeField] private FoldOutButtonController foldOutButtonController;
    #endregion
    
    [SerializeField] private GameObject hamberger;
    
    #region Item
    public bool isItem = false;
    public GameObject itemPanel;
    #endregion

    #region Built in Event
    private void Start()
    {
        SetPanelToSchedule();
    }
    #endregion

    #region Panel Control
    public void SetPanelToItem(int index)
    {
        foldOutButtonController.SetItemPanelByIndex(index); //None ����

        itemPanel.SetActive(true);
        isItem = true;
    }

    public void SetPanelToSchedule()
    {
        itemPanel.SetActive(false);
        isItem = false;

        foldOutButtonController.ShowLast();
    }

    public void ShowSchedulePanel()
    {
        foldOutButtonController.ShowLast();
    }
    #endregion

    #region Schedule/ItemSelection
    public void SetCurrentSelector(ItemData itemData)
    {
        scheduleController.SetCurrentSelector(itemData);
        RefreshItemQuantity();
    }

    public void SetCurrentSelector(ScheduleData scheduleData)
    {
        scheduleController.SetCurrentSelector(scheduleData);
    }

    public void RefreshItemQuantity()
    {
        foldOutButtonController.RefreshItemQuantity();
    }
    #endregion
    
    #region Hamburger Control
    public void OpenHamburger()
    {
        hamberger.SetActive(true);
    }

    public void CloseHamburger()
    {
        hamberger.SetActive(false);
    }
    #endregion
    
    #region Callee Event
    public void OnScheduleEnd()
    {
        //Event Chain
        scheduleController.OnScheduleEnd();
    }

    public void OnDateEnd()
    {
        scheduleController.OnDateEnd();
    }
    #endregion
}