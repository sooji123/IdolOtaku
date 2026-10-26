using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using Table;
using TableData;

[CreateAssetMenu(fileName = "DialogueDBSO", menuName = "DB/DialogueDBSO")]
public class DialogueDBSO : ScriptableObject
{
    public List<Idol> IdolDataList = new();
    public List<DialogueData> dialogueDataList = new();
    public List<CharacterDialogueData> characterDialogueDataList = new();
    public List<DialogueSaveData> saveDatas = new List<DialogueSaveData>();
        
    public void UpdateDB(List<Idol> IdolList, DialogueTableSO dialogueTable)
    {
        this.IdolDataList = new List<Idol>(IdolList);

        dialogueDataList = BuildDialogueDataList(dialogueTable);

        MergeDatas();
    }

    private List<DialogueData> BuildDialogueDataList(DialogueTableSO dialogueTable)
    {
        var result = new List<DialogueData>();

        if (dialogueTable == null || dialogueTable.dataList == null || dialogueTable.dataList.Count == 0)
        {
            Debug.LogWarning("[DialogueDBSO] DialogueTableSO is null or empty.");
            return result;
        }

        Dictionary<int, List<MessageData>> messageMap = new Dictionary<int, List<MessageData>>();

        foreach (var row in dialogueTable.dataList)
        {
            if (row == null) continue;

            int actualID = row.ID;
            int idolId = row.ID / 1000;
            int branchId = row.ID % 1000;

            if (!messageMap.ContainsKey(idolId))
                messageMap[idolId] = new List<MessageData>();

            MessageData md = new MessageData(
                ID: actualID,
                textKey: row.Text,
                choiceStringKey: row.ChoiceStrings,
                imageKey: row.Dialogues,
                function: row.Function
                );

            md.branchID = branchId;

            messageMap[idolId].Add(md);
        }

        foreach (var kvp in messageMap)
        {
            int idolId = kvp.Key;
            List<MessageData> messages = kvp.Value;

            result.Add(new DialogueData(idolId, messages));
        }
        return result;
    }

    public void SetUpSaveData()
    {
        saveDatas.Clear();
        for (int i = 0; i < characterDialogueDataList.Count; i++)
            saveDatas.Add(new DialogueSaveData(characterDialogueDataList[i].dialogueData));
    }
    
    #region Merge
    public void MergeDatas()
    {
        Dictionary<int, Idol> IdolDataList = LoadIdols(this.IdolDataList);
        Dictionary<int, DialogueData> dialogueDataList = LoadDialogueDataList(this.dialogueDataList);

        characterDialogueDataList.Clear();

        foreach (var kvp in dialogueDataList)
        {
            int id = kvp.Key;
            DialogueData dialogueData = kvp.Value;

            if (IdolDataList.TryGetValue(id, out Idol idol))
            {
                CharacterDialogueData characterDialogueData = new CharacterDialogueData(id, idol, dialogueData);
                characterDialogueDataList.Add(characterDialogueData);
            }
        }
        SetUpSaveData();
    }

    public Dictionary<int, Idol> LoadIdols(List<Idol> idolList)
    {
        Dictionary<int, Idol> idolDict = new Dictionary<int, Idol>();
        foreach (var idol in idolList)
        {
            if (!idolDict.ContainsKey(idol.ID))
                idolDict.Add(idol.ID, idol);
        }
        return idolDict;
    }

    public Dictionary<int, DialogueData> LoadDialogueDataList(List<DialogueData> dialogueDataList)
    {
        Dictionary<int, DialogueData> dialogueDict = new Dictionary<int, DialogueData>();
        foreach (var dialogueData in dialogueDataList)
            dialogueDict.Add(dialogueData.ID, dialogueData);
        return dialogueDict;
    }
    #endregion
    
    #region Public Methods
    public CharacterDialogueData GetDialogueByID(int ID)
    { 
        foreach (var dialogue in characterDialogueDataList)
            if (dialogue.ID == ID)
                return dialogue;

        return null;
    }

