using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private MainMenuDesignSO mainMenuDesignSO;

    [Header("UI Elements")]
    [SerializeField] private Image bg;
    [SerializeField] private Image btn;
    [SerializeField] private Image gallery;
    
    #region Built in Events
    private void Start() => AdjustElements();
    #endregion

    private void AdjustElements()
    {
        MainMenuDesignElement data = GetElementByType(UserDataManager.Instance.GetSelectedIdol());

        bg.sprite = data.bg;
        btn.sprite = data.btn;
        gallery.sprite = data.gallery;
    }

    #region Utils
    private MainMenuDesignElement GetElementByType(MemberType idolType)
    {
        foreach (var data in mainMenuDesignSO.elements)
            if (data.type == idolType)
                return data;
        
        Debug.LogWarning($"[MainMenuUI] IdolType : {idolType} cannot found");
        return null;
    }
    #endregion
}
