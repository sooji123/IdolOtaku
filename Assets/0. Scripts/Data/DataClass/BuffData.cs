using System;

[Serializable]
public class BuffData
{
    public string sourceItemID;
    public StatusType statusType;
    public float value;
    public int remainingTurns;
    public bool isPercent;

    public BuffData(string sourceItemID, StatusType statusType, float value, int duration)
    {
        this.sourceItemID = sourceItemID;
        this.statusType = statusType;
        this.value = value;
        this.remainingTurns = duration == 0 ? 1 : duration;
        this.isPercent = value < 0;
    }

    public int GetEffectValue(int baseValue = 0)
    {
        float effectValue = value;

        // Health는 10배
        if (statusType == StatusType.Health)
            effectValue *= 10;

        // 음수면 퍼센트 적용
        if (isPercent && baseValue != 0)
            return (int)(baseValue * (effectValue / 100f));

        return (int)effectValue;
    }

    public bool Tick()
    {
        remainingTurns--;
        return remainingTurns <= 0;
    }
}
