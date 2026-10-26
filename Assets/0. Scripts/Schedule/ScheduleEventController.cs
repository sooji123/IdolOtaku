using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Table;
using UnityEngine;

/// <summary>
/// 스케줄 이벤트들을 관리하고 실행하는 컨트롤러
/// </summary>
public class ScheduleEventController : MonoBehaviour
{
    [Title("UI Elements")] [SerializeField]
    private GameObject UI;
    
    [Title("Event Prefabs")]
    [SerializeField, Tooltip("등록된 이벤트 프리팹 목록")]
    private List<BaseScheduleEvent> eventPrefabs = new List<BaseScheduleEvent>();

    [Title("Event Container")]
    [SerializeField, Tooltip("이벤트 인스턴스가 생성될 부모 Transform")]
    private Transform eventContainer;

    [Title("UI References")]
    [SerializeField, Tooltip("메시지 다이얼로그 UI")]
    private ScheduleEventMessageDialogue messageUI;

    [SerializeField, Tooltip("선택지 다이얼로그 UI")]
    private ScheduleEventChoiceDialogue choiceUI;

    [Title("Debug")]
    [SerializeField, ReadOnly]
    private BaseScheduleEvent currentEvent;

    public event Action OnEventSequenceCompleted;

    private EventContext currentContext;

    // UI 상태
    private bool isWaitingForConfirm;
    private bool isWaitingForChoice;
    private int selectedChoiceIndex;

    private void Awake()
    {
        if (messageUI != null) messageUI.Initialize(this);
        if (choiceUI != null) choiceUI.Initialize(this);
    }

    #region Event Sequence

    /// <summary>
    /// 이벤트 시퀀스를 시작합니다.
    /// </summary>
    public void StartEventSequence(bool isWeekend)
    {
        ScheduleManager.Instance.isEventSequence = true;
        
        currentContext = new EventContext
        {
            isWeekend = isWeekend
        };

        if (UI != null) UI.SetActive(true);
        ExecuteNextEvent();
    }

    /// <summary>
    /// 다음 이벤트를 실행합니다.
    /// </summary>
    private void ExecuteNextEvent()
    {
        BaseScheduleEvent selectedEvent = SelectEvent(currentContext);

        if (selectedEvent == null)
        {
            OnAllEventsCompleted();

            ScheduleManager.Instance.isEventSequence = false;
            
            return;
        }

        StartEvent(selectedEvent);
    }

