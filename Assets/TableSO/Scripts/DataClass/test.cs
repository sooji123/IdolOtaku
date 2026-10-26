using System;
using System.Collections.Generic;
using UnityEngine;
using TableSO.Scripts;

/// <summary>
/// Custom Data Class - Made by TableSO CustomTableGenerator
/// Key Type: string
/// </summary>

namespace TableData
{
    [System.Serializable]
    public class test : IIdentifiable<string>
    {
        [field: SerializeField] public string ID { get; internal set; }
        [field: SerializeField] public float progress { get; internal set; }
        [field: SerializeField] public List<TestSequence> sequences { get; private set; } = new List<TestSequence>();

        public test(string id)
        {
            ID = id;
        }

        public void SetSequences(List<TestSequence> sequences)
        {
            this.sequences = sequences;
        }

        public void SetProgress(float progress)
        {
            this.progress = progress;
        }
    }

    [Serializable]
    public class TestSequence
    {
        public string ID;
        public int test_no;
        public string key_test;
        public testType testType;
        public List<TestSelection> selections;
        public Testqte qte;
        public string key_loc;
        public string bad_bubble;

        public TestSequence(string ID, int test_no,  string key_test, string key_loc, string bad_bubble)
        {
            this.ID = ID;
            this.test_no = test_no;
            this.key_test = key_test;
            this.key_loc = key_loc;
            this.bad_bubble = bad_bubble;
        }
    }

    [Serializable]
    public class TestSelection
    {
        public string key_test;
        public int select_no;
        public string key_select;
        public TestResult result;
        public string key_loc;

        public TestSelection(string key_test, int select_no, string key_select, string key_loc)
        {
            this.key_test = key_test;
            this.select_no = select_no;
            this.key_select = key_select;
            this.key_loc = key_loc;
        }
    }

    [Serializable]
    public class TestResult
    {
        public string key_select;
        public float success_rate;
        public int score;

        public TestResult(string key_select, float success_rate, int score)
        {
            this.key_select = key_select;
            this.success_rate = success_rate;
            this.score = score;
        }
    }

    [Serializable]
    public class Testqte
    {
        public string key_select;
        public int bg_width;
        public int goal_width;
        public int goal_start_pos;
        public int goal_end_pos;

        public Testqte(string key_select, int bg_width, int goal_width, int goal_start_pos, int goal_end_pos)
        {
            this.key_select = key_select;
            this.bg_width = bg_width;
            this.goal_width = goal_width;
            this.goal_start_pos = goal_start_pos;
            this.goal_end_pos = goal_end_pos;
        }
    }
}
