using UnityEngine;

public partial class UserDataManager
{
    #region Save/Load

    private void SaveDateData()
    {
        Save<SavableDateData>(dateData, dateFileName);
    }

    private void LoadDateData()
    {
        var data = Load<SavableDateData>(dateFileName);
        if (data != null) dateData = data;
    }

    #endregion

    [SerializeField] private SavableDateData dateData;

    #region DateIndex
    public int GetDateIndex() => dateData.dateIndex;
    public void SetDateIndex(int value) => dateData.dateIndex = value;

    /// <summary>
    /// 현재 dateIndex를 반환한 뒤 1 증가시킵니다.
    /// </summary>
    public int AdvanceDateIndex() => dateData.dateIndex++;
    #endregion

    #region IsWeekend
    public bool GetIsWeekend() => dateData.isWeekend;
    public void SetIsWeekend(bool value) => dateData.isWeekend = value;
    public void ToggleIsWeekend() => dateData.isWeekend = !dateData.isWeekend;
    #endregion
}

[System.Serializable]
public class SavableDateData
{
    public int dateIndex = 0;
    public bool isWeekend = false;
}
