using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class Tester : MonoBehaviour
{
    [Button]
    public List<DialogueData> GetDialougeTable()
    {
        ExcelReader reader = new ExcelReader();

        return reader.ReadDialougeSheet("Assets/BubbleTest.xlsx");
    }

    [Button]
    public List<ItemData> GetItemTable()
    {
        ExcelReader reader = new ExcelReader();

        return reader.ReadItemSheet("Assets/ItemTest.xlsx");
    }
    
    [Button]
    public List<ScheduleData> GetScheduleTable()
    {
        ExcelReader reader = new ExcelReader();
        
        return reader.ReadScheduleSheet("Assets/schedule.xlsx");
    }
}
