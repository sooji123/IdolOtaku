using System;
using UnityEngine;
using UnityEngine.UI;

public class QteTest : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform qteBackground;
    [SerializeField] private RectTransform qteGoal;
    [SerializeField] private RectTransform qteArrow;

    [SerializeField] private Button stopbutton;

    private float arrowSpeed = 500f;
    private bool isRunning;
    private bool movingRight = true;
    private Action<bool> onResult; // 성공 여부를 전달할 콜백

    private TestController controller;

    public void Initialize(TestController testController)
    {
        controller = testController;
    }

    // 게임 설정 및 시작
    public void StartGame(TableData.Testqte data, Action<bool> callback)
    {
        onResult = callback;

        // 1. 배경 넓이 설정 (가운데 정렬 기준)
        qteBackground.sizeDelta = new Vector2(data.bg_width, qteBackground.sizeDelta.y);

        // 2. 성공 칸(Goal) 설정
        qteGoal.sizeDelta = new Vector2(data.goal_width, qteGoal.sizeDelta.y);

        float startX = data.goal_start_pos;

        qteGoal.anchoredPosition = new Vector2(startX, 0);

        // 3. 화살표 초기 위치를 배경 왼쪽 끝으로 설정
        qteArrow.anchoredPosition = new Vector2(0, qteArrow.anchoredPosition.y);

        gameObject.SetActive(true);
        stopbutton.gameObject.SetActive(true);
        isRunning = true;
        movingRight = true; // 시작 시 항상 오른쪽으로 이동
    }

    private void Update()
    {
        if (!isRunning) return;

        float maxWidth = qteBackground.sizeDelta.x;
        float currentX = qteArrow.anchoredPosition.x;

        if (movingRight)
        {
            currentX += arrowSpeed * Time.deltaTime;
            if (currentX >= maxWidth) movingRight = false;
        }
        else
        {
            currentX -= arrowSpeed * Time.deltaTime;
            if (currentX <= 0) movingRight = true;
        }

        qteArrow.anchoredPosition = new Vector2(currentX, qteArrow.anchoredPosition.y);
    }

    public void StopArrow()
    {
        if (!isRunning) return;
        isRunning = false;

        float arrowX = qteArrow.anchoredPosition.x;

        float goalMin = qteGoal.anchoredPosition.x;
        float goalMax = qteGoal.anchoredPosition.x + qteGoal.sizeDelta.x;

        bool isSuccess = (arrowX >= goalMin && arrowX <= goalMax);

        onResult?.Invoke(isSuccess);
        stopbutton.gameObject.SetActive(false);
    }
}
