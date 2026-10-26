using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using TableData;
using NUnit.Framework.Interfaces;
using Table;

public class MessageUI : MonoBehaviour, IWindowUI
{
    #region Properties
    public BubbleUI bubble;
    public MessageProfileUI profile;

    public GameObject UI;

    [BoxGroup("CharacterData")] public TextMeshProUGUI characterName;
    [BoxGroup("CharacterData")] public TextMeshProUGUI characterRelationship;


    public Transform messageParent;
    public ScrollRect scrollRect;
    public MessageChoiceUI choicePanel;

    public CharacterSO currentCharacter = null;

    public int currentID = -1;
    public int currentBranchID = -1;
    public int currentChoiceID = -1;


    #region Units
    public GameObject unitMeChat;
    public GameObject unitOppChat;
    public GameObject unitOppImage;
    public Image detailsImage;
    public GameObject detailsPanel;
    #endregion

    private Queue<(MessageData data, int index, int branchIdx)> chatQueue = new Queue<(MessageData, int, int)>();

    private bool isProcessingQueue = false;
    private bool isChoice = false;
    private Coroutine processCoroutine = null;
    private MessageUnitMe currentChoiceChat = null;

    private bool isHiding = true;

    #endregion

    private void SetCharacterData(CharacterSO character)
    {
        currentCharacter = character;
        currentID = currentCharacter.ID;
        characterName.text = character.characterName;
        characterRelationship.text = character.favorability.ToString();
    }

    public void SetMessagePanel(CharacterSO character)
    {
        if (MessageQueue.runningCoroutines.TryGetValue(character.ID, out var routine))
        {
            if (routine != null) StopCoroutine(routine);
            MessageQueue.runningCoroutines.Remove(character.ID);
        }

        SetCharacterData(character);

        InitializedByID(character.ID);

        if (chatQueue.Count > 0)
        {
            processCoroutine = StartCoroutine(ProcessChatQueue(character.ID));
        }

        Show();
        isHiding = false;
    }

    private void InitializedByID(int ID)
    {
        Debug.Log($"@ Started With {ID}");
        ClearAllMessages();
        chatQueue.Clear();

        DataManager.Instance.SetMessageAlarm(ID, false);
        CharacterDialogueData data = DataManager.Instance.GetDialogueByID(ID);
        if (data == null) return;

        int lastSavedBranch = DataManager.Instance.GetLastBranchIndex(ID);
        Debug.Log($"@ lastSavedBranchh {lastSavedBranch}");
        int maxBranch = DataManager.Instance.GetMaxBranch(ID);
        Debug.Log($"@ maxBranch {maxBranch}");
        int lastSavedID = DataManager.Instance.GetLastID(ID);
        Debug.Log($"@ lastSavedID {lastSavedID}");

        if (lastSavedBranch > maxBranch) return;

        DialogueData dialogueData = data.dialogueData;

        for (int bIdx = 0; bIdx <= maxBranch; bIdx++)
        {
            if (bIdx >= dialogueData.branches.Count) break;

            Branch branch = dialogueData.branches[bIdx];
            if (branch == null) continue;

            currentBranchID = branch.ID;

            List<MessageData> messageDatas = branch.messageDatas;

            int instantLimit = -1;
            if (bIdx < lastSavedBranch) instantLimit = messageDatas.Count - 1;
            else if (bIdx == lastSavedBranch) instantLimit = lastSavedID;
            else instantLimit = -1;

            for (int i = 0; i <= instantLimit && i < messageDatas.Count; i++)
            {
                MessageData md = messageDatas[i];
                if (md.messageType == ChatType.Choice)
                {
                    int selectionIndex = DataManager.Instance.GetSelectionBy_ID_Branch_ChoiceID(ID, bIdx, md.choiceID);
                    
                    if (selectionIndex != -1)
                    {
                        AddChat(md, selectionIndex);
                    }
                    else
                    {
                        chatQueue.Enqueue((md, i, bIdx));
                    }
                }
                else AddChat(md, -1,i);
            }

            for (int i = instantLimit + 1; i < messageDatas.Count; i++)
            {
                chatQueue.Enqueue((messageDatas[i], i, bIdx));
            }
        }

        StartCoroutine(ScrollToBottomInstant());
    }

