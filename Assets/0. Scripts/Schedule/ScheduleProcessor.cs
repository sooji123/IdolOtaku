using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Table;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleProcessor : MonoBehaviour
{
    #region Optional Field
    [BoxGroup("Option")] public float frameTimeModifier = 1.0f;
    [BoxGroup("Option")] public float animationFrameTime = 1.0f;
    [BoxGroup("Option")] public float textFrameTime = 0.05f;
    [BoxGroup("Option")] public float lineFrameTime = 1.0f;
    #endregion

    [BoxGroup("Setting")] public ScheduleProcessTextSetting setting;

    // [SerializeField] private StatusController statusController;
    
    public GameObject UI;

    [Title("UI Elements")]
    public Image titleIcon;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI dateText;
    public Image animationImage;
    public Image itemImage;
    public GameObject addedItemPanel;
    public Image addedItemImage;
    public TextMeshProUGUI scheduleText;
    public Image resultImage;

    [Title("Data")]
    [SerializeField] private List<ScheduleResultImage> resultImages = new();
    
    private int processFlag = 0;
    // 0 : 대기 단계
    // 1 : 대사 출력 중
    // 2 : 대사 출력 완료
    // 3 : 스탯 출력 중
    // 4 : 스탯 출력 완료
    // 5 : 아이템 표시 대기
    // 6 : 아이템 표시 완료

    private bool isTouchable = false;

    private ScheduleProcessNode currentNode = null;

    private ScheduleAnimationAssetTableSO table;
    
    #region Built In Events

    private void Start()
    {
        table = TableManager.Instance.GetTable<ScheduleAnimationAssetTableSO>();
    }

    #endregion

    #region Show/Hide
    public void Show()
    {
        UI.SetActive(true);
    }

    public void Hide()
    {
        UI.SetActive(false);
    }
    #endregion

    #region Input
    public void OnTouch()
    {
        if (!isTouchable) return;

        if (processFlag == 0)
            Process_EarlyDone();
        else if (processFlag == 1)
            Process_Next();
        else if (processFlag == 2)
            Status_EarlyDone();
        else if (processFlag == 3)
            Status_Next();
        else if (processFlag == 5)
            Item_Next();
    }

    [Button]
    private void Process_EarlyDone()
    {
        processFlag = 1;

        ResetTexting();

        scheduleText.text = $"{setting.GetScheduleStartString(currentNode.scheduleData.scheduleType)}\n" +
                            $"{setting.GetSchedulingText()}\n" +
                            $"{setting.GetScheduleResultString(currentNode.scheduleData.scheduleType, currentNode.resultData.resultType)}";
    }

    private void Process_Next()
    {
        processFlag = 2;
    }

    private void Status_EarlyDone()
    {
        processFlag = 3;

        ResetTexting();

        scheduleText.text = $"{setting.GetScheduleResultString(currentNode.resultData)}";
        
        /*
        scheduleText.text = $"{setting.GetScheduleStartString(currentNode.scheduleData.scheduleType)}\n" +
                            $"{setting.GetSchedulingText()}\n" +
                            $"{setting.GetScheduleResultString(currentNode.scheduleData.scheduleType, currentNode.resultData.resultType)}";
                            */
    }
    
    private void Status_Next()
    {
        processFlag = 4;
    }

    private void Item_Next()
    {
        processFlag = 6;
    }
    #endregion

    #region Schedule Process
    public void StartScheduleProcess(List<ScheduleProcessNode> nodes)
    {
        ResetScheduleProcess();

        ScheduleManager.Instance.isScheduleProcessing = true;

        Show();

        scheduleCor = CoroutineRunner.Run(ScheduleProcess(nodes));
    }

    private void ResetScheduleProcess()
    {
        CoroutineRunner.Stop(scheduleCor);
        scheduleCor = null;
        ResetAnimation();
    }

    private Coroutine scheduleCor;

    private IEnumerator ScheduleProcess(List<ScheduleProcessNode> nodes)
    {
        isTouchable = true;

        for (int i = 0; i < nodes.Count; i++)
        {
            resultImage.gameObject.SetActive(false);
            currentNode = nodes[i];
            processFlag = 0;            // 0
            Match(currentNode);

            StartScheduleTexting();     // 1 or Wait
            while (processFlag < 2)     // Process_Next 먹음
                yield return null;

            resultImage.sprite = GetResultSpriteByResultType(currentNode.resultData.resultType);
            resultImage.gameObject.SetActive(true);
            AudioManager.Instance.PlayScheduleResultSFX(currentNode.resultData.resultType);

            StatusController.Instance.ApplyResultData(currentNode.resultData);
            if (processFlag < 3)        // 연타로 이미 Status_EarlyDone된 경우 텍스팅 재시작 방지
                StartStatusTexting();   // 3 or Wait

            while (processFlag < 4)     // Status_Next 먹음
                yield return null;
            StatusController.Instance.ResetAllStatusHightlights();

            if (currentNode.scheduleData.scheduleType == ScheduleType.ParttimeJob)
            {
                if (currentNode.resultData.itemRewardList.Count > 0)
                {
                    var itemReward = currentNode.resultData.itemRewardList[0];

                    Debug.Log(itemReward.ItemID);
                    processFlag = 5;
                    ShowAddedItem(int.Parse(itemReward.ItemID));
                    while (processFlag < 6)     // Item_Next 먹음
                        yield return null;
                    HideAddedItem();

                    UserDataManager.Instance.AddItem(itemReward.ItemID);
                }

                var levelData = currentNode.scheduleData.parttimeLevelData;
                UserDataManager.Instance.AddParttimeCount(currentNode.scheduleData.scheduleID, levelData?.levelupCounts);
            }

            resultImage.gameObject.SetActive(false);

            ScheduleManager.Instance.OnDateEnd();

            // 이벤트 돌아가면 잠시 정지.
            while (ScheduleManager.Instance.isEventSequence)
                yield return null;

            // 턴 종료 시 버프 처리
            UserDataManager.Instance.ProcessTurnEnd();
        }

        currentNode = null;

        isTouchable = false;

        EndSequence();
    }

    private void EndSequence()
    {
        ScheduleManager.Instance.isScheduleProcessing = false;

        Hide();
        ScheduleManager.Instance.OnScheduleEnd();
    }
    #endregion
    
    #region Match
    private void Match(ScheduleProcessNode node)
    {
        var iconTable = TableManager.Instance.GetTable<IconAssetTableSO>();

        var scheduleIcon = iconTable.GetData(node.scheduleData.scheduleIcon);
        titleIcon.sprite = scheduleIcon?.Asset;
        titleText.text = LocalizationManager.Instance.GetString(node.scheduleData.scheduleName);

        dateText.text = node.date;

        if (node.itemData == null)
        {
            itemImage.sprite = null;
        }
        else
        {
            var itemIcon = iconTable.GetData(node.itemData.SpriteName);
            itemImage.sprite = itemIcon?.Asset;
        }

        StartAnimation(table.GetAnimationClip(node.scheduleData.scheduleIcon));
    }
    #endregion

    #region Texting
    Sequence texting;
    private void StartScheduleTexting()
    {
        ResetTexting();

        string startStr = setting.GetScheduleStartString(currentNode.scheduleData.scheduleType) + "\n";
        string duringStr = startStr + setting.GetSchedulingText() + "\n";
        string resultStr = duringStr + setting.GetScheduleResultString(currentNode.scheduleData.scheduleType, currentNode.resultData.resultType);

        scheduleText.text = "";
        texting = DOTween.Sequence()
        .Append(scheduleText.DOText(startStr, startStr.Length * textFrameTime * frameTimeModifier).From(""))
        .AppendInterval(lineFrameTime * frameTimeModifier)
        .Append(scheduleText.DOText(duringStr, (duringStr.Length - startStr.Length) * textFrameTime * frameTimeModifier).From(startStr))
        .AppendInterval(lineFrameTime * frameTimeModifier)
        .Append(scheduleText.DOText(resultStr, (resultStr.Length - duringStr.Length) * textFrameTime * frameTimeModifier).From(duringStr))
        .OnComplete(() =>
        {
            processFlag = 1;
        });
    }

    private void StartStatusTexting()
    {
        ResetTexting();

        scheduleText.text = "";
        string resultString = setting.GetScheduleResultString(currentNode.resultData);

        texting = DOTween.Sequence()
            .Append(scheduleText.DOText(resultString, resultString.Length * textFrameTime * frameTimeModifier))
            .OnComplete(() => processFlag = 3);
    }

    private void ResetTexting()
    {
        if (texting != null)
        {
            texting.Pause();
            texting.Kill();
        }
    }
    #endregion

    #region Item
    private void ShowAddedItem(int id)
    {
        var iconPath = TableManager.Instance.GetTable<ItemTableSO>().GetItem(id).SpriteName;
        var itemImage = TableManager.Instance.GetTable<IconAssetTableSO>().GetData(iconPath).Asset;

        addedItemImage.sprite = itemImage;
        addedItemPanel.SetActive(true);
    }

    private void HideAddedItem()
    {
        addedItemPanel.SetActive(false);
    }

    #endregion

    #region Animation
    private void StartAnimation(List<Sprite> animationClip)
    {
        ResetAnimation();
        animationCor = CoroutineRunner.Run(AnimationProcess(animationClip));
    }

    private void ResetAnimation()
    {
        CoroutineRunner.Stop(animationCor);
        animationCor = null;
    }

    private Coroutine animationCor;
    private IEnumerator AnimationProcess(List<Sprite> animationClip)
    {
        if (animationClip == null) yield break;

        int index = 0;
        while (true)
        { 
            animationImage.sprite = animationClip[index];
            index = (index + 1) % animationClip.Count;
            yield return new WaitForSeconds(animationFrameTime * frameTimeModifier);
        }
    }
    #endregion
    
    #region Utils

    private Sprite GetResultSpriteByResultType(ResultType type)
    {
        foreach (var data in resultImages)
            if (data.type == type)
                return data.resultImage;
        
        Debug.LogError($"[ScheduleProcessor] Cannot find image for type '{type}', so return resultImages[0]");
        return resultImages[0].resultImage;
    }

    #endregion
}


[SerializeField]
public class ScheduleProcessNode
{
    public string date;
    public ScheduleData scheduleData;
    public ItemData itemData;
    public ResultData resultData;

    public ScheduleProcessNode(string date, ScheduleData scheduleData, ItemData itemData)
    {
        this.date = date;
        this.scheduleData = scheduleData;
        this.itemData = itemData;

        SetResult();
    }

    private void SetResult()
    {
        resultData = ScheduleResultLogic.GetResult(scheduleData, itemData);
    }
}

[Serializable]
public class ScheduleResultImage
{
    public ResultType type;
    public Sprite resultImage;
}