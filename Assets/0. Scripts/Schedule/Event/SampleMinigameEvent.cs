using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 예제: 미니게임 이벤트
/// 복잡한 흐름과 분기를 보여주는 샘플입니다.
/// </summary>
public class SampleMinigameEvent : BaseScheduleEvent
{
    [Header("Minigame Settings")]
    [SerializeField] private int targetScore = 80;
    [SerializeField] private float minigameDuration = 3f;

    [Header("UI References")]
    [SerializeField] private GameObject minigameUI;

    private int currentScore;
    private bool minigameCompleted;

    protected override void OnEventStart()
    {
        currentScore = 0;
        minigameCompleted = false;
    }

    protected override IEnumerator EventSequence()
    {
        // 1단계: 미니게임 시작 안내
        yield return ShowMessage("리듬 게임을 시작합니다!\n목표 점수: " + targetScore);

        // 2단계: 미니게임 실행
        yield return RunMinigame();

        // 3단계: 결과에 따른 분기
        if (currentScore >= targetScore)
        {
            // 성공
            yield return ShowMessage($"훌륭합니다! {currentScore}점을 획득했습니다!");

            CompleteEvent(new EventResult
            {
                isSuccess = true,
                score = currentScore
            });
        }
        else
        {
            // 실패 - 재시도 선택
            yield return ShowChoice(
                new[] { "다시 도전하기", "그만두기" },
                OnRetryChoice
            );
        }
    }

    private IEnumerator RunMinigame()
    {
        // 상태 초기화
        minigameCompleted = false;
        currentScore = 0;

        // 미니게임 UI 활성화
        if (minigameUI != null)
            minigameUI.SetActive(true);

        float elapsed = 0f;

        while (!minigameCompleted && elapsed < minigameDuration)
        {
            elapsed += Time.deltaTime;

            // TODO: 실제 미니게임 로직 구현
            // 예: 리듬 게임, QTE 등
            
            Debug.Log(elapsed);

            yield return null;
        }

        // 임시: 랜덤 점수 생성 (실제로는 미니게임 결과)
        currentScore = Random.Range(50, 100);

        if (minigameUI != null)
            minigameUI.SetActive(false);

        minigameCompleted = true;
    }

    private void OnRetryChoice(int choice)
    {
        if (choice == 0)
        {
            // 다시 도전
            StartCoroutine(RetrySequence());
        }
        else
        {
            // 그만두기
            CompleteEvent(new EventResult
            {
                isSuccess = false,
                score = currentScore
            });
        }
    }

    private IEnumerator RetrySequence()
    {
        yield return ShowMessage("다시 도전합니다!");
        yield return RunMinigame();

        if (currentScore >= targetScore)
        {
            yield return ShowMessage($"성공! {currentScore}점!");
            CompleteEvent(new EventResult { isSuccess = true, score = currentScore });
        }
        else
        {
            yield return ShowMessage($"아쉽습니다... {currentScore}점");
            CompleteEvent(new EventResult { isSuccess = false, score = currentScore });
        }
    }

    /// <summary>
    /// 미니게임 중 점수 추가 (외부 UI에서 호출)
    /// </summary>
    [Button]
    public void AddScore(int points)
    {
        currentScore += points;
    }

    /// <summary>
    /// 미니게임 완료 처리 (외부 UI에서 호출)
    /// </summary>
    [Button]
    public void FinishMinigame()
    {
        minigameCompleted = true;
    }
}
