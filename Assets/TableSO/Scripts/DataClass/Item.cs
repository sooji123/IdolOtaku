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
    public class Item : IIdentifiable<int>
    {
        [field: SerializeField] public int ID { get; internal set; }

        [field: SerializeField] public string Name { get; internal set; }

        [field: SerializeField] public string Description { get; internal set; }

        [field: SerializeField] public string SpritePath { get; internal set; }

        [field: SerializeField] public ItemType Type { get; internal set; }

        [field: SerializeField] public StatusType[] Status { get; internal set; }

        [field: SerializeField] public float[] Values { get; internal set; }

        [field: SerializeField] public int[] Durations { get; internal set; }

        public Item(int ID, string Name, string Description, string SpritePath, ItemType Type, StatusType[] Status, float[] Values, int[] Durations)
        {
            this.ID = ID;
            this.Name = Name;
            this.Description = Description;
            this.SpritePath = SpritePath;
            this.Type = Type;
            this.Status = Status;
            this.Values = Values;
            this.Durations = Durations;
        }
    }
}
