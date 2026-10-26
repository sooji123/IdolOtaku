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
    public class Idol : IIdentifiable<int>
    {
        [field: SerializeField] public int ID { get; internal set; }

        [field: SerializeField] public string StringID { get; internal set; }

        [field: SerializeField] public string Name { get; internal set; }

        [field: SerializeField] public string AgeSpritePath { get; internal set; }

        [field: SerializeField] public string PersonalitySpritePath { get; internal set; }

        [field: SerializeField] public string AbilitySpritePath { get; internal set; }

        public Idol(int ID, string StringID, string Name, string AgeSpritePath, string PersonalitySpritePath, string AbilitySpritePath)
        {
            this.ID = ID;
            this.StringID = StringID;
            this.Name = Name;
            this.AgeSpritePath = AgeSpritePath;
            this.PersonalitySpritePath = PersonalitySpritePath;
            this.AbilitySpritePath = AbilitySpritePath;
        }
    }
}
