using System;
using System.IO;
using ExcelDataReader;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ExcelReader
{
    #region Reader
    public List<List<List<string>>> ReadFile(string filePath)
    {
        List<List<List<string>>> allSheets = new List<List<List<string>>>();

        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();

                for (int sheetIndex = 0; sheetIndex < result.Tables.Count; sheetIndex++)
                {
                    var table = result.Tables[sheetIndex];
                    List<List<string>> sheetData = new();

                    for (int row = 0; row < table.Rows.Count; row++)
                    {
                        List<string> rowData = new();

                        for (int col = 0; col < table.Columns.Count; col++)
                        {
                            string cell = table.Rows[row][col]?.ToString() ?? "";
                            rowData.Add(cell);
                        }

                        sheetData.Add(rowData);
                    }
                    allSheets.Add(sheetData);
                }
            }
        }

        return allSheets;
    }
    #endregion

    #region Dialogue Reader
    public List<DialogueData> ReadDialougeSheet(string filePath)
    {
        List<DialogueData> dialougeSetList = new List<DialogueData>();
        DataMapper mapper = new DataMapper();   

        List<List<List<string>>> sheetList = ReadFile(filePath);

        for (int sheetIndex = 1; sheetIndex < 3; sheetIndex++)
        {
            List<List<string>> sheet = sheetList[sheetIndex];
            List<MessageData> messageDataList = new List<MessageData>();

            for (int rowIndex = 3;  rowIndex < sheet.Count; rowIndex++)
            {
                List<string> rowList = sheet[rowIndex];

                messageDataList.Add(mapper.MappingMessageData(rowList));
            }

            DialogueData dialogueData = new DialogueData(int.Parse(sheet[0][0]),
                                                      messageDataList);

            dialougeSetList.Add(dialogueData);
        }

        return dialougeSetList;
    }
    #endregion

    #region Item Reader
    public List<ItemData> ReadItemSheet(string filePath)
    {
        List<ItemData> itemDataList = new List<ItemData>();
        DataMapper mapper = new DataMapper();

        List<List<List<string>>> sheetList = ReadFile(filePath);
        
        for (int sheetIndex = 0; sheetIndex < sheetList.Count; sheetIndex++)
        {
            List<List<string>> sheet = sheetList[sheetIndex];

            for (int rowIndex = 2; rowIndex < sheet.Count; rowIndex++)
            {
                List<string> rowList = sheet[rowIndex];

                itemDataList.Add(mapper.MappingItemData(rowList));
            }
        }

        return itemDataList;
    }
    #endregion
    
    #region Schedule Reader

    public List<ScheduleData> ReadScheduleSheet(string filePath)
    {
        List<ScheduleData> scheduleDataList = new List<ScheduleData>();
        DataMapper mapper = new DataMapper();

        List<List<List<string>>> sheetList = ReadFile(filePath);

        // SheetIndex
        // 0 = Schedule
        // 1 = Schedule Result
        // 2 = Parttime Result

        //Schedule
        var scheduleSheet = sheetList[0];
        for (int rowIndex = 2; rowIndex < scheduleSheet.Count; rowIndex++)
        {
            List<string> rowList = scheduleSheet[rowIndex];
            
            scheduleDataList.Add(mapper.MappingScheduleData(rowList));
        }

        // Schedule Result
        var scheduleResultSheet = sheetList[1];
        for (int rowIndex = 2; rowIndex < scheduleResultSheet.Count; rowIndex++)
        {
            List<string> rowList = scheduleResultSheet[rowIndex];

            ScheduleData scheduleData = GetScheduleDataById(scheduleDataList, rowList[1]);
            scheduleData.AddScheduleResult(mapper.MappingScheduleStatReward(rowList));
        }
        
        // Parttime Result
        var parttimeResultSheet = sheetList[2];
        for (int rowIndex = 2; rowIndex < parttimeResultSheet.Count; rowIndex++)
        {
            List<string> rowList = parttimeResultSheet[rowIndex];
            
            ScheduleData scheduleData = GetScheduleDataById(scheduleDataList, rowList[0]);
            scheduleData.AddParttimeLevel(mapper.MappingParttimeLevelData(rowList));
        }
        
        return scheduleDataList;
    }
    
    ScheduleData GetScheduleDataById(List<ScheduleData> scheduleDataList, string targetScheduleId)
    {
        return scheduleDataList.Find(scheduleData => scheduleData.scheduleID == targetScheduleId);
    }

    #endregion
    
    #region Character Reader
    public List<CharacterData> ReadCharacterSheet(string filePath)
    {
        List<CharacterData> characterDataList = new List<CharacterData>();
        DataMapper mapper = new DataMapper();
        
        List<List<List<string>>> sheetList = ReadFile(filePath);
        
        List<List<string>> sheet = sheetList[0];

        for (int rowIndex = 2; rowIndex < sheet.Count; rowIndex++)
        {
            List<string> rowList = sheet[rowIndex];
            
            characterDataList.Add(mapper.MappingCharacterData(rowList));
        }
        
        return characterDataList;
    }
    #endregion
    
    #region Message Reader
    public List<MessageKeyData> ReadMessageSheet(string filePath)
    {
        List<MessageKeyData> dataList = new List<MessageKeyData>();

        List<List<List<string>>> sheetList = ReadFile(filePath);
        for (int sheetIndex = 0; sheetIndex < sheetList.Count; sheetIndex++)
        {
            List<List<string>> sheet = sheetList[sheetIndex];

            for (int rowIndex = 1; rowIndex < sheet.Count; rowIndex++)
            {
                List<string> rowList = sheet[rowIndex];
                dataList.Add(new MessageKeyData(rowList[0], rowList[1]));
            }
        }

        return dataList;
    }
    #endregion
    
    #region Config
    public ConfigData ReadConfigSheet(string filePath)
    {
        List<string> dataList = new List<string>();
        DataMapper mapper = new DataMapper();
        
        List<List<List<string>>> sheetList = ReadFile(filePath);
        for (int sheetIndex = 0; sheetIndex < sheetList.Count; sheetIndex++)
        {
            List<List<string>> sheet = sheetList[sheetIndex];

            for (int rowIndex = 2; rowIndex < sheet.Count; rowIndex++)
            {
                List<string> rowList = sheet[rowIndex];
                dataList.Add(rowList[1]);
            }
        }

        return mapper.MappingConfigData(dataList);
    }
    #endregion
}