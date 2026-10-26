using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainMenuDesignSO", menuName = "Scriptable Objects/MainMenuDesignSO")]
public class MainMenuDesignSO : ScriptableObject
{
    public List<MainMenuDesignElement> elements = new();
}

[Serializable]
public class MainMenuDesignElement
{
    public MemberType type;
    
    public Sprite bg;
    public Sprite btn;
    public Sprite gallery;
}