    public string GetLastMessage(int ID)
    {
        string str = "";

        int lastIndex = GetLastBranchIndex(ID);
        int lastID = GetLastID(ID);

        CharacterDialogueData dialouge = GetDialogueByID(ID);
        if (dialouge == null) return str;

        Branch branch = dialouge.dialogueData.branches[lastIndex];
        List<MessageData> messageDatas = branch.messageDatas;

        if (messageDatas[lastID].messageType == ChatType.Choice) { lastID= lastID - 1; }

        MessageData lastMessage = messageDatas[lastID];

        if(lastMessage.messageType== ChatType.OppChat)
        {
            str = lastMessage.chat;
        }
        else if(lastMessage.messageType == ChatType.OppImage)
        {
            str = "(사진)";
        }

        return str;
    }

    public bool GetMessageAlarm(int ID) { 
        DialogueSaveData data = GetDialogueSaveDataByID(ID);

        if (data == null) return false;
        return data.alarm;
    }

    public void SetMessageAlarm(int ID, bool state)
    {
        DialogueSaveData data = GetDialogueSaveDataByID(ID);

        data.alarm = state;
    }

    public int GetLastID(int ID)
    {
        DialogueSaveData data = GetDialogueSaveDataByID(ID);

        if (data == null) return 0;
        return data.lastID;
    }

    public void SetLastID(int ID, int lastID)
    {
        DialogueSaveData data = GetDialogueSaveDataByID(ID);

        data.lastID = lastID;
    }

    #endregion

    #region Message
    public int GetLastMessageIndex(int ID, int branchID)
    {
        Debug.Log($"ID : {ID}, Branch : {branchID}");
        
        int index = -1;

        BranchSaveData branchSaveData = GetBranchSaveData(ID, branchID);

        Debug.Log($"BranchSaveData : {branchSaveData}");
        
        List<int> datas = branchSaveData.ChoiceSaveDatas;
        for (int i = 0; i < datas.Count; i++)
            if (datas[i] == -1)
            {
                index = i;
                break;
            }

        return index;
    }
    
    #endregion

    #region Branch
    public BranchSaveData GetBranchSaveData(int ID, int branchID)
    {
        DialogueSaveData data = GetDialogueSaveDataByID(ID);
        
        if (data == null) return null;
        
        List<BranchSaveData> branchSaveDatas = data.BranchSaveDatas;
        
        if (branchID > branchSaveDatas.Count) return null;
        
        return branchSaveDatas[branchID];
    }

    public int GetLastBranchIndex(int ID)
    {
        DialogueSaveData data = GetDialogueSaveDataByID(ID);
        if (data == null) return -1;
        return data.lastBranch;
    }
    public void SetLastBranchIndex(int ID,int lastBranch)
    {
        DialogueSaveData data = GetDialogueSaveDataByID(ID);
        data.lastBranch = lastBranch;
    }
    public int GetMaxBranch(int ID)
    {
        DialogueSaveData data = GetDialogueSaveDataByID(ID);
        return (data != null) ? data.maxBranch : 0;
    }

    public void SetMaxBranch(int ID, int index)
    {
        DialogueSaveData data = GetDialogueSaveDataByID(ID);
        if (data == null) return;

        if (data.maxBranch < index){ data.maxBranch = index; }

        if (data.lastBranch == index - 1 && data.alarm == false)
        {
            DialogueData dialogueData = dialogueDataList.Find(d => d.ID == ID);

            if (dialogueData != null && data.lastBranch < dialogueData.branches.Count)
            {
                int lastMessageIndex = dialogueData.branches[data.lastBranch].messageDatas.Count - 1;

                if (data.lastID >= lastMessageIndex)
                {
                    data.lastBranch = index;
                    data.lastID = 0;
                    data.alarm = true;
                }
            }
        }
    }
    #endregion

