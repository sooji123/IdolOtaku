using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MessageDetailsUI : MonoBehaviour, IWindowUI, IPointerClickHandler
{
    #region Properties
    public GameObject ui;
    public Image detailImage;
    #endregion

    #region Panel Controll
    public void ShowPanel(Sprite data)
    {
        detailImage.sprite = data;
        Show();
    }

    public void HidePanel()
    {
        Hide();
    }
    #endregion

    #region UIWindow Functions
    public void Hide()
    {
        detailImage.gameObject.SetActive(false);
    }

    public void Show()
    {
        detailImage.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        HidePanel();
    }
    #endregion
}
