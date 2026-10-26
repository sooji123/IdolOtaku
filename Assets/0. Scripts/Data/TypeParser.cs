using UnityEngine;
using System;
using System.Globalization;
public class TypeParser
{
    public bool ParseToBool(string val)
    {
        if (string.IsNullOrWhiteSpace(val))
            return false;
        
        if (val.Equals("TRUE") || val.Equals("true") || val.Equals("True")) return true;
        else if (val.Equals("FALSE") || val.Equals("false") || val.Equals("False")) return false;
        
        throw new FormatException($"Cannot Parse to Bool: '{val}'");
    }

    public int ParseToInt(string val)
    {
        if (string.IsNullOrWhiteSpace(val))
            return 0;

        if (int.TryParse(val.Trim(), out int result))
            return result;

        throw new FormatException($"Cannot Parse to Int: '{val}'");
    }

    public float ParseToFloat(string val)
    {
        if (string.IsNullOrWhiteSpace(val))
            return 0f;

        if (float.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
            return result;

        throw new FormatException($"Cannot Parse to Float: '{val}'");
    }

    public ItemType ParseToItemType(string val)
    {
        if (val.Equals("1")) return ItemType.Study;
        else if (val.Equals("2")) return ItemType.ParttimeJob;
        else if (val.Equals("3")) return ItemType.Fanship;
        else if (val.Equals("4")) return ItemType.Rest;
        
        throw new FormatException($"Cannot Parse to ItemType: '{val}");
    }
}
