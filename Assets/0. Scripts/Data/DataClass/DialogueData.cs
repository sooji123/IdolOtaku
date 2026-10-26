using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class DialogueData
{
    public int ID = -1;

    public List<Branch> branches;

    public DialogueData(int id)
    {
        this.ID = id;
        branches = new List<Branch>();
    }

    public DialogueData(int id, List<MessageData> messageDataList)
    {
        this.ID = id;
        this.branches = new List<Branch>();

        foreach (var messageData in messageDataList)
            AddUnit(messageData.branchID, messageData);

        foreach (var branch in branches)
            branch.SortChoiceID();
    }

    public void AddUnit(int branch, MessageData unit)
    {
        Branch _branch = GetOrAddBranch(branch);
        _branch.AddDialogue(unit);
    }

    private Branch GetOrAddBranch(int branch)
    {
        for (int i = 0; i < branches.Count; i++)
            if (branches[i].ID == branch)
                return branches[i];

        Branch _branch = new Branch(branch);
        branches.Add(_branch);
        return _branch;
    }

    public void Print()
    {
        for (int i = 0; i < branches.Count; i++)
            branches[i].Print();
    }
}

[Serializable]
public class Branch
{
    public int ID = -1;

    public List<MessageData> messageDatas;

    public Branch(int ID)
    {
        this.ID = ID;
        messageDatas = new List<MessageData>();
    }

    public void AddDialogue(MessageData dialogue)
    {
        messageDatas.Add(dialogue);
    }

    public void Print()
    {
        for (int i = 0; i < messageDatas.Count; i++)
            messageDatas[i].Print();
    }

    public void SortChoiceID()
    {
        int index = 0;
        foreach (var messageData in messageDatas)
            if (messageData.messageType == ChatType.Choice)
                messageData.choiceID = index++;
    }
}
public enum ChatType
{
    None,
    Me,
    Choice,
    OppChat,
    OppImage,
    EventChat,
}

[Serializable]
public class MessageData
{
    public int branchID;
    public ChatType messageType;
    [ShowIf("@messageType == ChatType.OppChat")]
    public string chat;
    [ShowIf("@messageType == ChatType.OppImage")]
    public string imageID;
    [ShowIf("@messageType == ChatType.OppImage")]
    public Sprite image;
    [ShowIf("@messageType == ChatType.Choice")]
    public int choiceID = -1;
    [ShowIf("@messageType == ChatType.Choice")]
    public List<string> choices = new();

    public MessageData(
        int ID,
        string textKey,
        string[] choiceStringKey,
        string imageKey,
        string function
    )
    {
        branchID = -1;

        // 1) Choice
        if (choiceStringKey != null && choiceStringKey.Length > 0)
        {
            messageType = ChatType.Choice;
            choices = new List<string>(choiceStringKey);
            return;
        }

        // 2) Image
        if (!string.IsNullOrEmpty(imageKey))
        {
            messageType = ChatType.OppImage;
            imageID = imageKey;
            return;
        }

        // 3) Event
        if (!string.IsNullOrEmpty(function))
        {
            messageType = ChatType.EventChat;
            chat = function;
            return;
        }

        // 4) Chat
        if (!string.IsNullOrEmpty(textKey))
        {
            messageType = ChatType.OppChat;
            chat = textKey;
            return;
        }

        messageType = ChatType.None;
    }

    public MessageData(string chat, ChatType bubbleMessageType)
    {
        this.messageType = bubbleMessageType;

        if (bubbleMessageType == ChatType.OppImage)
        {
        }
        else
            this.chat = chat;
    }

    public MessageData(string chat, List<string> choices, int num)
    {
        messageType = ChatType.Choice;

        this.chat = chat;
        this.choices = new List<string>(choices);
        this.choiceID = num;
    }

    public bool IsChoicable()
    {
        return choices != null &&
               choices.Count > 0;
    }

    public void Print()
    {
        Debug.Log($"\t\t  {chat} : {IsChoicable()}");
        if (IsChoicable())
            for (int i = 0; i < choices.Count; i++)
                Debug.Log($"\t\t\t  {i} : {choices[i]}");
    }
}