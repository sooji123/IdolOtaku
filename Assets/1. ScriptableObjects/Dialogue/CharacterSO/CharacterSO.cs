using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    [PreviewField] public Sprite portrait;
    public int ID;
    public MemberType Member_ID;
    public string characterName;
    public int favorability;

}
