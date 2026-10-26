using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "IconDBSO", menuName = "DB/IconDBSO")]
public class IconDBSO : ScriptableObject
{
    public List<IconData> iconDataList = new List<IconData>();
    
    public void UpdateDB(List<IconData> iconDataList)
    {
        this.iconDataList = new List<IconData>(iconDataList);
    }
}

[Serializable]
public class IconData
{
    public string key;
    public Sprite icon;

    public IconData(Sprite icon)
    {
        key = icon.name;
        this.icon = icon;
    }
}