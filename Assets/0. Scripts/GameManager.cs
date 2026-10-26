using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public List<GameObject> UIs = new();
    public StatusSO status;

    private void Awake()
    {
    }

    private void Start()
    {
        OpenUI(UIType.MainMenu);
    }
    
    public void OpenUI(UIType type)
    {
        CloseAllUIs();
        UIs[(int)type].SetActive(true);
    }

    public void OpenUI(int i)
    {        
        CloseAllUIs();
        UIs[i].SetActive(true);
    }

    /// <summary>
    /// 초반 UI 구조 잘못 잡은듯?
    /// 리펙토링 비용은 꽤 비쌀듯
    /// 추후 해상도 조정할 때 같이 잡자.
    /// </summary>
    public UIType GetCurrentUIType()
    {
        if (UIs[3].activeSelf)
        {
            if (ScheduleManager.Instance.isScheduleProcessing) return UIType.ScheduleProcessing;
            else return UIType.Schedule;
        }
        else
        {
            return UIType.MainMenu;
        }
    }

    private void CloseAllUIs()
    {
        for (int i = 1; i < UIs.Count; i++)
            UIs[i].SetActive(false);
    }
}

public enum UIType
{
    MainMenu,
    Bubble,
    Gallery,
    Schedule,
    ScheduleProcessing,
}