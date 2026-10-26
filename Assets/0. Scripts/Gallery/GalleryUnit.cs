using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GalleryUnit : MonoBehaviour, IPointerClickHandler
{
    #region Properties
    private GalleryUI ui;
    private GalleryUnitSO data;
    public Image previewImage;
    #endregion

    public void Init(GalleryUI ui, GalleryUnitSO data)
    {
        this.ui = ui;
        this.data = data;
        this.previewImage.sprite = data.preview;
    }

    #region Implements
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ui == null || data == null) return;
        ui.OnGalleryUnitClicked(data);
    }
    #endregion
}