    public void AddChat(MessageData data, int choiceSelectionIndex = -1, int index = -1)
    {
        switch (data.messageType)
        {
            case ChatType.Choice:
                {
                    GameObject go = Instantiate(unitMeChat, messageParent);
                    MessageUnitMe co = go.GetComponent<MessageUnitMe>();
                    co.Init(this, null);

                    if (choiceSelectionIndex == -1)
                    {
                        isChoice = true;
                        currentChoiceChat = co;
                        currentChoiceID = data.choiceID;

                        choicePanel.Init(data);

                    }
                    else
                    {
                        co.SetMessage(DataManager.Instance.GetDialogueString(data.choices[choiceSelectionIndex]));
                    }
                }
                break;
            case ChatType.OppChat:
                {
                    bool isLast = IsLastMessageOpp();

                    GameObject go = Instantiate(unitOppChat, messageParent);
                    MessageUnitOppChat co = go.GetComponent<MessageUnitOppChat>();

                    string originalChat = DataManager.Instance.GetDialogueString(data.chat);
                    int maxLineLength = 20;
                    string formattedChat = InsertLineBreaks(originalChat, maxLineLength);

                    co.Init(this, currentCharacter.portrait, DataManager.Instance.GetDialogueString(formattedChat));

                    if (isProcessingQueue)
                    {
                        StartCoroutine(hideWaitImg(co));
                    }
                    else
                    {
                        co.HideChatWait();
                    }

                    if (isLast && index != 0) co.HidePortrait();
                }
                break;
            case ChatType.OppImage:
                {
                    bool isLast = IsLastMessageOpp();

                    GameObject go = Instantiate(unitOppImage, messageParent);
                    MessageUnitOppImage co = go.GetComponent<MessageUnitOppImage>();
                    
                    co.Init(this, currentCharacter.portrait, TableManager.Instance.GetTable<IconAssetTableSO>().GetData(data.imageID).Asset);
                    DataManager.Instance.GalleryImage(data.imageID);

                    if (isLast) co.HidePortrait();
                }
                break;
        }

        if (!isProcessingQueue)
            StartCoroutine(ScrollToBottomInstant());
    }

