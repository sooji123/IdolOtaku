# Schedule Event System

스케줄 이벤트 시스템은 미니게임, 선택 이벤트, 안내 등 다양한 형태의 이벤트를 코드로 정의하고 관리할 수 있는 프레임워크입니다.

---

## 파일 구조

```
Assets/0. Scripts/Schedule/
├── ScheduleEventController.cs    # 이벤트 관리 및 UI 요청 처리
└── Event/
    ├── BaseScheduleEvent.cs      # 이벤트 추상 클래스
    ├── EventData.cs              # EventContext, EventResult 데이터 클래스
    ├── SampleChoiceEvent.cs      # 예제: 선택 이벤트
    └── SampleMinigameEvent.cs    # 예제: 미니게임 이벤트
```

---

## 아키텍처 개요

```
┌─────────────────────────────────────────────────────────────┐
│                  ScheduleEventController                     │
│  ┌─────────────────────────────────────────────────────────┐│
│  │ - 이벤트 선택/실행/완료 관리                              ││
│  │ - UI 요청 처리 (ShowMessage, ShowChoice)                 ││
│  │ - 메시지/선택지 다이얼로그 UI 참조                        ││
│  └─────────────────────────────────────────────────────────┘│
│                              │                               │
│                    Initialize(controller)                    │
│                              │                               │
│                              ▼                               │
│  ┌─────────────────────────────────────────────────────────┐│
│  │                   BaseScheduleEvent                      ││
│  │  ┌─────────────────────────────────────────────────────┐││
│  │  │ ShowMessage() ──► controller.RequestShowMessage()   │││
│  │  │ ShowChoice()  ──► controller.RequestShowChoice()    │││
│  │  └─────────────────────────────────────────────────────┘││
│  └─────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────┘
```

**핵심 포인트**: 이벤트에서 `ShowMessage()`, `ShowChoice()`를 호출하면 Controller를 통해 UI가 표시됩니다. UI 로직은 Controller에서 한 번만 구현하면 됩니다.

---

## 핵심 클래스

### ScheduleEventController

이벤트 관리 및 UI 요청을 처리하는 중앙 컨트롤러입니다.

**주요 역할**:
- 이벤트 프리팹 등록/관리
- 조건에 따른 이벤트 선택
- 이벤트 실행 및 결과 처리
- **메시지/선택지 UI 표시** (중앙 관리)

**Inspector 설정**:
| 필드 | 설명 |
|------|------|
| Event Prefabs | 등록된 이벤트 프리팹 목록 |
| Event Container | 이벤트 인스턴스가 생성될 부모 Transform |
| Message Dialog UI | 메시지 다이얼로그 UI 오브젝트 |
| Choice Dialog UI | 선택지 다이얼로그 UI 오브젝트 |

### BaseScheduleEvent

모든 이벤트의 기본 클래스입니다.

```csharp
public class MyEvent : BaseScheduleEvent
{
    protected override IEnumerator EventSequence()
    {
        // Controller를 통해 UI 표시
        yield return ShowMessage("안녕하세요!");
        yield return ShowChoice(new[]{"옵션1", "옵션2"}, OnSelect);
        CompleteEvent();
    }
}
```

### EventContext / EventResult

```csharp
// 이벤트에 전달되는 컨텍스트
EventContext context = new EventContext
{
    isWeekend = true,
    extraData = new Dictionary<string, object>()
};

// 이벤트 결과
EventResult result = new EventResult
{
    isSuccess = true,
    score = 85,
    selectedOption = 0
};
```

---

## 새 이벤트 만들기

### 1단계: 클래스 생성

