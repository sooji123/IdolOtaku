using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class UserDataManager
{
    private static readonly string testFileName = "TestResult.json";
    [SerializeField] private SavableTestData testData = new();

    #region Save/Load
    private void SaveTestData() => Save(testData, testFileName);
    private void LoadTestData()
    {
        var data = Load<SavableTestData>(testFileName);
        if (data != null) testData = data;
    }
    #endregion

    #region Test Result Logic
    public void AddTestResult(string testID, int score)
    {
        var result = new TestResultData
        {
            testID = testID,
            score = score,
        };

        int index = testData.results.FindIndex(r => r.testID == testID);
        if (index >= 0) testData.results[index] = result;
        else testData.results.Add(result);

        SaveTestData();
    }

    /// <summary>
    /// 특정 과목의 중간+기말 합산 점수를 계산하고 학점을 반환합니다.
    /// </summary>
    /// <param name="subjectKey">예: "Spring_Math"</param>
    public string GetTotalGrade(string subjectKey)
    {
        var subjectTests = testData.results
            .Where(r => r.testID.Contains(subjectKey))
            .ToList();

        int totalScore = subjectTests.Sum(r => r.score);

        //학점 기준이 아직 없음
        return totalScore.ToString();
    }
    #endregion
}

[Serializable]
public class TestResultData
{
    public string testID;
    public int score;
}

[Serializable]
public class SavableTestData
{
    public List<TestResultData> results = new();
}