using UnityEngine;
using System;

[Serializable]
public class CharacterData
{
    public int Index;
    public string Member_ID;
    public string Name;
    public string Age;
    public string Personality;
    public string Ability;

    public CharacterData(int index, string member_ID, string name, string age, string personality, string ability)
    {
        Index = index;
        Member_ID = member_ID;
        Name = name;
        Age = age;
        Personality = personality;
        Ability = ability;
    }
}
