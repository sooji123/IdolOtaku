using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;

public class MessageUnitOppChat : MonoBehaviour
{
    #region Properties
    private MessageUI ui;
    public ChatType bubbleMessageType;
    public GameObject      maskUnit;
    public Image           portraitImage;
    public TextMeshProUGUI messageText;

    public List<RectTransform> csf = new List<RectTransform>();
    #endregion

    public void Init(MessageUI ui, Sprite portrait, string text)
    {
        this.ui = ui;
        bubbleMessageType = ChatType.OppChat;
        this.portraitImage.sprite = portrait;
        messageText.text = text;

        UpdateCSF();
    }

    public void HideChatWait()
    {
        csf[1].gameObject.SetActive(true);
        csf[3].gameObject.SetActive(false);
    }


    public void HidePortrait()
    {
        portraitImage.color = new Color(0, 0, 0, 0);
        maskUnit.SetActive(false);
    }

    private void UpdateCSF()
    {
        for (int i = 0; i < csf.Count; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(csf[i]);
    }

}
