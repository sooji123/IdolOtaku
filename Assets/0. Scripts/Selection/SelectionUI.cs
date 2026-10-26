using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject infoUI;
    [SerializeField] private Image infoImage;
    
    [Header("Content")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private Button startButton;
    [SerializeField] private Button infoButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    
    [Header("Data")]
    [SerializeField] private List<SelectionUnitUI> selectionUnits = new();
    [SerializeField] private SelectionUnitUI selectedUnit;

    [Header("Scroll")]
    [SerializeField] private SimpleScrollSnap scroll;

    [Header("Info")]
    [SerializeField] private Sprite cyw;
    [SerializeField] private Sprite kwj;
    
    /// <summary>
    /// 추후 데이터 기반으로 변경해야 함.
    /// 지금 패널 부분을 인간이 작업해야 하는 문제 존재.
    /// </summary>
    
    #region Built In Events
    private void Start()
    {
        Init();
        
        startButton.onClick.AddListener(StartGame);
        infoButton.onClick.AddListener(HideInfo);
        
        scroll.OnPanelCentered.AddListener(OnPanelCetered);
    }
    #endregion

    private void Init()
    {
        selectionUnits = new List<SelectionUnitUI>(contentParent.GetComponentsInChildren<SelectionUnitUI>());
        
        for (int i = 0; i < selectionUnits.Count; i++)
            selectionUnits[i].Init(i, this);
        
        selectedUnit = selectionUnits[0];
    }

    private void StartGame()
    {
        UserDataManager.Instance.SetSelectedIdol((MemberType)selectedUnit.ID + 1);
        
        SceneManager.LoadScene("GameScene");
    }
    
    #region Info

    public void ShowInfo(int index)
    {
        infoUI.SetActive(true);

        infoImage.sprite = index == 0 ? kwj : cyw;
    }

    private void HideInfo()
    {
        infoUI.SetActive(false);
        
        infoImage.sprite = null;
    }

    #endregion
    
    #region Events
    private void OnPanelCetered(int _new, int _old)
    {
        selectedUnit = selectionUnits[_new];

        prevButton.gameObject.SetActive(_new != 0);
        nextButton.gameObject.SetActive(_new != selectionUnits.Count - 1);
    }
    #endregion
}
