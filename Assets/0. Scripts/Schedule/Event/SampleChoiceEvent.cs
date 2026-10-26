using System.Collections;
using UnityEngine;

/// <summary>
/// 예제: 선택지 이벤트
/// 개발자가 이벤트를 만드는 방법을 보여주는 샘플입니다.
/// </summary>
public class SampleChoiceEvent : BaseScheduleEvent
{
    [Header("Event Settings")]
    [SerializeField] private string introMessage = "무엇을 하시겠습니까?";
    [SerializeField] private string[] choices = { "연습하기", "휴식하기", "팬과 소통하기" };

    [Header("Result Messages")]
    [SerializeField] private string[] resultMessages =
    {
        "열심히 연습했습니다! 실력이 향상됩니다.",
        "푹 쉬었습니다. 컨디션이 회복됩니다.",
        "팬들과 즐거운 시간을 보냈습니다!"
    };

    private int selectedChoice = -1;

    protected override IEnumerator EventSequence()
    {
        // 1단계: 안내 메시지
        yield return ShowMessage(introMessage);

        // 2단계: 선택지 표시
        yield return ShowChoice(choices, OnChoiceSelected);

        // 3단계: 선택 결과 처리
        if (selectedChoice >= 0 && selectedChoice < resultMessages.Length)
        {
            yield return ShowMessage(resultMessages[selectedChoice]);
        }

        // 4단계: 이벤트 완료
        CompleteEvent(new EventResult
        {
            isSuccess = true,
            selectedOption = selectedChoice
        });
    }

    private void OnChoiceSelected(int index)
    {
        selectedChoice = index;
        Debug.Log($"선택됨: {choices[index]}");
    }
}
