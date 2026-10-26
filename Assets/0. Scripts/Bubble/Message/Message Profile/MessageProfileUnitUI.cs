using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MessageProfileUnitUI : MonoBehaviour, IPointerClickHandler
{
    private MessageProfileUI messageProfileUI;
    private int index;
    private CharacterSO character;

    public Image portrait;
    public TextMeshProUGUI name;
    public TextMeshProUGUI lastMessage;
    public TextMeshProUGUI relationship;
    public Image messageAlarm;

    public void Init(MessageProfileUI messageProfileUI, int index, CharacterSO character)
    {
        this.messageProfileUI = messageProfileUI;
        this.index = index;
        this.character = character;

        SetUnit(character);
        UpdateLastMessage();
        UpdateFavorability();
        UpdateAlarm();
    }

    public void SetUnit(CharacterSO character)
    {
        this.character = character;

        portrait.sprite = character.portrait;
        name.text = character.characterName;
        lastMessage.text = "Last Message";
        relationship.text = character.favorability.ToString();
    }
    #region Update
    public void UpdateLastMessage()
    {
        lastMessage.text = DataManager.Instance.GetLastMessage(character.ID);
    }

    public void UpdateAlarm() {
        if (DataManager.Instance.GetMessageAlarm(character.ID))
        {
            messageAlarm.gameObject.SetActive(true);
        }
        else {
            messageAlarm.gameObject.SetActive(false);
        }
    }
    public void UpdateFavorability()
    {
        relationship.text = character.favorability.ToString();
    }
    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        messageProfileUI.OnMessageProfileUnitClicked(character);
    }

    public bool IsSameCharacter(int ID)
    {
        return this.character.ID == ID;
    }
}
