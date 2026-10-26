using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class ConfigData
{
    public float MaxHealth;
    public float InitHealth;
    public float HpPerSchedule;

    public float MaxStudy;
    public float InitStudy;
    
    public float MaxFanship;
    public float InitFanship;
    
    public float InitMoney;
    
    public float MaxMental;
    public float InitMental;

    public List<float> MentalModifiers = new();

    public ConfigData(float maxHealth, float initHealth, float hpPerSchedule,
                      float maxStudy, float initStudy,
                      float maxFanship, float initFanship,
                      float initMoney,
                      float maxMental, float initMental,
                      float bestMental, float goodMental, float normalMental, float badMental, float worstMental)
    {
        MaxHealth = maxHealth;
        InitHealth = initHealth;
        HpPerSchedule = hpPerSchedule;
        
        MaxStudy = maxStudy;
        InitStudy = initStudy;
        
        MaxFanship = maxFanship;
        InitFanship = initFanship;
        
        InitMoney = initMoney;
        
        MaxMental = maxMental;
        InitMental = initMental;
        MentalModifiers.Add(bestMental);
        MentalModifiers.Add(goodMental);
        MentalModifiers.Add(normalMental);
        MentalModifiers.Add(badMental);
        MentalModifiers.Add(worstMental);
    }
}

public enum MentalType
{
    Best,
    Good,
    Normal,
    Bad,
    Worst,
}