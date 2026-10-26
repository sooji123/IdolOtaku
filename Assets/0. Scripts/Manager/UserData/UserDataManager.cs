using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public partial class UserDataManager : Singleton<UserDataManager>
{
    #region Paths
    private static readonly string saveFolderName = "UserData";
    private static readonly string playerFileName = "Player.json";
    private static readonly string itemFileName = "Item.json";
    private static readonly string dateFileName = "Date.json";
    #endregion
    
    #region Built in Events

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    #endregion
    
    public void SaveData()
    {
        SavePlayerData();
        SaveItemData();
        SaveDateData();
        SaveParttimeData();
        SaveTestData();
    }

    public void LoadData()
    {
        LoadPlayerData();
        LoadItemData();
        LoadDateData();
        LoadParttimeData();
        LoadTestData();
    }
    
    #region Utils
    private string GetFolderPath()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFolderName);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }

    private string GetFilePath(string fileName) => Path.Combine(GetFolderPath(), fileName);
     
    private void DeleteFile(string fileName) => File.Delete(GetFilePath(fileName));

    private void Save<T>(T data, string fileName) where T : class
    {
        string path = GetFilePath(fileName);

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(path, json);

        Debug.Log($"[{fileName}] Saved");
    }

    private T Load<T>(string fileName) where T : class
    {
        string path = GetFilePath(fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"[{fileName}] Cannot Find");
            return null;
        }

        string json = File.ReadAllText(path);
        T data = JsonConvert.DeserializeObject<T>(json);

        Debug.Log($"[{fileName}] Loaded");

        return data;
    }
    #endregion
}
