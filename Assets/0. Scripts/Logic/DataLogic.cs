using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class DataLogic
{
    public static ConfigData GetConfig()
    {
        ExcelReader reader = new ExcelReader();
        return reader.ReadConfigSheet(DataPathConstant.CONFIG_PATH);
    }
    public static List<MessageKeyData> GetMessageKeyList()
    {
        ExcelReader reader = new ExcelReader();
        return reader.ReadMessageSheet(DataPathConstant.MESSAGEKEY_PATH);

    }
    public static List<ScheduleData> GetScheduleDataList()
    {
        ExcelReader reader = new ExcelReader();
        return reader.ReadScheduleSheet(DataPathConstant.SCHEDULE_PATH);
    }

    public static List<CharacterData> GetCharacterDataList()
    {
        ExcelReader reader = new ExcelReader();
        return reader.ReadCharacterSheet(DataPathConstant.CHARACTER_PATH);
    }

    public static List<DialogueData> GetDialogueDataList()
    {
        ExcelReader reader = new ExcelReader();
        return reader.ReadDialougeSheet(DataPathConstant.DIALOGUE_PATH);
    }

    public static List<ItemData> GetItemDataList()
    {
        ExcelReader reader = new ExcelReader();
        return reader.ReadItemSheet(DataPathConstant.ITEM_PATH);
    }
    public static List<IconData> GetIconDataList()
    {
        List<IconData> dataList = new List<IconData>();
        
        string iconPath = Path.Combine(Application.dataPath, DataPathConstant.ICON_PATH);
        if (!Directory.Exists(iconPath))
        {
            Debug.LogWarning("Cannot Find Folder : " + iconPath);
            return dataList;
        }
        
        DirectoryInfo rootDir = new DirectoryInfo(iconPath);
        
        FileInfo[] pngFiles = rootDir.GetFiles("*.png", SearchOption.TopDirectoryOnly)
            .ToArray();

        foreach (var file in pngFiles)
        {
            string relativePath = $"Assets/{DataPathConstant.ICON_PATH}/" + file.Name;
            relativePath = relativePath.Replace("\\", "/");

#if UNITY_EDITOR
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(relativePath);
#else
            Sprite sprite = null;
#endif
            
            if (sprite != null)
                dataList.Add(new IconData(sprite));
        }

        return dataList;
    }
    public static List<ScheduleAnimationData> GetScheduleAnimationDataList()
    {
        List<ScheduleAnimationData> dataList = new List<ScheduleAnimationData>();

        string scheduleAnimFullPath = Path.Combine(Application.dataPath, DataPathConstant.SCHEDULE_ANIMATION_PATH);
        if (!Directory.Exists(scheduleAnimFullPath))
        {
            Debug.LogWarning("Cannot Find Folder : " + scheduleAnimFullPath);
            return dataList;
        }

        DirectoryInfo rootDir = new DirectoryInfo(scheduleAnimFullPath);

        foreach (DirectoryInfo subDir in rootDir.GetDirectories())
        {
            string folderName = subDir.Name;

            FileInfo[] pngFiles = subDir.GetFiles("*.png", SearchOption.TopDirectoryOnly)
                .OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f.Name)))
                .ToArray();


            List<Sprite> sprites = new List<Sprite>();

            foreach (FileInfo file in pngFiles)
            {
                string relativePath = $"Assets/{DataPathConstant.SCHEDULE_ANIMATION_PATH}/" + folderName + "/" + file.Name;
                relativePath = relativePath.Replace("\\", "/");
                
#if UNITY_EDITOR

                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(relativePath);
#else
                Sprite sprite = null;
#endif

                if (sprite != null)
                {
                    sprites.Add(sprite);
                }
                else
                {
                    Debug.LogWarning($"Cannot Find Sprite : {relativePath}");
                }
            }

            if (sprites.Count > 0)
            {
                dataList.Add(new ScheduleAnimationData(folderName, sprites));
            }
        }
        
        return dataList;
    }
}