```csharp
using System.Collections;
using UnityEngine;

public class FanMeetingEvent : BaseScheduleEvent
{
    [Header("Settings")]
    [SerializeField] private string welcomeMessage = "팬미팅에 오신 것을 환영합니다!";

    protected override IEnumerator EventSequence()
    {
        // 1. 환영 메시지 (Controller의 UI 사용)
        yield return ShowMessage(welcomeMessage);

        // 2. 선택지 제공
        int selected = -1;
        yield return ShowChoice(
            new[] { "사인회", "포토타임", "Q&A" },
            index => selected = index
        );

        // 3. 선택에 따른 분기
        switch (selected)
        {
            case 0:
                yield return ShowMessage("팬들에게 사인을 해주었습니다!");
                break;
            case 1:
                yield return ShowMessage("멋진 포즈로 사진을 찍었습니다!");
                break;
            case 2:
                yield return ShowMessage("팬들의 질문에 답변했습니다!");
                break;
        }

        // 4. 완료
        CompleteEvent(new EventResult
        {
            isSuccess = true,
            selectedOption = selected
        });
    }
}
```

### 2단계: 프리팹 생성

1. Hierarchy에서 빈 GameObject 생성
2. 이벤트 스크립트 컴포넌트 추가
3. Inspector에서 설정값 조정
4. Project 폴더로 드래그하여 프리팹 저장

### 3단계: 컨트롤러에 등록

`ScheduleEventController`의 Inspector에서 `Event Prefabs` 리스트에 프리팹 추가

---

## 유틸리티 메서드

BaseScheduleEvent에서 제공하는 메서드들 (모두 Controller 경유):

| 메서드 | 설명 | 사용 예시 |
|--------|------|-----------|
| `ShowMessage(string, Action)` | 메시지 표시 후 확인 대기 | `yield return ShowMessage("안녕하세요!");` |
| `ShowChoice(string[], Action<int>)` | 선택지 표시 후 선택 대기 | `yield return ShowChoice(options, OnSelect);` |
| `Wait(float)` | 지정 시간(초) 대기 | `yield return Wait(2f);` |
| `CompleteEvent(EventResult)` | 이벤트 완료 처리 | `CompleteEvent(result);` |
| `CompleteEvent()` | 기본 성공 결과로 완료 | `CompleteEvent();` |

---

## UI 연동

### 구조

UI는 `ScheduleEventController`에서 중앙 관리합니다. 개별 이벤트에서는 UI를 직접 참조하지 않습니다.

```
ScheduleEventController
├── messageDialogUI (GameObject)
├── choiceDialogUI (GameObject)
├── RequestShowMessage() ──► ShowMessageCoroutine()
└── RequestShowChoice()  ──► ShowChoiceCoroutine()
```

### 연동 방법

`ScheduleEventController`를 상속하여 UI 코루틴을 오버라이드합니다:

```csharp
public class UIScheduleEventController : ScheduleEventController
{
    [SerializeField] private MessageDialog messageDialog;
    [SerializeField] private ChoiceDialog choiceDialog;

    protected override IEnumerator ShowMessageCoroutine(string message, Action onConfirm)
    {
        // UI 표시
        messageDialog.Show(message);

        // 확인 버튼 대기
        while (!messageDialog.IsConfirmed)
        {
            yield return null;
        }

        // UI 숨기기
        messageDialog.Hide();
        onConfirm?.Invoke();
    }

    protected override IEnumerator ShowChoiceCoroutine(string[] options, Action<int> onSelected)
    {
        // UI 표시
        choiceDialog.Show(options);

        // 선택 대기
        while (!choiceDialog.HasSelected)
        {
            yield return null;
        }

        int selectedIndex = choiceDialog.SelectedIndex;
        choiceDialog.Hide();

        onSelected?.Invoke(selectedIndex);
    }
}
```

### UI 콜백

버튼에서 Controller의 콜백 메서드를 연결합니다:

```csharp
// 메시지 확인 버튼
confirmButton.onClick.AddListener(() => controller.OnConfirmButtonClicked());

// 선택지 버튼 (인덱스 전달)
choiceButtons[0].onClick.AddListener(() => controller.OnChoiceButtonClicked(0));
choiceButtons[1].onClick.AddListener(() => controller.OnChoiceButtonClicked(1));
```

---

## 이벤트 선택 로직 구현

