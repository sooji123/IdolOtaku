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
    public class Dialogue : IIdentifiable<int>
    {
        [field: SerializeField] public int ID { get; internal set; }

        [field: SerializeField] public string Text { get; internal set; }

        [field: SerializeField] public string[] ChoiceStrings { get; internal set; }

        [field: SerializeField] public string Dialogues { get; internal set; }

        [field: SerializeField] public string Function { get; internal set; }

        public Dialogue(int ID, string Text, string[] ChoiceStrings, string Dialogues, string Function)
        {
            this.ID = ID;
            this.Text = Text;
            this.ChoiceStrings = ChoiceStrings;
            this.Dialogues = Dialogues;
            this.Function = Function;
        }
    }
}