    public IEnumerator ScrollToBottomInstant()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageParent.GetComponent<RectTransform>());
        yield return null;
        scrollRect.verticalNormalizedPosition = 0f;
        if (choicePanel != null) choicePanel.UpdateCSF();
    }

    private void ClearAllMessages()
    {
        for (int i = 0; i < messageParent.childCount; i++)
            Destroy(messageParent.GetChild(i).gameObject);
    }

    private string InsertLineBreaks(string text, int targetLength)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= targetLength)
            return text;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int startIndex = 0;

        while (startIndex < text.Length)
        {
            if (startIndex + targetLength >= text.Length)
            {
                sb.Append(text.Substring(startIndex));
                break;
            }

            int breakIndex = -1;
            for (int i = startIndex + targetLength; i > startIndex; i--)
            {
                if (text[i] == ' ')
                {
                    breakIndex = i;
                    break;
                }
            }

            if (breakIndex == -1)
            {
                breakIndex = startIndex + targetLength;
                sb.AppendLine(text.Substring(startIndex, targetLength));
                startIndex = breakIndex;
            }
            else
            {
                sb.AppendLine(text.Substring(startIndex, breakIndex - startIndex));
                startIndex = breakIndex + 1;
            }
        }

        return sb.ToString();
    }

    #region IWindowUI
    public void Hide()
    {
        isHiding = true;

        if (isProcessingQueue)
        {
            DataManager.Instance.SetMessageAlarm(currentCharacter.ID, true);
        }
        else
        {
            DataManager.Instance.SetMessageAlarm(currentCharacter.ID, false);
        }

        profile.UpdateLastMessage(currentCharacter.ID);
        profile.UpdateAlarm(currentCharacter.ID);
        DataManager.Instance.TotalAlarmUI();

        isProcessingQueue = false;

        if (processCoroutine != null)
        {
            StopCoroutine(processCoroutine);
            processCoroutine = null;
        }

        if (isChoice == false)
        {
            var remaining = new Queue<(MessageData data, int index, int branch)>(chatQueue);
            chatQueue.Clear();
            StartRemainingMessage(currentCharacter.ID, remaining);
        }
        else
        {
            chatQueue.Clear();
        }

        isChoice = false;
        ClearAllMessages();
        UI.SetActive(false);
        Debug.Log("@ Hide : MessageUI");
        choicePanel.Hide();
    }

    public void Show()
    {
        UI.SetActive(true);
    }
    #endregion

    #region Utils
    private bool IsLastMessageMe()
    {
        if (messageParent.childCount == 0) return false;
        GameObject lastChild = messageParent.GetChild(messageParent.childCount - 1).gameObject;
        return lastChild.GetComponent<MessageUnitMe>() != null;
    }

    private bool IsLastMessageOpp()
    {
        if (messageParent.childCount == 0) return false;
        GameObject lastChild = messageParent.GetChild(messageParent.childCount - 1).gameObject;

        if (lastChild.GetComponent<MessageUnitOppChat>() == null &&
            lastChild.GetComponent<MessageUnitOppImage>() == null)
            return false;

        return true;
    }
    #endregion

    #region Callee Event
    public void OnOppImageUnitClicked(Sprite sprite)
    {
        ShowDetails(sprite);
    }

    public void OnChoiceUnitSetup(int height)
    {
        messageParent.GetComponent<VerticalLayoutGroup>().padding.bottom = height + 15;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)messageParent.transform);
        StartCoroutine(ScrollToBottomInstant());
    }

    public void OnChoice(MessageData data, int selectionIndex, int branchID)
    {
        messageParent.GetComponent<VerticalLayoutGroup>().padding.bottom = 60;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)messageParent.transform);

        DataManager.Instance.SetSelectionBy_ID_Branch_ChoiceID(currentID, branchID - 1,
                                                               currentChoiceID, selectionIndex);
        Debug.Log($"@ [OnChoice] currentID {currentID}, currentBranchID {branchID-1}");

        if (currentChoiceChat != null)
        {
            currentChoiceChat.SetMessage(DataManager.Instance.GetDialogueString(data.choices[selectionIndex]));
            currentChoiceChat = null;
        }

        Debug.Log($"@ OnClick!!!!");
        choicePanel.Clear();
        choicePanel.Hide();
        isChoice = false;
        processCoroutine = StartCoroutine(ProcessChatQueue(currentID));
    }
    #endregion

    #region Coroutine
    IEnumerator hideWaitImg(MessageUnitOppChat co)
    {
        yield return new WaitForSeconds(0.8f);
        if (co == null) { yield break; }
        co.HideChatWait();
    }

    IEnumerator ProcessChatQueue(int ID)
    {
        isProcessingQueue = true;

        while (chatQueue.Count > 0)
        {
            var (data, index, branchIdx) = chatQueue.Dequeue();

            if (data.messageType == ChatType.Choice)
            {

                int savedSelection = DataManager.Instance.GetSelectionBy_ID_Branch_ChoiceID(ID, branchIdx, data.choiceID);

                if (savedSelection == -1)
                {
                    AddChat(data, -1, branchIdx);

                    isProcessingQueue = false;
                    yield break;
                }
                else
                {
                    AddChat(data, savedSelection);
                }
            }
            else
            {
                AddChat(data, -1, index);
                StartCoroutine(ScrollToBottomInstant());
                yield return new WaitForSeconds(1f);
            }

            DataManager.Instance.SetLastBranchIndex(ID, branchIdx);
            DataManager.Instance.SetLastID(ID, index);

            yield return new WaitForSeconds(0.3f);
            yield return StartCoroutine(ScrollToBottomInstant());
        }

        processCoroutine = null;
        isProcessingQueue = false;
    }
    #endregion

    #region Details Panel
    public void ShowDetails(Sprite data)
    {
        detailsImage.sprite = data;
        detailsImage.SetNativeSize();
        detailsPanel.SetActive(true);
    }
    public void HideDetails() => detailsPanel.SetActive(false);
    #endregion

    #region RemainingMessage
    private void StartRemainingMessage(int characterID, Queue<(MessageData, int, int)> queue)
    {
        if (MessageQueue.runningCoroutines.TryGetValue(characterID, out var routine))
            StopCoroutine(routine);

        var newRoutine = StartCoroutine(RemainingMessage(characterID, queue));
        MessageQueue.runningCoroutines[characterID] = newRoutine;
    }

    IEnumerator RemainingMessage(int characterID, Queue<(MessageData data, int index, int branch)> queue)
    {
        yield return new WaitUntil(() => UI.activeSelf == false);

        while (queue.Count > 0)
        {
            if (currentCharacter != null && currentCharacter.ID == characterID && UI.activeSelf)
                yield break;

            var (data, index, branchIdx) = queue.Dequeue();

            if (data.messageType == ChatType.Choice)
            {
                DataManager.Instance.SetLastBranchIndex(characterID, branchIdx);
                DataManager.Instance.SetLastID(characterID, index);
                profile.UpdateLastMessage(characterID);
                yield break;
            }

            yield return new WaitForSeconds(1f);
            DataManager.Instance.SetLastID(characterID, index);
            DataManager.Instance.SetLastBranchIndex(characterID, branchIdx);
            profile.UpdateLastMessage(characterID);
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

}

public static class MessageQueue
{
    public static Dictionary<int, Coroutine> runningCoroutines = new Dictionary<int, Coroutine>();
}