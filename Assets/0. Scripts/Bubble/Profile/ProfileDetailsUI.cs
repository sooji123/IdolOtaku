using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class ProfileDetailsUI : MonoBehaviour, IWindowUI
{
    public GameObject UI;

    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI stateMessage;
    [SerializeField] private TextMeshProUGUI relationship;

    public void SetProfileDetailsToCharacter(CharacterSO character)
    {
        SetProfileDetails(character);

        Show();
    }

    private void SetProfileDetails(CharacterSO character)
    {
        portrait.sprite = character.portrait;
        name.text = character.characterName;
        stateMessage.text = "Hello World";
        relationship.text = character.favorability.ToString();
    }

    #region IWindowUI
    public void Hide()
    {
        UI.SetActive(false);
    }

    public void Show()
    {
        UI.SetActive(true);
    }
    #endregion
}