`ScheduleEventController.SelectEvent()`를 오버라이드:

```csharp
public class CustomScheduleEventController : ScheduleEventController
{
    [SerializeField] private BaseScheduleEvent weekendSpecialEvent;
    [SerializeField] private BaseScheduleEvent randomEvent;

    protected override BaseScheduleEvent SelectEvent(EventContext context)
    {
        // 주말에만 특별 이벤트
        if (context.isWeekend)
        {
            return weekendSpecialEvent;
        }

        // 30% 확률로 랜덤 이벤트
        if (Random.value < 0.3f)
        {
            return randomEvent;
        }

        // 이벤트 없음
        return null;
    }
}
```

---

## 이벤트 결과 처리

`ScheduleEventController.HandleEventResult()`를 오버라이드:

```csharp
protected override void HandleEventResult(EventResult result)
{
    if (result.isSuccess)
    {
        UserDataManager.Instance.AddStat("charm", result.score / 10);
    }
    else
    {
        UserDataManager.Instance.AddStat("stress", 5);
    }
}
```

---

## 예제 이벤트

### SampleChoiceEvent

단순한 선택지 이벤트 예제입니다.

- 안내 메시지 표시
- 3개 선택지 제공
- 선택에 따른 결과 메시지

### SampleMinigameEvent

미니게임 + 분기 처리 예제입니다.

- 미니게임 시작 안내
- 미니게임 실행 (목표 점수 달성 여부)
- 성공/실패에 따른 분기
- 실패 시 재도전 선택

---

## 흐름도

```
ScheduleController.OnScheduleEnd()
        │
        ▼
ScheduleEventController.StartEventSequence(isWeekend)
        │
        ▼
    SelectEvent()
        │
        ├─ null → OnEventSequenceCompleted
        │
        └─ event → Instantiate + Initialize(this)
                        │
                        ▼
                BaseScheduleEvent.StartEvent()
                        │
                        ▼
                    EventSequence()
                        │
                        ├─ ShowMessage() ──► Controller.RequestShowMessage()
                        │                            │
                        │                            ▼
                        │                    ShowMessageCoroutine() (UI 표시)
                        │
                        ├─ ShowChoice() ──► Controller.RequestShowChoice()
                        │                            │
                        │                            ▼
                        │                    ShowChoiceCoroutine() (UI 표시)
                        │
                        ▼
                    CompleteEvent()
                        │
                        ▼
                HandleEventResult()
                        │
                        ▼
                ExecuteNextEvent() → (반복)
```

---

## 오버라이드 가능한 메서드

### BaseScheduleEvent

```csharp
// 필수: 이벤트 흐름 정의
protected abstract IEnumerator EventSequence();

// 선택: 이벤트 시작 시 초기화
protected virtual void OnEventStart() { }

// 선택: 이벤트 종료 시 정리
protected virtual void OnEventEnd() { }
```

### ScheduleEventController

```csharp
// 이벤트 선택 로직
protected virtual BaseScheduleEvent SelectEvent(EventContext context);

// 이벤트 결과 처리
protected virtual void HandleEventResult(EventResult result);

// UI 표시 (연동 시 오버라이드)
protected virtual IEnumerator ShowMessageCoroutine(string message, Action onConfirm);
protected virtual IEnumerator ShowChoiceCoroutine(string[] options, Action<int> onSelected);
```

---

## 주의사항

1. **CompleteEvent 호출 필수**: `EventSequence()` 마지막에 반드시 `CompleteEvent()`를 호출하세요.

2. **프리팹 관리**: 이벤트는 실행 시 인스턴스화되고, 완료 시 자동으로 Destroy됩니다.

3. **동시 실행 불가**: 한 번에 하나의 이벤트만 실행됩니다.

4. **UI 중앙 관리**: 개별 이벤트에서 UI를 직접 참조하지 마세요. Controller를 통해 요청합니다.

5. **Context 활용**: `context.extraData`를 통해 이벤트에 추가 정보를 전달할 수 있습니다.
