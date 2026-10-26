using System;
using TableData;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CalendarInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private Image banner;
    [SerializeField] private Sprite idolBanner;
    [SerializeField] private Sprite schoolBanner;
    [SerializeField] private TextMeshProUGUI eventName;
    [SerializeField] private TextMeshProUGUI dateName;
    [SerializeField] private Button closeButton;

    private static readonly string[] DayOfWeekKor = { "일", "월", "화", "수", "목", "금", "토" };

    private void Start()
    {
        closeButton.onClick.AddListener(() => Hide());
    }

    public void Show(calendar data)
    {
        int month = data.month;
        int day = data.day;
        int weekIndex = data.week switch
        {
            "mon" => 1, "tue" => 2, "wed" => 3, "thu" => 4,
            "fri" => 5, "sat" => 6, "sun" => 0,
            _ => 0
        };
        dateName.text = $"{month}월 {day}일 ({DayOfWeekKor[weekIndex]})";
        eventName.text = LocalizationManager.Instance.GetString(data.ID);

        switch (data.event_type)
        {
            case EventType.idol:
                banner.sprite =  idolBanner;
                break;
            case EventType.school:
            case EventType.school_test:
                banner.sprite = schoolBanner;
                break;
        }
        
        UI.SetActive(true);
    }

    public void Hide() => UI.SetActive(false);
}