    /// <summary>
    /// 실행할 이벤트를 선택합니다. 필요시 오버라이드하여 선택 로직을 변경할 수 있습니다.
    /// </summary>
    protected virtual BaseScheduleEvent SelectEvent(EventContext context)
    {
        var configTable = TableManager.Instance.GetTable<ConfigTableSO>();
        var rateConfig = configTable.GetData("schedule_otakuevent_rate");
        var rate1Config = configTable.GetData("select_member_rate1");
        var rate2Config = configTable.GetData("select_member_rate2");

        if (rateConfig == null || rate1Config == null || rate2Config == null) return null;

        float eventOccurrenceRate = Convert.ToSingle(rateConfig.Value);
        if (UnityEngine.Random.value > eventOccurrenceRate) return null;

        if (eventPrefabs == null || eventPrefabs.Count == 0) return null;

        // 가중치 설정
        float memberRate1 = Convert.ToSingle(rate1Config.Value);
        float memberRate2 = Convert.ToSingle(rate2Config.Value);

        const float BEAT_ALL_WEIGHT = 1.0f;

        MemberType selectedIdol = UserDataManager.Instance.GetSelectedIdol();
        List<(BaseScheduleEvent prefab, float weight)> weightedEvents = new List<(BaseScheduleEvent, float)>();
        float totalWeight = 0f;

        foreach (var eventPrefab in eventPrefabs)
        {
            if (eventPrefab == null) continue;

            float currentWeight = 0f;

            if (eventPrefab.memberType == MemberType.beat_all)
            {
                currentWeight = BEAT_ALL_WEIGHT;
            }
            else if (eventPrefab.memberType == selectedIdol)
            {
                currentWeight = memberRate1;
            }
            else
            {
                currentWeight = memberRate2;
            }

            if (currentWeight > 0)
            {
                weightedEvents.Add((eventPrefab, currentWeight));
                totalWeight += currentWeight;
            }
        }

        if (weightedEvents.Count == 0) return null;

        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var item in weightedEvents)
        {
            cumulativeWeight += item.weight;
            if (roll <= cumulativeWeight)
            {
                Debug.Log($"[EventSelected] {item.prefab.name} | Type: {item.prefab.memberType} | Weight: {item.weight}");
                return item.prefab;
            }
        }
        return null;
    }

    /// <summary>
    /// 특정 이벤트를 직접 시작합니다.
    /// </summary>
    public void StartEvent(BaseScheduleEvent eventPrefab)
    {
        if (currentEvent != null)
        {
            Debug.LogWarning("이미 진행 중인 이벤트가 있습니다.");
            return;
        }

        Transform parent = eventContainer != null ? eventContainer : transform;
        currentEvent = Instantiate(eventPrefab, parent);
        currentEvent.Initialize(this);
        currentEvent.OnEventCompleted += OnEventCompleted;
        currentEvent.StartEvent(currentContext);
    }

    /// <summary>
    /// ID로 이벤트를 찾아 시작합니다.
    /// </summary>
    public void StartEventById(string eventId)
    {
        var eventPrefab = GetEventById(eventId);
        if (eventPrefab != null)
        {
            StartEvent(eventPrefab);
        }
        else
        {
            Debug.LogWarning($"이벤트를 찾을 수 없습니다: {eventId}");
        }
    }

    /// <summary>
    /// ID로 이벤트 프리팹을 검색합니다.
    /// </summary>
    public BaseScheduleEvent GetEventById(string eventId)
    {
        foreach (var eventPrefab in eventPrefabs)
        {
            if (eventPrefab != null && eventPrefab.name == eventId)
            {
                return eventPrefab;
            }
        }
        return null;
    }

    /// <summary>
    /// 이벤트 프리팹을 등록합니다.
    /// </summary>
    public void RegisterEvent(BaseScheduleEvent eventPrefab)
    {
        if (eventPrefab != null && !eventPrefabs.Contains(eventPrefab))
        {
            eventPrefabs.Add(eventPrefab);
        }
    }

    private void OnEventCompleted(EventResult result)
    {
        if (currentEvent != null)
        {
            currentEvent.OnEventCompleted -= OnEventCompleted;
            Destroy(currentEvent.gameObject);
            currentEvent = null;
        }

        HandleEventResult(result);

        // 이벤트는 하루에 하나만 발생하므로 바로 종료
        OnAllEventsCompleted();
        ScheduleManager.Instance.isEventSequence = false;
    }

    /// <summary>
    /// 이벤트 결과를 처리합니다. 필요시 오버라이드합니다.
    /// </summary>
    protected virtual void HandleEventResult(EventResult result)
    {
        Debug.Log($"Event Result - Success: {result.isSuccess}, Score: {result.score}");
    }

    private void OnAllEventsCompleted()
    {
        if (UI != null) UI.SetActive(false);
        OnEventSequenceCompleted?.Invoke();
    }

    #endregion

    #region UI Request Methods

    /// <summary>
    /// 메시지를 표시하고 확인을 기다립니다.
    /// </summary>
    public Coroutine RequestShowMessage(string message, Action onConfirm = null)
    {
        return CoroutineRunner.Run(ShowMessageCoroutine(message, onConfirm));
    }

    /// <summary>
    /// 선택지를 표시하고 선택을 기다립니다.
    /// </summary>
    public Coroutine RequestShowChoice(string[] options, Action<int> onSelected)
    {
        return CoroutineRunner.Run(ShowChoiceCoroutine(options, onSelected));
    }

    /// <summary>
    /// 메시지 다이얼로그 코루틴.
    /// </summary>
    protected virtual IEnumerator ShowMessageCoroutine(string message, Action onConfirm)
    {
        isWaitingForConfirm = true;
        bool isTyping = true;
        Coroutine currentCoroutine = CoroutineRunner.Run(TypeText(message, () => isTyping = false));

        // 확인 버튼 클릭 대기
        while (isWaitingForConfirm)
        {
            yield return null;
        }

        if (isTyping)
        {
            CoroutineRunner.Stop(currentCoroutine);
            messageUI.Show(message);
            isWaitingForConfirm = true;

            while (isWaitingForConfirm)
            {
                yield return null;
            }
        }

        messageUI.Hide();
        onConfirm?.Invoke();
    }

    private IEnumerator TypeText(string targetMessage, Action onComplete)
    {
        messageUI.Show("");
        string currentText = "";

        for (int i = 0; i < targetMessage.Length; i++)
        {
            currentText += targetMessage[i];
            messageUI.Show(currentText);
            yield return new WaitForSeconds(0.03f);
        }

        onComplete?.Invoke(); // 타이핑 종료 알림
    }

    /// <summary>
    /// 선택지 다이얼로그 코루틴.
    /// </summary>
    protected virtual IEnumerator ShowChoiceCoroutine(string[] options, Action<int> onSelected)
    {
        isWaitingForChoice = true;
        selectedChoiceIndex = -1;
        choiceUI.Show(options);

        // 선택 버튼 클릭 대기
        while (isWaitingForChoice)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);//선택지 표시 보여주기
        choiceUI.Hide();
        onSelected?.Invoke(selectedChoiceIndex);
    }

    #endregion

    #region Editor Test (Odin)

    [Title("Test")]
    [Button("Test Message"), GUIColor(0.4f, 0.8f, 1f)]
    private void TestShowMessage()
    {
        if (UI != null) UI.SetActive(true);
        RequestShowMessage("테스트 메시지입니다.\n클릭하면 닫힙니다.", () =>
        {
            if (UI != null) UI.SetActive(false);
            Debug.Log("메시지 확인됨");
        });
    }

    [Button("Test Choice"), GUIColor(0.4f, 1f, 0.6f)]
    private void TestShowChoice()
    {
        if (UI != null) UI.SetActive(true);
        RequestShowChoice(new[] { "선택지 1", "선택지 2", "선택지 3" }, index =>
        {
            if (UI != null) UI.SetActive(false);
            Debug.Log($"선택됨: {index}");
        });
    }

    [Button("Test Event Sequence (Weekend)"), GUIColor(1f, 0.8f, 0.4f)]
    private void TestEventSequenceWeekend()
    {
        StartEventSequence(true);
    }

    [Button("Test Event Sequence (Weekday)"), GUIColor(1f, 0.8f, 0.4f)]
    private void TestEventSequenceWeekday()
    {
        StartEventSequence(false);
    }

    [Title("Sample Event Test")]
    [SerializeField, Tooltip("테스트용 SampleChoiceEvent 프리팹")]
    private BaseScheduleEvent sampleChoiceEventPrefab;

    [SerializeField, Tooltip("테스트용 SampleMinigameEvent 프리팹")]
    private SampleMinigameEvent sampleMinigameEventPrefab;

    [Button("Test SampleChoiceEvent"), GUIColor(0.8f, 0.6f, 1f)]
    public void TestSampleChoiceEvent()
    {
        if (sampleChoiceEventPrefab == null)
        {
            Debug.LogWarning("SampleChoiceEvent 프리팹이 할당되지 않았습니다.");
            return;
        }

        currentContext = new EventContext { isWeekend = false };
        if (UI != null) UI.SetActive(true);
        StartEvent(sampleChoiceEventPrefab);
    }

    [Button("Test SampleMinigameEvent"), GUIColor(1f, 0.6f, 0.8f)]
    private void TestSampleMinigameEvent()
    {
        if (sampleMinigameEventPrefab == null)
        {
            Debug.LogWarning("SampleMinigameEvent 프리팹이 할당되지 않았습니다.");
            return;
        }

        currentContext = new EventContext { isWeekend = false };
        if (UI != null) UI.SetActive(true);
        StartEvent(sampleMinigameEventPrefab);
    }

    #endregion

    #region UI Callbacks (버튼에서 호출)

    /// <summary>
    /// 메시지 확인 버튼 클릭 시 호출
    /// </summary>
    public void OnConfirmButtonClicked()
    {
        isWaitingForConfirm = false;
    }

    /// <summary>
    /// 선택지 버튼 클릭 시 호출
    /// </summary>
    public void OnChoiceButtonClicked(int index)
    {
        selectedChoiceIndex = index;
        isWaitingForChoice = false;
    }

    #endregion
}
