using System.Collections.Generic;   
using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleAnimationSO", menuName = "Scriptable Objects/ScheduleAnimationSO")]
public class ScheduleAnimationSO : ScriptableObject
{
    public List<Sprite> animationClip = new List<Sprite>();
}
