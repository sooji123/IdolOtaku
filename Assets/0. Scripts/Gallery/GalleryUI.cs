using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;

public class GalleryUI : MonoBehaviour, IWindowUI
{
    #region Properties
    public GameObject ui;
    public GalleryDetailsUI detailsPanel;
    public GameObject previewPanel;
    public GameObject galleryUnit;
    #endregion
    
    public List<GalleryUnitSO> galleryUnits = new List<GalleryUnitSO>();

    private void Start() { Init(); }

    private void Init()
    {
        detailsPanel.HidePanel();

        foreach (Transform child in previewPanel.transform)
            Destroy(child.gameObject);

        foreach (var unit in galleryUnits)
        {
            if (DataManager.Instance.unlockedGalleryKeys.Contains(unit.id))
            {
                unit.show = true;
            }
            AddUnit(unit);
        }
    }

    #region Default Interfaces
    [Button]
    public void AddUnit(GalleryUnitSO data)
    {
        if (data.show)
        {
            GameObject go = Instantiate(galleryUnit, previewPanel.transform);
            GalleryUnit unit = go.GetComponent<GalleryUnit>();
            unit.Init(this, data);
        }
    }

    #endregion

    #region Details Panel
    private void ShowDetails(GalleryUnitSO data)
    {
        detailsPanel.ShowPanel(data);
    }

    private void HideDetails()
    {
        detailsPanel.Hide();
    }
    #endregion

    #region Callee Events
    public void OnGalleryUnitClicked(GalleryUnitSO data)
    {
        ShowDetails(data);
    }
    #endregion

    #region UIWindow Function
    public void Hide() { ui.SetActive(false); }

    public void Show() { ui.SetActive(true); Init(); }
    #endregion
}