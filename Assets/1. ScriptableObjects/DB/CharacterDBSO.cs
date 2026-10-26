using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterDBSO", menuName = "DB/CharacterDBSO")]
public class CharacterDBSO : ScriptableObject
{
    public List<CharacterData> characterDataList = new();

    public void UpdateDB(List<CharacterData> characterDataList)
    {
        this.characterDataList = new List<CharacterData>(characterDataList);
    }
}