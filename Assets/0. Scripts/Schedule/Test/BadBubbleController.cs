using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class BadBubbleController : MonoBehaviour
{
    #region UI
    [Title("BadBubble")]
    [SerializeField] private BadBubble badBubblePrefab;

    [SerializeField, MinMaxSlider(0f, 5f, true)]
    private Vector2 spawnDelayRange = new Vector2(0.5f, 2.0f);

    [Title("Container")]
    [SerializeField, Tooltip("badBubble이 생성될 부모 Transform")]
    private Transform container;

    private readonly Vector2[] basePositions = new Vector2[]
    {
        new Vector2(0, 0),
        new Vector2(250, 250),
        new Vector2(250, -250),
        new Vector2(-250, 250),
        new Vector2(-250, -250)
    };

    private List<int> usedIndices = new List<int>();
    private List<BadBubble> activeBubbles = new List<BadBubble>();
    #endregion

    #region Method
    public void AddBadBubble(string textKey)
    {
        /*if (badBubblePrefab == null || container == null) return;

        int randomIndex = GetRandomUnusedIndex();
        usedIndices.Add(randomIndex);

        Vector2 basePos = basePositions[randomIndex];
        float randomX = basePos.x + Random.Range(-100f, 100f);
        float randomY = basePos.y + Random.Range(-100f, 100f);

        BadBubble bubbleInstance = Instantiate(badBubblePrefab, container);
        activeBubbles.Add(bubbleInstance);

        string text = LocalizationManager.Instance.GetString(textKey);
        bubbleInstance.transform.localPosition = new Vector3(randomX, randomY, 0);
        bubbleInstance.Init(this, randomIndex, text);*/
        if (badBubblePrefab == null || container == null) return;

        if (usedIndices.Count >= basePositions.Length)
        {
            Debug.LogWarning("모든 버블 위치가 사용 중입니다.");
            return;
        }

        StartCoroutine(Co_DelayedSpawn(textKey));
    }

    private int GetRandomUnusedIndex()
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < basePositions.Length; i++)
        {
            if (!usedIndices.Contains(i)) availableIndices.Add(i);
        }

        return availableIndices[Random.Range(0, availableIndices.Count)];
    }
    #endregion

    #region Coroutine
    private IEnumerator Co_DelayedSpawn(string textKey)
    {
        // 1. 랜덤한 시간만큼 대기
        float delay = Random.Range(spawnDelayRange.x, spawnDelayRange.y);
        yield return new WaitForSeconds(delay);

        // 2. 대기 후에 사용 가능한 인덱스 다시 확인 (대기 중에 상황이 변했을 수 있음)
        if (usedIndices.Count >= basePositions.Length) yield break;

        // 3. 실제 생성 로직
        int randomIndex = GetRandomUnusedIndex();
        usedIndices.Add(randomIndex);

        Vector2 basePos = basePositions[randomIndex];
        float randomX = basePos.x + Random.Range(-100f, 100f);
        float randomY = basePos.y + Random.Range(-100f, 100f);

        BadBubble bubbleInstance = Instantiate(badBubblePrefab, container);
        activeBubbles.Add(bubbleInstance);

        string text = LocalizationManager.Instance.GetString(textKey);
        bubbleInstance.transform.localPosition = new Vector3(randomX, randomY, 0);
        bubbleInstance.Init(this, randomIndex, text);
    }
    #endregion

    #region Reset
    public void ResetUsedPositions()
    {
        usedIndices.Clear();
    }
    public void ReturnIndex(int index)
    {
        if (usedIndices.Contains(index))
        {
            usedIndices.Remove(index);
        }
    }
    public void ClearAllBubbles()
    {
        StopAllCoroutines();

        foreach (var bubble in activeBubbles)
        {
            if (bubble != null)
            {
                Destroy(bubble.gameObject);
            }
        }

        activeBubbles.Clear();
        ResetUsedPositions();
    }
    #endregion
}
