using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestMessageDialogue : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI messageText;

    private TestController controller;

    public void Initialize(TestController testController)
    {
        controller = testController;
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
