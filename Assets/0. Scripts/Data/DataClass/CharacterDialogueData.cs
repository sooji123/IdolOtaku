using System;
using TableData;

[Serializable]
public class CharacterDialogueData
{
    public int ID;
    public Idol characterData;
    public DialogueData dialogueData;

    public CharacterDialogueData(int ID, Idol characterData, DialogueData dialogueData)
    {
        this.ID = ID;
        this.characterData = characterData;
        this.dialogueData = dialogueData;
    }
}