using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GalleryUnitSO", menuName = "Scriptable Objects/GalleryUnitSO")]
public class GalleryUnitSO : ScriptableObject
{
    public string id;
    public bool show = false;
    public Sprite preview;
    public Sprite detail;
}
