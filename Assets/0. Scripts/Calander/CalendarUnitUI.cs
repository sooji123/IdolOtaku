using TableData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarUnitUI : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private Button infoBtn;
    [SerializeField] private GameObject contour;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private GameObject eventObj;
    [SerializeField] private Image eventImage;
    [SerializeField] private TextMeshProUGUI eventName;

    [SerializeField] private Sprite idolEvent;
    [SerializeField] private Sprite schoolEvent;

    private calendar data;
    private CalendarInfoUI infoUI;

    public void Init(CalendarInfoUI infoUI)
    {
        this.infoUI = infoUI;
        infoBtn.onClick.AddListener(OnClickInfo);
    }

    public void Load(int day, calendar calendarData = null)
    {
        data = calendarData;
        dayText.text = day.ToString();

        if (calendarData != null)
        {
            eventObj.SetActive(true);

            switch (calendarData.event_type)
            {
                case EventType.idol:
                    eventImage.sprite = idolEvent;
                    break;
                case EventType.school:
                case EventType.school_test:
                    eventImage.sprite = schoolEvent;
                    break;
            }

            eventName.text = LocalizationManager.Instance.GetString(calendarData.ID);
            infoBtn.interactable = true;
        }
        else
        {
            eventObj.SetActive(false);
            infoBtn.interactable = false;
        }

        Show();
    }

    private void OnClickInfo()
    {
        if (data == null) return;
        infoUI.Show(data);
    }

    public void SetContour(bool isCurrentDay) => contour.SetActive(isCurrentDay);
    public void Show() => UI.SetActive(true);
    public void Hide() => UI.SetActive(false);
}
