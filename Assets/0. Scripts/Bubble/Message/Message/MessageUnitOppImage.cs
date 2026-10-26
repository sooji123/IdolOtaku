using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MessageUnitOppImage : MonoBehaviour, IPointerClickHandler
{
    #region Properties
    private MessageUI ui;
    public ChatType bubbleMessageType;
    public GameObject      maskUnit;
    public Image           portraitImage;
    public Image           messageImage;

    public List<RectTransform> csf = new List<RectTransform>();
    #endregion

    public void Init(MessageUI ui, Sprite portrait, Sprite sprite)
    {
        this.ui = ui;
        bubbleMessageType = ChatType.OppImage;
        this.portraitImage.sprite = portrait;
        messageImage.sprite = sprite;

        UpdateCSF();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        ui.OnOppImageUnitClicked(messageImage.sprite);
    }
}
