using UnityEngine;
using UnityEngine.UI;

public class SelectionUnitUI : MonoBehaviour
{
    private SelectionUI selectionUI;
    public int ID;
    [SerializeField] private Button showInfoButton;

    public void Init(int ID, SelectionUI selectionUI)
    {
        this.selectionUI = selectionUI;
        this.ID = ID;
        showInfoButton.onClick.AddListener(() => selectionUI.ShowInfo(ID));
    }
}
