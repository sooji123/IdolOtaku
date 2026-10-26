using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestChoiceDialogue : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Sprite selection_press;
    [SerializeField] private Sprite selection_default;
    [SerializeField] private List<Button> buttons = new();
    [SerializeField] private List<TextMeshProUGUI> buttonTexts = new();

    private TestController controller;

    public void Initialize(TestController testController)
    {
        controller = testController;

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnChoiceClicked(index));
        }
    }

    public void Show(string[] options, string title)
    {
        // 모든 버튼 숨기기
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        // 옵션 수만큼 버튼 표시
        for (int i = 0; i < options.Length && i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            if (i < buttonTexts.Count)
            {
                buttons[i].image.sprite = selection_default;
                buttons[i].interactable = true;
                buttonTexts[i].text = options[i];
            }
        }

        // 타이틀 설정
        if (title != null)
        {
            titleText.text = title;
            titlePanel.SetActive(true);
        }
        else { titlePanel.SetActive(false); }

        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    private void OnChoiceClicked(int index)
    {
        foreach (var button in buttons)
        {
            button.interactable = false;
        }

        buttons[index].interactable = true;
        buttons[index].image.sprite = selection_press;

        controller.OnChoiceButtonClicked(index);
    }
}