    #region Selection
    public void SetSelectionBy_ID_Branch_ChoiceID(int ID, int Branch, int ChoiceID, int SelectionIndex)
    {
        DialogueSaveData data = GetDialogueSaveDataByID(ID);

        if (data == null) { return; }
        if (data.BranchSaveDatas.Count < Branch) { return; }
        Debug.Log($"@ ID : {ID}, Branch : {Branch}, ChoiceID : {ChoiceID}, SelectionIndex : {SelectionIndex}");
        Debug.Log($"@ BranchSaveDatas Count : {data.BranchSaveDatas.Count}");
        Debug.Log($"@ ChoiceSaveDatas Count : {data.BranchSaveDatas[Branch].ChoiceSaveDatas.Count}, ChoiceID : {ChoiceID}, SelectionIndex : {SelectionIndex}");
        if (data.BranchSaveDatas[Branch].ChoiceSaveDatas.Count < ChoiceID) { Debug.Log($"초이스가 존재하지 않음"); return; }

        data.BranchSaveDatas[Branch].ChoiceSaveDatas[ChoiceID] = SelectionIndex;
    }

    public int GetSelectionBy_ID_Branch_ChoiceID(int ID, int Branch, int ChoiceID)
    {
        int selection = -1;
        DialogueSaveData data = GetDialogueSaveDataByID(ID);

        Debug.Log($"GetSelectionBy_ID_Branch_ChoiceID Called - ID : {ID}, Branch : {Branch}, ChoiceID : {ChoiceID}");

        if (data == null){
            Debug.Log($"[DialogueDBSO] 정상적이지않은접근1");
            return selection;
        }
        if (data.BranchSaveDatas.Count < Branch){
            Debug.Log($"[DialogueDBSO] 정상적이지않은접근1");
            return selection;
        }
        if (data.BranchSaveDatas[Branch].ChoiceSaveDatas.Count < ChoiceID){
            Debug.Log($"[DialogueDBSO] 정상적이지않은접근1");
            return selection;
        }

        Debug.Log($"GetSelectionBy_ID_Branch_ChoiceID : {data.BranchSaveDatas[Branch].ChoiceSaveDatas[ChoiceID]}");

        selection = data.BranchSaveDatas[Branch].ChoiceSaveDatas[ChoiceID];
        return selection;

        /*DialogueSaveData data = GetDialogueSaveDataByID(ID);
        if (data == null) return -1;

        BranchSaveData branchSave = data.BranchSaveDatas.Find(x => x.ID == Branch);
        if (branchSave == null) return -1;

        if (branchSave.ChoiceSaveDatas.Count <= ChoiceID) return -1;

        return branchSave.ChoiceSaveDatas[ChoiceID];*/
    }
    #endregion

    #region Utils
    private DialogueSaveData GetDialogueSaveDataByID(int ID)
    {
        for (int i = 0; i < saveDatas.Count; i++)
            if (saveDatas[i].ID == ID)
                return saveDatas[i];

        return null;
    }
    #endregion
}

[Serializable]
public class DialogueSaveData
{
    public int ID;            
    public int lastBranch = 0;
    public int maxBranch = -1;
    public bool alarm = true;
    public int lastID;

    [ShowInInspector] public List<BranchSaveData> BranchSaveDatas;

    public DialogueSaveData(DialogueData dialogueData)
    {
        SetUp(dialogueData);
    }

    public void SetUp(DialogueData dialogueData)
    {
        this.ID = dialogueData.ID;
        this.BranchSaveDatas = new List<BranchSaveData>();

        List<Branch> branches = new List<Branch>(dialogueData.branches);
        for (int i = 0; i < branches.Count; i++)
        {
            List<int> currentBranchSelection = new List<int>();
            List<MessageData> messageDatas = new List<MessageData>(branches[i].messageDatas);
            for (int j = 0; j < messageDatas.Count; j++)
                if (messageDatas[j].messageType == ChatType.Choice)
                    currentBranchSelection.Add(-1);
            BranchSaveDatas.Add(new BranchSaveData(branches[i].ID, currentBranchSelection));
        }
    }

}


[Serializable]
public class BranchSaveData
{
    public int ID;
    public List<int> ChoiceSaveDatas;

    public BranchSaveData(int ID, List<int> choices)
    {
        this.ID = ID;
        this.ChoiceSaveDatas = new List<int>(choices);
    }
}