using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleEventMessageDialogue : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI messageText;

    private ScheduleEventController controller;

    public void Initialize(ScheduleEventController eventController)
    {
        controller = eventController;
        confirmButton.onClick.AddListener(OnClick);
    }

    public void Show(string message)
    {
        messageText.text = message;
        panel.SetActive(true);
    }


    public void Hide()
    {
        panel.SetActive(false);
    }

    private void OnClick()
    {
        controller.OnConfirmButtonClicked();
    }
}
