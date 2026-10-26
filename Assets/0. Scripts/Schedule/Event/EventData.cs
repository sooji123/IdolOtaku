using System.Collections.Generic;

/// <summary>
/// 이벤트 실행에 필요한 컨텍스트 정보
/// </summary>
[System.Serializable]
public class EventContext
{
    /// <summary>
    /// 주말 여부
    /// </summary>
    public bool isWeekend;

    /// <summary>
    /// 추가 데이터 (이벤트별로 필요한 정보 전달용)
    /// </summary>
    public Dictionary<string, object> extraData;

    public EventContext()
    {
        extraData = new Dictionary<string, object>();
    }

    public T GetExtra<T>(string key, T defaultValue = default)
    {
        if (extraData != null && extraData.TryGetValue(key, out var value))
        {
            return (T)value;
        }
        return defaultValue;
    }

    public void SetExtra(string key, object value)
    {
        extraData ??= new Dictionary<string, object>();
        extraData[key] = value;
    }
}

/// <summary>
/// 이벤트 실행 결과
/// </summary>
[System.Serializable]
public class EventResult
{
    /// <summary>
    /// 이벤트 성공 여부
    /// </summary>
    public bool isSuccess;

    /// <summary>
    /// 결과 점수 (미니게임 등에서 사용)
    /// </summary>
    public int score;

    /// <summary>
    /// 선택한 옵션 인덱스 (선택 이벤트에서 사용)
    /// </summary>
    public int selectedOption;

    /// <summary>
    /// 추가 결과 데이터
    /// </summary>
    public Dictionary<string, object> extraData;

    public EventResult()
    {
        isSuccess = true;
        extraData = new Dictionary<string, object>();
    }

    public T GetExtra<T>(string key, T defaultValue = default)
    {
        if (extraData != null && extraData.TryGetValue(key, out var value))
        {
            return (T)value;
        }
        return defaultValue;
    }

    public void SetExtra(string key, object value)
    {
        extraData ??= new Dictionary<string, object>();
        extraData[key] = value;
    }
}
