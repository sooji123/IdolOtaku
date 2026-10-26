using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Table;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleController : MonoBehaviour
{
    [SerializeField] private FoldOutButtonController foldOut;

    private ScheduleTableSO table;
    
    #region Content Variable
    [BoxGroup("Content"), SerializeField] public GameObject scheduleUnit;
    [BoxGroup("Content"), SerializeField] private Transform studyContent;
    [BoxGroup("Content"), SerializeField] private Transform parttimejobContent;
    [BoxGroup("Content"), SerializeField] private Transform fanshipContent;
    [BoxGroup("Content"), SerializeField] private Transform restContent;
    [BoxGroup("Content"), SerializeField] public List<ScheduleUnit> scheduleUnits = new List<ScheduleUnit>();
    [SerializeField] private TextMeshProUGUI examDate;
    [SerializeField] private TextMeshProUGUI idolDate;
    #endregion

    #region Week
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TestController testController;
    private bool isWeekend => UserDataManager.Instance.GetIsWeekend();
    [SerializeField] private GameObject weekdayPanel;
    [SerializeField] private GameObject weekendPanel;
        
    [SerializeField] private List<ScheduleItem> weekday = new List<ScheduleItem>();
    [SerializeField] private List<ScheduleItem> weekend = new List<ScheduleItem>();

    [SerializeField] private Selector currentSelector;
    #endregion
    
    #region Main Menu
    [SerializeField] private TextMeshProUGUI mainMenuDayText;
    [SerializeField] private TextMeshProUGUI mainMenuDateText;
    #endregion

    #region Processor 
    [SerializeField] private ScheduleProcessor scheduleProcessor;
    [SerializeField] private Button startButton;
    [SerializeField] private Button testButton;
    #endregion
    
    #region Event
    [SerializeField] private ScheduleEventController scheduleEventController;
    #endregion
    
    #region Date
    private int dateIndex => UserDataManager.Instance.GetDateIndex();
    #endregion
    
    /// <summary>
    /// 하드 코딩된 문자들 전부 수정 필요.
    /// 그러려면 데이터 시트 필요함
    /// TableSO로 연동할 것.
    /// </summary>

    #region Built in Event
    private void Start()
    {
        Init();

        if (testController != null)
        {
            testController.OnTestSequenceCompleted += HandleAfterTest;
        }
    }
    #endregion

    #region Initialize
    private void Init()
    {
        table = TableManager.Instance.GetTable<ScheduleTableSO>();

        InitContent();

        for (int i = 0; i < weekday.Count; i++)
            weekday[i].dateText.text = GetDateString(i);

        weekday[0].scheduleSelector.Select();
        foldOut.Show(0);

        UpdateDate();
        UpdateEventDates();
    }

    #region Content
    private void InitContent()
    {
        Dictionary<ScheduleType, List<ScheduleData>> scheduleDataList = table.GetScheduleDataByDict();

        foreach (var schedule in scheduleDataList)
        {
            Transform parent = null;
            switch (schedule.Key)
            {
                case ScheduleType.Study: parent = studyContent; break;
                case ScheduleType.ParttimeJob: parent = parttimejobContent; break;
                case ScheduleType.Fanship: parent = fanshipContent; break;
                case ScheduleType.Rest: parent = restContent; break;
            }

            foreach (var item in schedule.Value)
            {
                scheduleUnits.Add(GetScheduleUnit(parent, item));
            }
        }
    }
    private ScheduleUnit GetScheduleUnit(Transform parent, ScheduleData scheduleData)
    {
        GameObject go = Instantiate(scheduleUnit, parent);
        ScheduleUnit co = go.GetComponent<ScheduleUnit>();
        co.Init(scheduleData);
        return co;
    }
    #endregion
    #endregion

    private void UpdateDate()
    {
        mainMenuDayText.text = $"D+{dateIndex.ToString("D3")}";

        string weekText = isWeekend ? "주말" : "평일";
        dayText.text = $"{GetDate()} - {weekText}";
        mainMenuDateText.text = $"{GetDate()} - {weekText}";
    }

    #region Reset
    public void ResetAllScheduleItem()
    {
        foreach (var item in weekday)
            item.ResetData();

        foreach (var item in weekend)
            item.ResetData();
    }
    #endregion

    #region Week Control
    private void NextWeek()
    {
        UserDataManager.Instance.ToggleIsWeekend();
        UpdateWeekPanel();
    }

    private void UpdateWeekPanel()
    {
        weekdayPanel.SetActive(!isWeekend);
        weekendPanel.SetActive(isWeekend);

        if (!isWeekend)
        {
            for (int i = 0; i < weekday.Count; i++)
            {
                weekday[i].dateText.text = GetDateString(i);
            }

            UpdateDate();
            
            weekday[0].scheduleSelector.Select();
            foldOut.Unlock(0);
            foldOut.Show(0);
        }
        else
        {
            for (int i = 0; i < weekend.Count; i++)
            {
                // TODO : Remove Magic Number
                weekend[i].dateText.text = GetDateString(i);
            }

            UpdateDate();
            
            weekend[0].scheduleSelector.Select();
            foldOut.Lock(0);
            foldOut.Show(1);
        }
    }
    #endregion

    #region Select
    public void Select(Selector selector)
    {
        bool Validate()
        {
            if (selector is ScheduleSelector) return true;

            if (selector is ItemSelector itemSelector)
            {
                // 잠긴 슬롯은 선택 불가
                if (itemSelector.isLockedByDuration) return false;

                var week = new List<ScheduleItem>(weekday);
                week.AddRange(weekend);
                foreach (var item in week)
                {
                    if (item.itemSelector == selector)
                    {
                        if (item.scheduleSelector.isContains)
                            return true;
                        else return false;
                    }
                }
            }

            return false;
        }

        if (!Validate()) return;

        HideAll();
        selector.ShowContour();
        currentSelector = selector;
        ShowPanel();
    }

    public void HideAll()
    {
        foreach (var item in weekday)
        {
            item.scheduleSelector.HideContour();
            item.itemSelector.HideContour();
        }
        foreach (var item in weekend)
        {
            item.scheduleSelector.HideContour();
            item.itemSelector.HideContour();
        }
        currentSelector = null;
    }

    private void ShowPanel()
    {
        int GetIndex()
        {
            if (currentSelector is ScheduleSelector) return -1;
            else if (currentSelector is ItemSelector)
            {
                var week = new List<ScheduleItem>(weekday);
                week.AddRange(weekend);
                foreach (var item in week)
                {
                    if (item.itemSelector == currentSelector)
                    {
                        return (int)item.scheduleSelector.scheduleData.scheduleType - 1;
                    }
                }
            }
            return -1;
        }

        if (currentSelector == null) return;
        else if (currentSelector is ScheduleSelector)
        {
            ScheduleManager.Instance.ShowSchedulePanel();
            ScheduleManager.Instance.SetPanelToSchedule();
        }
        else if (currentSelector is ItemSelector)
        {
            ScheduleManager.Instance.SetPanelToItem(GetIndex());
        }
    }
    #endregion

    #region Selector
    public void SetCurrentSelector(ItemData itemData)
    {
        ItemSelector itemSelector = (ItemSelector)currentSelector;
        int currentIndex = GetCurrentItemSelectorIndex();
        if (currentIndex < 0) return;

        List<ScheduleItem> targetList = isWeekend ? weekend : weekday;
        int maxSlots = targetList.Count;

        if (itemSelector.itemData != null && itemSelector.itemData.ItemID == itemData.ItemID)
        {
            UnequipItemWithDuration(currentIndex, itemSelector.itemData);
        }
        else
        {
            if (itemSelector.itemData != null)
                UnequipItemWithDuration(currentIndex, itemSelector.itemData);

            EquipItemWithDuration(currentIndex, itemData, maxSlots);
        }
    }

    private int GetCurrentItemSelectorIndex()
    {
        List<ScheduleItem> targetList = isWeekend ? weekend : weekday;

        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i].itemSelector == currentSelector)
                return i;
        }
        return -1;
    }

    private int GetItemDuration(ItemData itemData)
    {
        if (itemData?.itemStats == null || itemData.itemStats.Count == 0)
            return 1;

        int maxDuration = 0;
        foreach (var stat in itemData.itemStats)
        {
            if (stat.duration > maxDuration)
                maxDuration = stat.duration;
        }

        return maxDuration == 0 ? 1 : maxDuration;
    }

    private void EquipItemWithDuration(int startIndex, ItemData itemData, int maxSlots)
    {
        List<ScheduleItem> targetList = isWeekend ? weekend : weekday;
        int duration = GetItemDuration(itemData);
        int endIndex = Mathf.Min(startIndex + duration, maxSlots);

        for (int i = startIndex; i < endIndex; i++)
        {
            var selector = targetList[i].itemSelector;

            if (selector.itemData != null && !selector.isLockedByDuration)
            {
                UnequipItemWithDuration(i, selector.itemData);
            }
        }

        for (int i = startIndex; i < endIndex; i++)
        {
            var selector = targetList[i].itemSelector;

            if (i == startIndex)
                selector.SetItem(itemData);
            else
                selector.SetItemLocked(itemData);
        }

        UserDataManager.Instance.UseItem(itemData.ItemID);
    }

    private void UnequipItemWithDuration(int startIndex, ItemData itemData)
    {
        List<ScheduleItem> targetList = isWeekend ? weekend : weekday;
        int duration = GetItemDuration(itemData);
        int endIndex = Mathf.Min(startIndex + duration, targetList.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            var selector = targetList[i].itemSelector;

            if (selector.itemData != null && selector.itemData.ItemID == itemData.ItemID)
                selector.ResetItem();
        }

        UserDataManager.Instance.AddItem(itemData.ItemID);
    }

    public void SetCurrentSelector(ScheduleData scheduleData)
    {
        ScheduleSelector scheduleSelector = (ScheduleSelector)currentSelector;
        scheduleSelector.SetSchedule(scheduleData);
        
        CheckCurrentScheduleState();
        
        ClearCurrentScheduleItem();
        SelectNextScheduleSelector();
    }

    #endregion

    #region Utils
    private void SelectNextScheduleSelector()
    {
        if (currentSelector is ItemSelector) return;

        for (int i = 0; i < weekday.Count; i++)
            if (currentSelector == weekday[i].scheduleSelector &&
                i + 1 < weekday.Count)
            {
                weekday[i + 1].scheduleSelector.Select();
                return;
            }

        for (int i = 0; i < weekend.Count; i++)
            if (currentSelector == weekend[i].scheduleSelector &&
                i + 1 < weekend.Count)
            {
                weekend[i + 1].scheduleSelector.Select();
                return;
            }
    }

    private void ClearCurrentScheduleItem()
    {
        if (currentSelector is ItemSelector) return;

        for (int i = 0; i < weekday.Count; i++)
            if ((ScheduleSelector)currentSelector == weekday[i].scheduleSelector)
            {
                var itemSelector = weekday[i].itemSelector;

                if (itemSelector.isLockedByDuration) return;

                if (itemSelector.itemData != null)
                {
                    UnequipItemWithDuration(i, itemSelector.itemData);
                }
                return;
            }

        for (int i = 0; i < weekend.Count; i++)
            if ((ScheduleSelector)currentSelector == weekend[i].scheduleSelector)
            {
                var itemSelector = weekend[i].itemSelector;
                
                if (itemSelector.isLockedByDuration) return;

                if (itemSelector.itemData != null)
                {
                    UnequipItemWithDuration(i, itemSelector.itemData);
                }
                return;
            }
    }

    public bool IsFull()
    {
        List<ScheduleItem> target = isWeekend ? weekend : weekday;
        foreach (var item in target)
        {
            if (item.scheduleSelector.IsEmpty())
            {
                return false;   
            }
        }

        return true;
    }
    #endregion

    #region Schedule Processor
    public void StartScheduleProcess()
    {
        scheduleProcessor.StartScheduleProcess(GetScheduleProcessNode());
    }

    private List<ScheduleProcessNode> GetScheduleProcessNode()
    {
        List<ScheduleProcessNode> nodes = new List<ScheduleProcessNode>();
        List<ScheduleItem> target = isWeekend ? weekend : weekday;
        
        for (int i = 0 ; i < target.Count; i++)
            nodes.Add(new ScheduleProcessNode(GetNextDate(),
                target[i].scheduleSelector.scheduleData,
                target[i].itemSelector.itemData));

        return nodes; 
    }
    
    private void CheckCurrentScheduleState()
    {
        startButton.interactable = IsFull(); ;
    }

    public void OnDateEnd()
    {
        scheduleEventController.StartEventSequence(isWeekend);
    }
    #endregion
    
    #region Date Control
    public string GetNextDate()
    {
        DateTime baseDate = new DateTime(2026, 3, 2);
        int idx = UserDataManager.Instance.AdvanceDateIndex();
        DateTime targetDate = baseDate.AddDays(idx);

        string dayOfWeek = "";
        switch (targetDate.DayOfWeek)
        {
            case DayOfWeek.Monday: dayOfWeek = "월"; break;
            case DayOfWeek.Tuesday: dayOfWeek = "화"; break;
            case DayOfWeek.Wednesday: dayOfWeek = "수"; break;
            case DayOfWeek.Thursday: dayOfWeek = "목"; break;
            case DayOfWeek.Friday: dayOfWeek = "금"; break;
            case DayOfWeek.Saturday: dayOfWeek = "토"; break;
            case DayOfWeek.Sunday: dayOfWeek = "일"; break;
        }

        return $"{targetDate.Month}/{targetDate.Day}일({dayOfWeek})";
    }

    public string GetDateString(int addedDate)
    {
        DateTime baseDate = new DateTime(2026, 3, 2);
        DateTime targetDate = baseDate.AddDays(dateIndex + addedDate);

        return targetDate.Day.ToString();
    }

    public string GetDate()
    {
        DateTime baseDate = new DateTime(2026, 3, 2);
        DateTime targetDate = baseDate.AddDays(dateIndex);

        int yearNumber = ((targetDate - baseDate).Days / 365) + 1;
        
        DateTime firstDayOfMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
        int weekOfMonth = ((targetDate.Day - 1) / 7) + 1;

        return $"{yearNumber}년차 {targetDate.Month}월 {weekOfMonth}주차";
    }

    #endregion

    #region  Events
    public void OnScheduleEnd()
    {
        ResetAllScheduleItem();
        NextWeek();
        CheckCurrentScheduleState();
        UpdateEventDates();
    }
    #endregion

    #region Event Dates
    private void UpdateEventDates()
    {
        var calendarTable = TableManager.Instance.GetTable<Table.calendarTableSO>();

        int schoolDays = calendarTable.GetDaysUntilNextEvent(EventType.school_test);
        int idolDays   = calendarTable.GetDaysUntilNextEvent(EventType.idol);


        if (schoolDays == 0)
        {
            examDate.text = "D-Day";

            foldOut.UnlockAll(false);

            testButton.gameObject.SetActive(true);
            testButton.onClick.AddListener(StartTest);
            testButton.interactable = true;
        }
        else
        {
            examDate.text = $"D-{schoolDays}";
            if (schoolDays > 0) examDate.text = $"D-{schoolDays}";
            else examDate.text = "-";
        }

        idolDate.text = idolDays >= 0 ? $"컴백까지\nD-{idolDays}" : "-";
    }
    #endregion

    #region Test
    public SeasonType GetCurrentSeason()
    {
        DateTime baseDate = new DateTime(2026, 3, 2);
        DateTime targetDate = baseDate.AddDays(dateIndex);

        switch (targetDate.Month)
        {
            case 3:
            case 4:
            case 5:
                return SeasonType.Spring;
            case 6:
            case 7:
            case 8:
                return SeasonType.Summer;
            case 9:
            case 10:
            case 11:
                return SeasonType.Autumn;
            case 12:
            case 1:
            case 2:
                return SeasonType.Winter;
            default:
                return SeasonType.None;
        }
    }
    private void StartTest()
    {
        SeasonType currentSeason = GetCurrentSeason();

        if (testController != null)
        {
            testController.StartTestSequence(currentSeason);
        }
    }
    private void HandleAfterTest()
    {
        foldOut.UnlockAll(true);
        testButton.gameObject.SetActive(false);

        int currentIdx = UserDataManager.Instance.GetDateIndex();

        for (int i = 0; i < 4; i++) //강제로 시간변경
        {
            UserDataManager.Instance.AdvanceDateIndex();
        }
        UserDataManager.Instance.SetIsWeekend(true);
        UpdateWeekPanel();
        UpdateDate();
        UpdateEventDates();

        ScheduleManager.Instance.isTest = false;
    }
    #endregion
}

[System.Serializable]
public class ScheduleItem
{
    public TextMeshProUGUI dateText;
    public ScheduleSelector scheduleSelector;
    public ItemSelector itemSelector;

    public void ResetData()
    {
        scheduleSelector.ResetSchedule();
        itemSelector.ResetItem();
    }
}