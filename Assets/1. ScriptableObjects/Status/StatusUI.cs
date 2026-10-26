using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Table;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private StatusSO StatusSO;
    
    [SerializeField] private Slider HP;
    [SerializeField] private Image Mental;

    [SerializeField] private TextMeshProUGUI Study;
    [SerializeField] private TextMeshProUGUI Fanship;
    [SerializeField] private TextMeshProUGUI Money;

    [Header("Mental Resource")]
    [SerializeField] private List<Sprite> mentalSprites = new();

    private StatGradeTableSO table;

    private void Start()
    {
        table = TableManager.Instance.GetTable<StatGradeTableSO>();

        UpdateStatusUI();
    }

    [Button]
    public void UpdateStatusUI()
    {
        Status status = StatusSO.status;
        
        HP.value = GetHPPersentage(status);
        Mental.sprite = GetMentalSprite(status);
        Study.text = GetStudyText(status);
        Fanship.text = GetFanshipText(status);
        Money.text = GetMoneyText(status);
    }

    private float GetHPPersentage(Status status)
    {
        return (status.MaxHP - status.HP) / (float)status.MaxHP;
    }

    private Sprite GetMentalSprite(Status status)
    {
        return mentalSprites[(int)status.mentalType];
    }

    private string GetStudyText(Status status)
    {
        return $"<size=45>{table.GetGrade(StatusType.Study, status.Study)}</size> <size=30>{status.Study}/{status.MaxStudy}</size>";
    }
    
    private string GetFanshipText(Status status)
    {
        // TODO : Favorability랑 Fanship 혼용 중. 이건 좀 문제가 있삼.
        return $"<size=45>{table.GetGrade(StatusType.Favorability, status.Fanship)}</size> <size=30>{status.Fanship}/{status.MaxFanship}</size>";
    }
    
    private string GetMoneyText(Status status)
    {
        return $"{status.Money}";
    }
}
