using Sirenix.OdinInspector;
using Table;
using TableSO.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public partial class UserDataManager
{
    #region Save/Load

    private void SavePlayerData()
    {
        Save<SavablePlayerData>(playerData, playerFileName);
    }

    private void LoadPlayerData()
    {
        var data = Load<SavablePlayerData>(playerFileName);
        if (data != null) playerData = data;
    }

    #endregion
    
    [SerializeField] private SavablePlayerData playerData;

    #region isSelected
    public void SetIsSelected(bool isSelected) => playerData.isSelected = isSelected;
    public bool GetIsSelected() => playerData.isSelected;
    #endregion
    
    #region Idol
    public void SetSelectedIdol(MemberType idol) => playerData.selectedIdol = idol;
    public MemberType GetSelectedIdol() => playerData.selectedIdol;
    #endregion
    
    #region Favorability
    public void SetFavorability(int value) => playerData.favorability = value;
    public int GetFavorability() => playerData.favorability;
    public int ModifyFavorability(int value) => playerData.favorability += value;
    #endregion

    public void IdolFavorabilityChange(MemberType idol, int value)
    {
        SetSelectedIdol(idol);
        SetFavorability(value);
    }
}

[System.Serializable]
public class SavablePlayerData
{
    public bool isSelected = false;
    public MemberType selectedIdol;
    public int favorability = 0;
}
