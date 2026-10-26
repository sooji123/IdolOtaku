using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 모든 스케줄 이벤트의 기본 클래스.
/// 개발자는 이 클래스를 상속받아 EventSequence()를 구현하여 이벤트 흐름을 정의합니다.
/// </summary>
public abstract class BaseScheduleEvent : MonoBehaviour
{
    public MemberType memberType = MemberType.beat_all;
    
    public event Action<EventResult> OnEventCompleted;

    protected EventContext context;
    protected ScheduleEventController controller;
    protected bool isRunning;

    /// <summary>
    /// 컨트롤러에 의해 호출됩니다. 직접 호출하지 마세요.
    /// </summary>
    public void Initialize(ScheduleEventController eventController)
    {
        controller = eventController;
    }

    /// <summary>
    /// 이벤트를 시작합니다.
    /// </summary>
    public void StartEvent(EventContext eventContext)
    {
        if (isRunning) return;

        context = eventContext;
        isRunning = true;

        OnEventStart();
        CoroutineRunner.Run(RunEventSequence());
    }

    private IEnumerator RunEventSequence()
    {
        yield return EventSequence();

        isRunning = false;
        OnEventEnd();
    }

    /// <summary>
    /// 이벤트 흐름을 정의합니다. 개발자가 오버라이드하여 구현합니다.
    /// </summary>
    protected abstract IEnumerator EventSequence();

    /// <summary>
    /// 이벤트 시작 시 호출됩니다. 필요시 오버라이드합니다.
    /// </summary>
    protected virtual void OnEventStart() { }

    /// <summary>
    /// 이벤트 종료 시 호출됩니다. 필요시 오버라이드합니다.
    /// </summary>
    protected virtual void OnEventEnd()
    {
        StatusController.Instance.ResetAllStatusHightlights();
    }

    /// <summary>
    /// 이벤트를 완료 처리합니다. EventSequence 내에서 호출하세요.
    /// </summary>
    protected void CompleteEvent(EventResult result)
    {
        OnEventCompleted?.Invoke(result);
    }

    /// <summary>
    /// 기본 결과로 이벤트를 완료합니다.
    /// </summary>
    protected void CompleteEvent()
    {
        CompleteEvent(new EventResult { isSuccess = true });
    }

    #region Utility Methods (Controller 경유)

    /// <summary>
    /// 메시지를 표시하고 사용자 확인을 기다립니다.
    /// </summary>
    protected Coroutine ShowMessage(string message, Action onConfirm = null)
    {
        return controller.RequestShowMessage(message, onConfirm);
    }

    /// <summary>
    /// 선택지를 표시하고 사용자 선택을 기다립니다.
    /// </summary>
    protected Coroutine ShowChoice(string[] options, Action<int> onSelected)
    {
        return controller.RequestShowChoice(options, onSelected);
    }

    /// <summary>
    /// 지정된 시간만큼 대기합니다.
    /// </summary>
    protected Coroutine Wait(float seconds)
    {
        return CoroutineRunner.Run(WaitCoroutine(seconds));
    }

    private IEnumerator WaitCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    #endregion

    protected ResultType GetWeightedResult(List<OtakuEventResult> results)
    {
        int totalWeight = 0;
        foreach (var res in results) totalWeight += res.weight;

        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int accumulatedWeight = 0;

        foreach (var res in results)
        {
            accumulatedWeight += res.weight;
            if (randomValue < accumulatedWeight) return res.resultType;
        }
        return ResultType.Fail;
    }
}
