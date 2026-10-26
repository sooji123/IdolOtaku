using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BadBubble : MonoBehaviour, IPointerClickHandler
{
    private BadBubbleController controller;
    private int myIndex;

    [SerializeField] private TextMeshProUGUI text;

    public void Init(BadBubbleController controller, int index, string text)
    {
        this.controller = controller;
        this.myIndex = index;
        this.text.text = text;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller.ReturnIndex(myIndex);

        Destroy(gameObject);
    }
}
