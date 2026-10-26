using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StatusHightLightUI : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    [SerializeField] private Image arrow;
    //[SerializeField] private UIEffect hightLightColor;

    [SerializeField] private List<Sprite> arrows = new();

    private void Start()
    {
        arrow.rectTransform.DOAnchorPosY(10, 0.5f).SetRelative(true).SetLoops(-1, LoopType.Yoyo);
    }

    public void ResetHightLight()
    {
        arrow.sprite = null;
        
        UI.SetActive(false);
    }

    public void SetHighLight(ResultType resultType)
    {
        ResetHightLight();
        
        SetArrow(resultType);
        UI.SetActive(true);
    }

    private void SetArrow(ResultType resultType)
    {
        arrow.sprite = arrows[(int)resultType];
    }
}
