using UnityEngine;
using System;
using Table;

[CreateAssetMenu(fileName = "StatusSO", menuName = "Scriptable Objects/StatusSO")]
public class StatusSO : ScriptableObject
{
    public Status status;

    public void InitStatus()
    {
        var config = TableManager.Instance.GetTable<ConfigTableSO>();
        status = new Status(
            (int)config.GetData("character_health_max").Value,
            (int)config.GetData("character_study_max").Value,
            (int)config.GetData("character_fanship_max").Value,
            (int)config.GetData("schedule_health_count").Value,
            (int)config.GetData("character_health_init").Value,
            (float)config.GetData("character_mental_init").Value,
            (int)config.GetData("character_study_init").Value,
            (int)config.GetData("character_fanship_init").Value,
            (int)config.GetData("character_money_init").Value);
    }
}

[Serializable]
public class Status
{
    public int MaxHP;
    public int MaxStudy;
    public int MaxFanship;
    
    public int HP;
    public float schedulePerHp;
    public MentalType mentalType;
    public float Mental;
    public int Study;
    public int Fanship;
    public int Money;
    public int Favorability;

    public Status(int MaxHP, int MaxStudy, int MaxFanship, float schedulePerHp,
        int initHP, float initMental, int initStudy, int initFanship, int initMoney)
    {
        this.MaxHP = MaxHP;
        this.MaxStudy = MaxStudy;
        this.MaxFanship = MaxFanship;
        
        this.schedulePerHp = schedulePerHp;
        
        this.HP = initHP;
        this.Mental = initMental;
        this.Study = initStudy;
        this.Fanship = initFanship;
        this.Money = initMoney;
    }

    public void AddMental(float value)
    {
        if (Mental + value >= 5.0f) Mental = 5.0f;
        else if (Mental + value <= 1.0f) Mental = 1.0f;
        else Mental += value;
        
        mentalType = (MentalType)((int)(Mental - 1));
    }

    public void AddHP(int value)
    {
        if (HP + value >= MaxHP) HP = MaxHP;
        else if (HP + value <= 0) HP = 0;
        else HP += value;
    }

    public void AddStudy(int value)
    {
        if (Study + value >= MaxStudy) Study = MaxStudy;
        else if (Study + value <= 0) Study = 0;
        else Study += value;
    }

    public void AddFanship(int value)
    {
        if (Fanship + value >= MaxFanship) Fanship = MaxFanship;
        else if (Fanship + value <= 0) Fanship = 0;
        else Fanship += value;
    }

    public void AddMoney(int value)
    {
        if (Money + value >= 0) Money += value;
        else Money = 0;
    }
}