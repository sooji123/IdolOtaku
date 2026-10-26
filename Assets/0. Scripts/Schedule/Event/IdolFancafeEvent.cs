using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 덕질 이벤트(팬카페)
/// </summary>
public class IdolFancafeEvent : BaseScheduleEvent
{
    #region Data
    [Header("Event Settings")]
    [SerializeField] private string eventKey; //입력이 필요함
    [SerializeField] private string introMessage;
    [SerializeField] private string[] choices = new string[3];

    [Header("Result Messages")]
    [SerializeField] private string WaitingMessages = "댓글이 달릴까?/n.....";
    [SerializeField] private string[] resultMessages =
    {
        "내 댓글을 봐줬다!!",
        "팬심이 50 상승했다."
    };

    private int selectedChoice = -1;

    [Header("UI References")]
    [SerializeField] private Image targetImage;

    [SerializeField] private List<Sprite> startClip;
    private List<Sprite> resultClip;
    private List<Sprite> selectClip;

    private ScheduleAnimationAssetTableSO animationTable;
    #endregion

    protected override void OnEventStart() {
        // 1. 텍스트 로컬라이징 로드
        introMessage = LocalizationManager.Instance.GetString(eventKey);

        WaitingMessages = LocalizationManager.Instance.GetString("live_comment_result_waiting") + "\n" //추후 수정
                        + LocalizationManager.Instance.GetString("otakuevent_ing");

        for (int i = 0; i < 3; i++)
        {
            string choiceKey = $"{eventKey}_select{i + 1}";
            choices[i] = LocalizationManager.Instance.GetString(choiceKey);
        }

        // 2. 스프라이트 자동 로드
        animationTable = TableManager.Instance.GetTable<ScheduleAnimationAssetTableSO>();

        startClip = animationTable.GetAnimationClip($"#{eventKey}");

    }

    protected override IEnumerator EventSequence()
    {
        // 1단계: 안내 메시지
        PlayAnimation(startClip);
        yield return ShowMessage(introMessage);

        // 2단계: 선택지 표시
        yield return ShowChoice(choices, OnChoiceSelected);

        // 3단계: 선택 결과 처리
        selectClip = animationTable.GetAnimationClip($"#{eventKey}_select{selectedChoice + 1}");
        PlayAnimation(selectClip);
        yield return ShowMessage(WaitingMessages);

        if (selectedChoice >= 0 && selectedChoice < choices.Length)
        {
            var eventTable = TableManager.Instance.GetTable<otakueventTableSO>();
            TableData.otakuevent eventData = eventTable.GetData(eventKey);
            ResultType finalResult = GetWeightedResult(eventData.resultData);
            bool isSuccess = (finalResult == ResultType.Success || finalResult == ResultType.Perfect);

            if (isSuccess)
            {
                resultClip = animationTable.GetAnimationClip($"#{eventKey}_result{selectedChoice + 1}");
                PlayAnimation(resultClip);
                resultMessages[0] = LocalizationManager.Instance.GetString("live_comment_result_success");
            }
            else
            {
                resultMessages[0] = LocalizationManager.Instance.GetString("live_comment_result_fail");
            }

            ResultData resultData = new ResultData(eventData, finalResult);
            StatusController.Instance.ApplyResultData(resultData);

            int fanshipChange = 0;
            foreach (var data in resultData.statusChangeDataList)
            {
                if (data.statusType == StatusType.Fanship) fanshipChange = data.value;
            }

            resultMessages[1] = fanshipChange >= 0 ? $"팬심이 {fanshipChange} 상승했다." : $"팬심이 {Mathf.Abs(fanshipChange)} 하락했다.";

            yield return ShowMessage(resultMessages[0]);
            yield return ShowMessage(resultMessages[1]);

            //아이돌 호감도 체크
            int currentFanship = StatusController.Instance.statusSO.status.Fanship;
            var table = TableManager.Instance.GetTable<Idol_favorabilityTableSO>();

            if (table != null)
            {
                foreach (MemberType currentIdol in Enum.GetValues(typeof(MemberType)))
                {
                    var levelInfo = table.GetLevelInfo(currentIdol.ToString(), currentFanship);

                    if (levelInfo != null)
                    {
                        int newFavorability = levelInfo.favorability_level;

                        UserDataManager.Instance.IdolFavorabilityChange(currentIdol, newFavorability);
                    }
                }
            }
        }

        // 4단계: 이벤트 완료
        CompleteEvent(new EventResult
        {
            isSuccess = true,
            selectedOption = selectedChoice
        });
    }
    protected override void OnEventEnd() { StatusController.Instance.ResetAllStatusHightlights(); }

    private void PlayAnimation(List<Sprite> clips)
    {
        if (clips == null || clips.Count == 0) return;
        targetImage.sprite = clips[0];
    }

    private void OnChoiceSelected(int index)
    {
        selectedChoice = index;
        Debug.Log($"선택됨: {choices[index]}");
    }

}
