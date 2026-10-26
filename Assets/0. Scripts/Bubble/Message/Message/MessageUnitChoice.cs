using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class MessageUnitChoice : MonoBehaviour, IPointerClickHandler
{
    private MessageChoiceUI ui;
    private int index;
    private int branchID;
    public TextMeshProUGUI messageText;
    public List<RectTransform> csf = new List<RectTransform>();

    public bool isCool = true;

    private void Start()
    {
        StartCoroutine(Cooldown());
    }

    public void Init(MessageChoiceUI ui, int choiceID, int index, string text, int branchID)
    { 
        this.ui = ui;
        this.index = index;
        this.branchID = branchID;
        this.messageText.text = DataManager.Instance.GetDialogueString(text);


        UpdateCSF();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCool) return;
        
        ui.OnChoiceUnitClicked(index, branchID);
        isCool = true;
    }

    private void UpdateCSF()
    {
        for (int i = 0; i < csf.Count; i++)
            if (csf[i] != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(csf[i]);
    }

    private IEnumerator Cooldown()
    {
        isCool = true; Debug.Log($"@ Cooldown {isCool}");
        yield return new WaitForSeconds(0.5f);
        isCool = false;Debug.Log($"@ Cooldown {isCool}");
    }
}
