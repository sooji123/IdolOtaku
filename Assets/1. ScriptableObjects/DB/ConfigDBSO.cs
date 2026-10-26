using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ConfigDBSO", menuName = "DB/ConfigDBSO")]
public class ConfigDBSO : ScriptableObject
{
    public ConfigData configData;

    public void UpdateDB(ConfigData configData)
    {
        this.configData = configData;
    }
}