using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoldOutButtonController : MonoBehaviour
{
    [SerializeField] List<FoldOutButton> buttons = new List<FoldOutButton>();
    [SerializeField] ItemController itemController;
    private int lastIndex;

    #region Show/Hide
    public void Show(int index)
    {
        if (index < 0 || index >= buttons.Count)
            return;

        if (buttons[index].isLocked) return;

        lastIndex = index;
        HideAll();
        buttons[index].Show();
        itemController.SetItemPanelByIndex(index);

        AudioManager.Instance.PlayClick();
    }

    public void HideAll()
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].Hide();
    }

    public void ShowLast()
    {
        Show(lastIndex);
    }

    public void SetItemPanelByIndex(int index)
    {
        itemController.SetItemPanelByIndex(index);
    }

    public void RefreshItemQuantity()
    {
        itemController.RefreshAllQuantity();
    }
    #endregion

    #region Lock/Unlock

    public void Lock(int index)
    {
        if (index < 0 || index >= buttons.Count)
            return;

        buttons[index].Lock();
    }

    public void Unlock(int index)
    {
        if (index < 0 || index >= buttons.Count)
            return;

        buttons[index].Unlock();
    }

    public void UnlockAll(bool isUnlock)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (isUnlock) buttons[i].Unlock();
            else buttons[i].Lock();
        }
        if (!isUnlock) HideAll();
    }
    #endregion
}

[Serializable]
public class FoldOutButton
{
    [SerializeField] Image image;
    [SerializeField, PreviewField] Sprite selectedSprite;
    [SerializeField, PreviewField] Sprite unselectedSprite;
    [SerializeField] GameObject panel;
    [SerializeField] Image lockImage;
    [SerializeField] public bool isLocked = false;
    
    #region Show/Hide
    public void Show()
    {
        if (isLocked) return;
        image.sprite = selectedSprite;
        panel.SetActive(true);
    }

    public void Hide()
    {
        image.sprite = unselectedSprite;
        panel.SetActive(false);
    }
    #endregion
    
    #region Lock/Unlock
    public void Lock()
    {
        isLocked = true;
        lockImage.gameObject.SetActive(true);
    }

    public void Unlock()
    {
        isLocked = false;
        lockImage.gameObject.SetActive(false);
    }

    #endregion
}