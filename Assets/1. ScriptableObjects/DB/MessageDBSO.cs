using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MessageDBSO", menuName = "DB/MessageDBSO")]
public class MessageDBSO : ScriptableObject
{
    public List<MessageKeyData> messageKeyDataList = new();
    
    public void UpdateDB(List<MessageKeyData> messageKeyDataList)
    {
        this.messageKeyDataList = new List<MessageKeyData>(messageKeyDataList);
    }
}

[Serializable]
public class MessageKeyData
{
    public string key;
    public string message;
    
    public MessageKeyData(string key, string message)
    {
        this.key = key;
        this.message = message;
    }
}