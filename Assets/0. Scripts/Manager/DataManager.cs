using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Table;
using TableSO.Scripts;
using TableData;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class DataManager : Singleton<DataManager>
{
    private IdolTableSO idolTable;

    [SerializeField] private DialogueDBSO dialogueDB;

    [SerializeField] private TableCenter tableCenter;

    private DialogueStringTableSO dialogueStringTable;
    [SerializeField] private LanguageType currentLanguage = LanguageType.ko;

    public List<GameObject> alarmImg;
    public List<string> unlockedGalleryKeys = new List<string>();

    [Button]
    public void UpdateDB()
    {
        // Characters
        idolTable = tableCenter.GetTable<IdolTableSO>();
        List<Idol> idolList = new List<Idol>(idolTable.dataList);

        // Dialogue
        var dialogueTable = tableCenter.GetTable<DialogueTableSO>();
        dialogueDB.UpdateDB(idolList, dialogueTable);

        dialogueStringTable = tableCenter.GetTable<DialogueStringTableSO>();
        dialogueStringTable.SetLanguage(currentLanguage);
        Debug.Log($"[DataManager] Dialogue Updated! {dialogueDB.dialogueDataList.Count}");
    }

    #region
    public void GalleryImage(string imageKey)
    {
        if (!unlockedGalleryKeys.Contains(imageKey))
        {
            unlockedGalleryKeys.Add(imageKey);
        }
    }
    #endregion

    #region Dialogue
    public CharacterDialogueData GetDialogueByID(int ID) => dialogueDB.GetDialogueByID(ID);

    public string GetDialogueString(string key)
    {
        if (string.IsNullOrEmpty(key))
            return "";

        if (dialogueStringTable == null)
        {
            dialogueStringTable = tableCenter.GetTable<DialogueStringTableSO>();
            if (dialogueStringTable == null)
                return key;

            dialogueStringTable.SetLanguage(currentLanguage);
        }

        return dialogueStringTable.GetString(key);
    }

    public int GetLastBranchIndex(int ID) => dialogueDB.GetLastBranchIndex(ID);
    public void SetLastBranchIndex(int ID,int lastBranch) => dialogueDB.SetLastBranchIndex(ID, lastBranch);
    
    public int GetSelectionBy_ID_Branch_ChoiceID(int ID, int Branch, int ChoiceID) => dialogueDB.GetSelectionBy_ID_Branch_ChoiceID(ID, Branch, ChoiceID);
    
    public void SetSelectionBy_ID_Branch_ChoiceID(int ID, int Branch, int ChoiceID, int SelectionIndex) => dialogueDB.SetSelectionBy_ID_Branch_ChoiceID(ID, Branch, ChoiceID, SelectionIndex);

    public string GetLastMessage(int ID) {
        var key = dialogueDB.GetLastMessage(ID); 
        return GetDialogueString(key);
    }
    public bool GetMessageAlarm(int ID) => dialogueDB.GetMessageAlarm(ID);

    public void SetMessageAlarm(int ID, bool state) => dialogueDB.SetMessageAlarm(ID, state);

    public int GetLastID(int ID) => dialogueDB.GetLastID(ID);

    public void SetLastID(int ID, int lastID) => dialogueDB.SetLastID(ID, lastID);

    public int GetMaxBranch(int ID) => dialogueDB.GetMaxBranch(ID);
    public void SetMaxBranch(int ID, int index) => dialogueDB.SetMaxBranch(ID, index);

    public void TotalAlarmUI()
    {
        if (alarmImg == null || alarmImg.Count == 0) return;

        bool isAnyAlarmActive = false;

        for (int index=1; index<3; index++) // 수정필요
        {
            if (GetMessageAlarm(index))
            {
                isAnyAlarmActive = true;
                break;
            }
        }

        foreach (var img in alarmImg)
        {
            if (img != null)
                img.SetActive(isAnyAlarmActive);
        }
    }
    #endregion
}
