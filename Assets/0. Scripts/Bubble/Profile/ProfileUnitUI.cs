using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfileUnitUI : MonoBehaviour, IPointerClickHandler
{
    private ProfileUI profileUI;
    private int index;
    private CharacterSO character;

    public Image portrait;
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI stateMessage;
    public TextMeshProUGUI relationship;

    public void Init(ProfileUI profileUI, int index, CharacterSO character)
    { 
        this.profileUI = profileUI;
        this.index = index;
        this.character = character;

        SetUnit(character);
    }

    public void SetUnit(CharacterSO character)
    { 
        this.character = character;

        if (character.portrait != null)
            portrait.sprite = character.portrait;
        else
            Debug.LogError("Charcter Portrait 찾을 수 없음.");
        nameField.text = character.characterName;
        stateMessage.text = "Hello World";
        relationship.text = character.favorability.ToString();
    }

    #region Update
    public void UpdateFavorability()
    {
        relationship.text = character.favorability.ToString();
    }
    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        profileUI.OnProfileUnitClicked(character);
    }
    public bool IsSameCharacter(int ID)
    {
        return this.character.ID == ID;
    }
}
