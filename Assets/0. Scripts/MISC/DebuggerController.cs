using System;
using UnityEngine;
using UnityEngine.UI;

public class DebuggerController : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private GameObject target;
    
    private void Awake()
    {
        btn.onClick.AddListener(() =>
            target.gameObject.SetActive(!target.activeSelf));
    }
}
