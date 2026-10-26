using System.Collections.Generic;
using UnityEngine;

public class ProfileUI : MonoBehaviour, IWindowUI
{
    public BubbleUI bubble;

    public GameObject UI;

    public ProfileDetailsUI profileDetails;

    public List<CharacterSO> characters = new List<CharacterSO>();
    public Transform profileParent;
    public GameObject profileUnit;
    public List<ProfileUnitUI> profileUnits = new List<ProfileUnitUI>();

    private CharacterSO currentCharacter = null;

    public void Init()
    {
        SetProfiles();
    }

    #region Set
    public void ResetProfiles()
    {
        for (int i = 0; i < profileParent.childCount; i++)
            Destroy(profileParent.GetChild(profileParent.childCount - i - 1).gameObject);
    }

    public void SetProfiles()
    {
        ResetProfiles();

        for (int i = 0; i < characters.Count; i++)
            SetProfile(i, characters[i]);
    }

    public void SetProfile(int index, CharacterSO so)
    {
        GameObject go = Instantiate(profileUnit, profileParent);
        ProfileUnitUI co = go.GetComponent<ProfileUnitUI>();
        co.Init(this, index, so);
        profileUnits.Add(co);
    }
    #endregion

    #region Update
    public void UpdateAllFavorability()
    {
        for (int i = 0; i < profileUnits.Count; i++)
            profileUnits[i].UpdateFavorability();
    }

    public void UpdateFavorability(int ID)
    {
        for (int i = 0; i < profileUnits.Count; i++)
            if (profileUnits[i].IsSameCharacter(ID))
                profileUnits[i].UpdateFavorability();
    }
    #endregion

    #region Callee Event
    public void OnProfileUnitClicked(CharacterSO character)
    {
        currentCharacter = character;
        profileDetails.SetProfileDetailsToCharacter(character);
    }

    public void OnProfileMessageClicked()
    {
        profileDetails.Hide();
        bubble.ShowCharacterMessagePanel(currentCharacter);
    }
    #endregion

    #region IWindowUI
    public void Hide()
    {
        UI.SetActive(false);
    }

    public void Show()
    {
        UI.SetActive(true);
        UpdateAllFavorability();
    }
    #endregion
}
