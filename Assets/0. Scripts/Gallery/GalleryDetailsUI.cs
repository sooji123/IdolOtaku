using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GalleryDetailsUI : MonoBehaviour, IWindowUI, IPointerClickHandler
{
    #region Properties
    public GameObject ui;
    public Image detailImage;
    #endregion

    #region Panel Controll
    public void ShowPanel(GalleryUnitSO data)
    {
        detailImage.sprite = data.detail;
        detailImage.SetNativeSize();
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