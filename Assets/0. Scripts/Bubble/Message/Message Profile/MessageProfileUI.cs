using System.Collections.Generic;
using UnityEngine;

public class MessageProfileUI : MonoBehaviour, IWindowUI
{
    public BubbleUI bubble;
    public MessageUI message;

    public GameObject UI;

    public List<CharacterSO> characters = new List<CharacterSO>();
    public Transform messageProfileParent;
    public GameObject messageProfileUnit;
    public List<MessageProfileUnitUI> messageProfileUnits = new List<MessageProfileUnitUI>();

    private CharacterSO currentCharacter = null;

    public void Init()
    {
        SetProfiles();
    }

    #region Update
    public void UpdateAllLastMessage()
    {
        for (int i = 0; i < messageProfileUnits.Count; i++)
            messageProfileUnits[i].UpdateLastMessage();
    }

    public void UpdateLastMessage(int ID)
    {
        for (int i = 0; i < messageProfileUnits.Count; i++)
            if (messageProfileUnits[i].IsSameCharacter(ID))
                messageProfileUnits[i].UpdateLastMessage();
    }

    public void UpdateAllAlarm()
    {
        for (int i = 0; i < messageProfileUnits.Count; i++)
            messageProfileUnits[i].UpdateAlarm();
    }

    public void UpdateAlarm(int ID)
    {
        for (int i = 0; i < messageProfileUnits.Count; i++)
            if (messageProfileUnits[i].IsSameCharacter(ID))
                messageProfileUnits[i].UpdateAlarm();
    }
    public void UpdateAllFavorability()
    {
        for (int i = 0; i < messageProfileUnits.Count; i++)
            messageProfileUnits[i].UpdateFavorability();
    }

    public void UpdateFavorability(int ID)
    {
        for (int i = 0; i < messageProfileUnits.Count; i++)
            if (messageProfileUnits[i].IsSameCharacter(ID))
                messageProfileUnits[i].UpdateFavorability();
    }
    
    #endregion

    #region Set
    public void ResetProfiles()
    {
        for (int i = 0; i < messageProfileParent.childCount; i++)
            Destroy(messageProfileParent.GetChild(messageProfileParent.childCount - i - 1).gameObject);
    }

    public void SetProfiles()
    {
        ResetProfiles();

        for (int i = 0; i < characters.Count; i++)
            SetProfile(i, characters[i]);
    }

    public void SetProfile(int index, CharacterSO so)
    {
        GameObject go = Instantiate(messageProfileUnit, messageProfileParent);
        MessageProfileUnitUI co = go.GetComponent<MessageProfileUnitUI>();
        co.Init(this, index, so);
        messageProfileUnits.Add(co);
    }
    #endregion

    #region Callee Event
    public void OnMessageProfileUnitClicked(CharacterSO character)
    {
        currentCharacter = character;

        message.SetMessagePanel(character);
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
        UpdateAllAlarm();
        UpdateAllLastMessage();
        UpdateAllFavorability();
    }
    #endregion
}
