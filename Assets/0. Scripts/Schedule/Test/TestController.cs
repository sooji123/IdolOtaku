using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Table;
using TableData;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestController : MonoBehaviour
{
    #region UI
    [Title("UI Elements")]
    [SerializeField]
    private GameObject UI;
    [SerializeField]
    private TextMeshProUGUI examDateTitle;

    [Title("TestField")]
    [SerializeField, Tooltip("Top Fanel")]
    private Image AnimationImage;
    [SerializeField]
    private Transform Test;
    [SerializeField]
    private GameObject Quiz;
    [SerializeField]
    private TextMeshProUGUI QuizText;
    [SerializeField]
    private GameObject Qte;
    [SerializeField]
    private GameObject SpeeachBubble;

    [Title("BadBubble")]
    [SerializeField]
    BadBubbleController bubbleController;

    [Title("UI References")]
    [SerializeField, Tooltip("메시지 다이얼로그 UI")]
    private TestMessageDialogue messageUI;

    [SerializeField, Tooltip("선택지 다이얼로그 UI")]
    private TestChoiceDialogue choiceUI;

    [SerializeField, Tooltip("QteTest UI")]
    private QteTest qteUI;

    public event Action OnTestSequenceCompleted;

    private EventContext currentContext;

    // UI 상태
    private bool isWaitingForConfirm;
    private bool isWaitingForChoice;
    private int selectedChoiceIndex;

    //현재 시험 종류
    private SeasonType currntSeason;
    private exam currentExam;
    private string currentExamName;

    [SerializeField] private testTableSO testTable;
    [SerializeField] private List<test> testSets = new List<test>();
    private int currentSequenceIndex = 0;

    //애니메이션
    [SerializeField]
    private ScheduleAnimationAssetTableSO Animationtable;
    private float frameTimeModifier = 0.5f;
    private float animationFrameTime = 0.5f;

    [Title("Score Management")]
    [SerializeField] private int totalScore;
    private Dictionary<string, int> subjectScores = new Dictionary<string, int>();
    [SerializeField] private int currentSubjectScore;
    private List<string> examSummaryList = new List<string>();
    #endregion

    private void Awake()
    {
        if (messageUI != null) messageUI.Initialize(this);
        if (choiceUI != null) choiceUI.Initialize(this);
        if (qteUI != null) qteUI.Initialize(this);
    }

    #region Test Sequence
    /// <summary>
    /// 테스트 시퀀스를 시작합니다.
    /// </summary>
    public void StartTestSequence(SeasonType currentSeason)
    {
        ScheduleManager.Instance.isTest = true;
        if (UI != null) UI.SetActive(true);
        Test.gameObject.SetActive(false);

        currntSeason = currentSeason;
        currentExam = (currntSeason == SeasonType.Spring || currntSeason == SeasonType.Autumn)
                      ? exam.midterm : exam.final;

        string targetSeason = currntSeason.ToString().ToLower();
        string targetExam = currentExam.ToString().ToLower();

        // _를 기준으로 분할
        // ID 구조: [0]Test _ [1]Season _ [2]Subject _ [3]Exam
        testSets = testTable.dataList
            .Where(t => {
                string[] splitID = t.ID.Split('_');
                if (splitID.Length < 4) return false;

                bool seasonMatch = splitID[1].ToLower() == targetSeason;
                bool examMatch = splitID[3].ToLower() == targetExam;

                return seasonMatch && examMatch;
            })
            .ToList();

        if (testSets.Count > 0)
        {
            if (UI != null) UI.SetActive(true);
            StartCoroutine(Co_RunExamPeriod());
        }
    }
    #endregion

    #region Coroutine
    // 시험 전체를 총괄하는 코루틴
    private IEnumerator Co_RunExamPeriod()
    {
        examSummaryList.Clear();
        currentSubjectScore = 0;
        totalScore = 0;
        subjectScores.Clear();

        foreach (var testSet in testSets)
        {
            examSummaryList.Clear();
            currentSubjectScore = 0;

            currentExamName = testSet.ID.Split('_')[2];
            string animationKey = GenerateAnimationKey(testSet.ID);
            StartAnimation(Animationtable.GetAnimationClip(animationKey));

            string message = "시험이 시작되었다.";
            yield return RequestShowMessage(message);

            var sequences = testSet.sequences.OrderBy(s => s.test_no);
            foreach (var sequence in sequences)
            {
                yield return StartCoroutine(Co_ExecuteSequence(sequence));
            }

            subjectScores[currentExamName] = currentSubjectScore;
            totalScore += currentSubjectScore;

            UserDataManager.Instance.AddTestResult(testSet.ID, currentSubjectScore);

            // 결과표시
            bubbleController.ClearAllBubbles();
            Test.gameObject.SetActive(true);
            Quiz.gameObject.SetActive(true);
            Qte.gameObject.SetActive(false);

            string finalReport = "";
            foreach (string line in examSummaryList)
            {
                finalReport += line + "\n";
            }

            int totalQuestions = testSet.sequences.Count;
            int correctCount = examSummaryList.Count(x => x.Contains("O"));

            finalReport += $"\n맞힌 문제: {correctCount}/{totalQuestions}";

            QuizText.text = finalReport;
            yield return RequestShowMessage("휴...힘들었다.");

            Test.gameObject.SetActive(false);
            ResetAnimation();
            AnimationImage.sprite = null;
        }

        UI.SetActive(false);
        ScheduleManager.Instance.isTest = false;
        UserDataManager.Instance.AdvanceDateIndex();
        UserDataManager.Instance.SaveData();
        examDateTitle.text = (currentExam == exam.midterm ? "기말고사까지" : "중간고사까지");

        OnTestSequenceCompleted?.Invoke();
    }

    // 개별 문제 처리
    private IEnumerator Co_ExecuteSequence(TestSequence sequence)
    {
        bubbleController.AddBadBubble(sequence.bad_bubble);

        bool isSuccess = false;
        string taskName = GetTaskName(sequence.testType);

        string[] optionTexts = null;
        string text = null;
        int selectedIndex = -1;

        switch (sequence.testType)
        {
            case testType.quiz:
                // 퀴즈
                Test.gameObject.SetActive(true);
                Quiz.gameObject.SetActive(true);
                Qte.gameObject.SetActive(false);

                text = LocalizationManager.Instance.GetString(sequence.key_loc);
                QuizText.text = text;
                optionTexts = null;
                optionTexts = sequence.selections
                                        .Select(s => LocalizationManager.Instance.GetString(s.key_loc))
                                        .ToArray();

                selectedIndex = -1;
                yield return RequestShowChoice(optionTexts, (index) => {
                    selectedIndex = index;
                });

                Test.gameObject.SetActive(false);

                if (selectedIndex != -1)
                {
                    var selectedData = sequence.selections[selectedIndex];
                    if (selectedData.result.success_rate == 1)
                    {
                        currentSubjectScore += selectedData.result.score;
                        yield return RequestShowMessage("문제를 맞췄다.");
                        isSuccess = true;
                    }
                    else
                    {
                        yield return RequestShowMessage("문제를 틀렸다.");
                    }
                }
                break;

            case testType.cheat:
                // 컨닝
                text = LocalizationManager.Instance.GetString(sequence.key_loc);
                optionTexts = null;
                optionTexts = sequence.selections
                                        .Select(s => LocalizationManager.Instance.GetString(s.key_loc))
                                        .ToArray();

                selectedIndex = -1;
                yield return RequestShowChoice(optionTexts, (index) => {
                    selectedIndex = index;
                }, text);

                Test.gameObject.SetActive(false);

                if (selectedIndex != -1)
                {
                    var selectedData = sequence.selections[selectedIndex];
                    float successRate = selectedData.result.success_rate;
                    if (UnityEngine.Random.value <= successRate)
                    {
                        int score = selectedData.result.score;
                        currentSubjectScore += score;

                        yield return RequestShowMessage("컨닝에 성공했다!");
                        isSuccess = true;
                    }
                    else
                    {
                        yield return RequestShowMessage("컨닝에 실패하였다.");
                    }
                }
                break;

            case testType.qte:
                // QTE
                Test.gameObject.SetActive(true);
                Quiz.gameObject.SetActive(false);
                Qte.gameObject.SetActive(true);

                bool? qteResult = null;

                qteUI.StartGame(sequence.qte, (isSuccess) => {
                    qteResult = isSuccess;
                });

                yield return new WaitUntil(() => qteResult.HasValue);

                Test.gameObject.SetActive(false);

                if (qteResult == true)
                {
                    currentSubjectScore += 1;
                    yield return RequestShowMessage("문제를 맞췄다.");
                    isSuccess = true;
                }
                else
                {
                    yield return RequestShowMessage("문제를 틀렸다.");
                }
                break;
        }
        string mark = isSuccess ? "O" : "X";
        examSummaryList.Add($"{sequence.test_no}. {taskName} {mark}");
        yield break;
    }

    #region Animation
    private void StartAnimation(List<Sprite> animationClip)
    {
        ResetAnimation();
        animationCor = StartCoroutine(AnimationProcess(animationClip));
    }

    private void ResetAnimation()
    {
        if (animationCor != null) StopCoroutine(animationCor);
    }

    private Coroutine animationCor;
    private IEnumerator AnimationProcess(List<Sprite> animationClip)
    {
        if (animationClip == null) yield break;

        int index = 0;
        while (true)
        {
            AnimationImage.sprite = animationClip[index];
            index = (index + 1) % animationClip.Count;
            yield return new WaitForSeconds(animationFrameTime * frameTimeModifier);
        }
    }

    private string GenerateAnimationKey(string testID)
    {
        // ID 구조: [0]Test _ [1]spring _ [2]major1 _ [3]midterm
        string[] splitID = testID.Split('_');
        if (splitID.Length >= 3)
        {
            return $"#{splitID[1].ToLower()}_{splitID[2].ToLower()}";
        }
        return string.Empty;
    }
    #endregion

    #region UI Request Methods

    /// <summary>
    /// 메시지를 표시하고 확인을 기다립니다.
    /// </summary>
    public Coroutine RequestShowMessage(string message, Action onConfirm = null)
    {
        return StartCoroutine(ShowMessageCoroutine(message, onConfirm));
    }

    /// <summary>
    /// 선택지를 표시하고 선택을 기다립니다.
    /// </summary>
    public Coroutine RequestShowChoice(string[] options, Action<int> onSelected, string title = null)
    {
        return StartCoroutine(ShowChoiceCoroutine(options, title, onSelected));
    }

    /// <summary>
    /// 메시지 다이얼로그 코루틴.
    /// </summary>
    protected virtual IEnumerator ShowMessageCoroutine(string message, Action onConfirm)
    {
        isWaitingForConfirm = true;
        bool isTyping = true;
        Coroutine currentCoroutine = StartCoroutine(TypeText(message, () => isTyping = false));

        // 확인 버튼 클릭 대기
        while (isWaitingForConfirm)
        {
            yield return null;
        }

        if (isTyping)
        {
            StopCoroutine(currentCoroutine);
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
    protected virtual IEnumerator ShowChoiceCoroutine(string[] options, string title, Action<int> onSelected)
    {
        isWaitingForChoice = true;
        selectedChoiceIndex = -1;
        choiceUI.Show(options, title);

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

    #region Debug Test
    [Title("Debug Tools")]
    [Button(ButtonSizes.Large, Name = "봄 학기 시험 시작 테스트")]
    private void DebugStartSpringTest()
    {
        StartTestSequence(SeasonType.Spring);
    }
    #endregion

    private string GetTaskName(testType type)
    {
        return type switch
        {
            testType.quiz => "미니퀴즈",
            testType.cheat => "컨닝",
            testType.qte => "QTE",
            _ => "시험"
        };
    }

}
#endregion

public enum testType
{
    quiz,
    cheat,
    qte,
}

public enum exam
{
    midterm,
    final,
}