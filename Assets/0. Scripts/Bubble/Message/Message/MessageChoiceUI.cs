using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageChoiceUI : MonoBehaviour, IWindowUI
{
    public GameObject UI;
    private MessageData data;
    public MessageUI messageUI;

    public RectTransform choicePanel;
    public GameObject unitChoice;
    public List<RectTransform> csf = new List<RectTransform>();

    public void Init(MessageData data)
    {
        Clear();
        Show();

        Debug.Log("@ Choice Panel ON");
        
        this.data = data;
        List<string> choices = data.choices;

        for (int i = 0; i < choices.Count; i++)
        {
            GameObject go = Instantiate(unitChoice, choicePanel);
            go.GetComponent<MessageUnitChoice>().Init(this, data.choiceID, i, choices[i], data.branchID);
        }

        messageUI.OnChoiceUnitSetup((int)choicePanel.rect.height);
        UpdateCSF();
    }

    public void Clear()
    {
        data = null;
        for (int i = 0; i < choicePanel.childCount; i++)
            Destroy(choicePanel.GetChild(choicePanel.childCount - 1 - i).gameObject);
    }

    public void UpdateCSF()
    {
        for (int i = 0; i < csf.Count; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(csf[i]);

        for (int i = 0; i < choicePanel.childCount; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)choicePanel.GetChild(choicePanel.childCount - 1 - i).transform);

        for (int i = 0; i < csf.Count; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(csf[i]);
    }

    #region Callee Event
    public void OnChoiceUnitClicked(int choiceID, int branchID)
    {
        Debug.Log($"@ Choice 선택됌! {choiceID}");
        messageUI.OnChoice(data, choiceID, branchID);
        
        Debug.Log("@ Hide : OnChoiceUnitClicked");
        Hide(); 
    }

    public void Show()
    {
        UI.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log("@ Hide 호출됌!");
        UI.SetActive(false);
    }
    #endregion
}
