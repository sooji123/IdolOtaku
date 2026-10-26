using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
//using static System.Net.Mime.MediaTypeNames;

public class MessageUnitMe : MonoBehaviour
{
    #region Properties
    private MessageUI ui;
    public ChatType messageType;
    public TextMeshProUGUI messageText;

    public List<RectTransform> csf = new List<RectTransform>();
    #endregion

    public void Init(MessageUI ui, Sprite portrait)
    {
        this.ui = ui;
        messageType = ChatType.Me;

        csf[3].gameObject.SetActive(true);
        csf[1].gameObject.SetActive(false);

        UpdateCSF();
    }

    public void SetMessage(string text) {
        csf[3].gameObject.SetActive(false);
        csf[1].gameObject.SetActive(true);

        messageText.text = text;
    }


    private void UpdateCSF()
    {
        for (int i = 0; i < csf.Count; i++)
            LayoutRebuilder.ForceRebuildLayoutImmediate(csf[i]);
    }
}
