using System;
using System.Collections.Generic;
using UnityEngine;
using TableSO.Scripts;

/// <summary>
/// Made by TableSO CsvTableGenerator
/// </summary>

namespace TableData
{
    [System.Serializable]
    public class Schedule : IIdentifiable<int>
    {
        [field: SerializeField] public int ID { get; internal set; }

        [field: SerializeField] public string ScheduleName { get; internal set; }

        [field: SerializeField] public string ScheduleIconPath { get; internal set; }

        [field: SerializeField] public ScheduleType ScheduleType { get; internal set; }

        [field: SerializeField] public SeasonType Season { get; internal set; }

        [field: SerializeField] public int MaxStatValue { get; internal set; }

        [field: SerializeField] public ResultType[] Results { get; internal set; }

        [field: SerializeField] public int[] Weights { get; internal set; }

        [field: SerializeField] public int[] Mental { get; internal set; }

        [field: SerializeField] public StatusType[] PerfectStats { get; internal set; }

        [field: SerializeField] public int[] PerfectValues { get; internal set; }

        [field: SerializeField] public StatusType[] SuccessStats { get; internal set; }

        [field: SerializeField] public int[] SuccessValues { get; internal set; }

        [field: SerializeField] public StatusType[] FailStats { get; internal set; }

        [field: SerializeField] public int[] FailValues { get; internal set; }

        [field: SerializeField] public StatusType[] WorstStats { get; internal set; }

        [field: SerializeField] public int[] WorstValues { get; internal set; }

        [field: SerializeField] public int[] PerfectPay { get; internal set; }

        [field: SerializeField] public int[] SuccessPay { get; internal set; }

        [field: SerializeField] public int[] FailPay { get; internal set; }

        [field: SerializeField] public int[] WorstPay { get; internal set; }

        [field: SerializeField] public int[] LevelCount { get; internal set; }

        [field: SerializeField] public float[] LevelHealth { get; internal set; }

        [field: SerializeField] public int[] Lv1ItemIDs { get; internal set; }

        [field: SerializeField] public int[] Lv1ItemRates { get; internal set; }

        [field: SerializeField] public int[] Lv2ItemIDs { get; internal set; }

        [field: SerializeField] public int[] Lv2ItemRates { get; internal set; }

        [field: SerializeField] public int[] Lv3ItemIDs { get; internal set; }

        [field: SerializeField] public int[] Lv3ItemRates { get; internal set; }

        public Schedule(int ID, string ScheduleName, string ScheduleIconPath, ScheduleType ScheduleType, SeasonType Season, int MaxStatValue, ResultType[] Results, int[] Weights, int[] Mental, StatusType[] PerfectStats, int[] PerfectValues, StatusType[] SuccessStats, int[] SuccessValues, StatusType[] FailStats, int[] FailValues, StatusType[] WorstStats, int[] WorstValues, int[] PerfectPay, int[] SuccessPay, int[] FailPay, int[] WorstPay, int[] LevelCount, float[] LevelHealth, int[] Lv1ItemIDs, int[] Lv1ItemRates, int[] Lv2ItemIDs, int[] Lv2ItemRates, int[] Lv3ItemIDs, int[] Lv3ItemRates)
        {
            this.ID = ID;
            this.ScheduleName = ScheduleName;
            this.ScheduleIconPath = ScheduleIconPath;
            this.ScheduleType = ScheduleType;
            this.Season = Season;
            this.MaxStatValue = MaxStatValue;
            this.Results = Results;
            this.Weights = Weights;
            this.Mental = Mental;
            this.PerfectStats = PerfectStats;
            this.PerfectValues = PerfectValues;
            this.SuccessStats = SuccessStats;
            this.SuccessValues = SuccessValues;
            this.FailStats = FailStats;
            this.FailValues = FailValues;
            this.WorstStats = WorstStats;
            this.WorstValues = WorstValues;
            this.PerfectPay = PerfectPay;
            this.SuccessPay = SuccessPay;
            this.FailPay = FailPay;
            this.WorstPay = WorstPay;
            this.LevelCount = LevelCount;
            this.LevelHealth = LevelHealth;
            this.Lv1ItemIDs = Lv1ItemIDs;
            this.Lv1ItemRates = Lv1ItemRates;
            this.Lv2ItemIDs = Lv2ItemIDs;
            this.Lv2ItemRates = Lv2ItemRates;
            this.Lv3ItemIDs = Lv3ItemIDs;
            this.Lv3ItemRates = Lv3ItemRates;
        }
    }
}
