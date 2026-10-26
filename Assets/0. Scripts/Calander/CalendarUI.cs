using System;
using System.Collections.Generic;
using System.Linq;
using Table;
using TableData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CalendarUI : MonoBehaviour, IWindowUI
{
    [SerializeField] private GameObject UI;

    [Header("Button")]
    [SerializeField] private Button prevBtn;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button exitBtn;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI yearMonth;

    [Header("Elements")]
    [SerializeField] private List<CalendarUnitUI> units = new();

    [SerializeField] private CalendarInfoUI infoUI;

    private int current; // Min 기준 월 오프셋 (0 = MinYear/MinMonth)
    private calendarTableSO calendarTable;

    private const int MinYear = 2026, MinMonth = 3;
    private const int MaxYear = 2029, MaxMonth = 12;
    private const int MaxIndex = (MaxYear - MinYear) * 12 + (MaxMonth - MinMonth);

    private int CurrentYear => MinYear + (MinMonth - 1 + current) / 12;
    private int CurrentMonth => (MinMonth - 1 + current) % 12 + 1;
    
    #region Built in Events

    private void Start()
    {
        calendarTable = TableManager.Instance.GetTable<calendarTableSO>();
        foreach (var unit in units)
            unit.Init(infoUI);
        prevBtn.onClick.AddListener(Prev);
        nextBtn.onClick.AddListener(Next);
        exitBtn.onClick.AddListener(Exit);
    }

    #endregion
    
    #region Visual

    private void UpdateButtonVisual()
    {
        prevBtn.interactable = current > 0;
        nextBtn.interactable = current < MaxIndex;
    }
    #endregion
    
    #region Button Control

    private void Prev()
    {
        current--;
        LoadByYearAndMonth(CurrentYear, CurrentMonth);
        UpdateButtonVisual();
    }

    private void Next()
    {
        current++;
        LoadByYearAndMonth(CurrentYear, CurrentMonth);
        UpdateButtonVisual();
    }

    private void Exit()
    {
        Hide();
    }

    #endregion

    #region IWindowUI implementation
    [Button]
    public void Show()
    {
        UI.SetActive(true);
        NavigateToCurrentDate();
    }

    private void NavigateToCurrentDate()
    {
        DateTime baseDate = new DateTime(MinYear, MinMonth, 1);
        DateTime currentDate = new DateTime(2026, 3, 2).AddDays(UserDataManager.Instance.GetDateIndex());

        int monthOffset = (currentDate.Year - MinYear) * 12 + (currentDate.Month - MinMonth);
        monthOffset = Mathf.Clamp(monthOffset, 0, MaxIndex);

        current = monthOffset;
        LoadByYearAndMonth(CurrentYear, CurrentMonth);
        UpdateButtonVisual();
    }

    public void Hide()
    {
        UI.SetActive(false);
    }
    #endregion
    
    [Button]
    private void LoadByYearAndMonth(int year, int month)
    {
        current = (year - MinYear) * 12 + (month - MinMonth);
        yearMonth.text = $"입덕 {year - 2025}년차<size=128>{month}</size>월";

        var firstDay = new DateTime(year, month, 1);
        int startDayOfWeek = (int)firstDay.DayOfWeek; // Sunday=0
        int daysInMonth = DateTime.DaysInMonth(year, month);

        DateTime currentDate = new DateTime(2026, 3, 2).AddDays(UserDataManager.Instance.GetDateIndex());
        bool isWeekend = UserDataManager.Instance.GetIsWeekend();

        // 현재일 기준 해당 주의 하이라이트 범위 계산
        // 주일(평일): 월~금, 주말: 토~일
        DateTime weekStart, weekEnd;
        if (!isWeekend)
        {
            // 월요일까지 되돌아가기
            int daysFromMon = ((int)currentDate.DayOfWeek - 1 + 7) % 7;
            weekStart = currentDate.AddDays(-daysFromMon);
            weekEnd = weekStart.AddDays(4); // 금요일
        }
        else
        {
            // 토요일까지 되돌아가기
            int daysFromSat = ((int)currentDate.DayOfWeek - 6 + 7) % 7;
            weekStart = currentDate.AddDays(-daysFromSat);
            weekEnd = weekStart.AddDays(1); // 일요일
        }

        // year=1 -> 2026, so tableYear = year - 2025
        int tableYear = year - 2025;
        var monthEvents = calendarTable.dataList
            .Where(e => e.year == tableYear && e.month == month)
            .ToDictionary(e => e.day);

        for (int i = 0; i < units.Count; i++)
        {
            int day = i - startDayOfWeek + 1;

            if (day < 1 || day > daysInMonth)
            {
                units[i].Hide();
            }
            else
            {
                monthEvents.TryGetValue(day, out calendar calendarData);
                units[i].Load(day, calendarData);
                var date = new DateTime(year, month, day);
                bool highlight = date >= weekStart && date <= weekEnd;
                units[i].SetContour(highlight);
            }
        }
    }
}
