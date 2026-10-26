using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BubbleUI : MonoBehaviour, IWindowUI
{
    public GameObject UI;

    #region Properties - ToggleButton
    [BoxGroup("ToggleButton")] public Button profileButton;
    [BoxGroup("ToggleButton")] public Button messageButton;
    #endregion

    [BoxGroup("Profile")] public ProfileUI profile;

    #region Properties - Message Profile
    [BoxGroup("Message Profile")] public MessageProfileUI messageProfile;
    #endregion

    #region Life Cycle
    private void Start()
    {
        profile.Init();
        messageProfile.Init();
    }

    private void OnEnable()
    {
        OnClickProfileButton();
    }
    #endregion

    public void ShowCharacterMessagePanel(CharacterSO currentCharacter)
    {
        OnClickMessageButton();
        messageProfile.OnMessageProfileUnitClicked(currentCharacter);
    }

    #region Panel Control
    public void ShowProfilePanel()
    {
        profile.Show();

        /// Do Something

        messageProfile.Hide();
    }

    public void ShowMessagePanel()
    {
        messageProfile.Show();

        /// Do Something

        profile.Hide();
    }
    #endregion

    #region Toggle Button
    public void OnClickProfileButton()
    {
        profileButton.interactable = false;
        messageButton.interactable = true;

        ShowProfilePanel();
    }

    public void OnClickMessageButton()
    {
        messageButton.interactable = false;
        profileButton.interactable = true;

        ShowMessagePanel();
    }
    #endregion

    #region IWindowUI
    public void Hide()
    {
        UI.SetActive(false);
    }

    public void Show()
    {
        OnClickMessageButton();
        OnClickProfileButton();
    }
    #endregion
}