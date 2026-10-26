using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Selector : MonoBehaviour, IPointerClickHandler
{
    public bool isContains = false;

    [SerializeField] protected ScheduleController scheduleController;

    [SerializeField] protected Image contour;

    #region Selection
    public void ShowContour()
    {
        contour.gameObject.SetActive(true);
    }

    public void HideContour()
    {
        contour.gameObject.SetActive(false);
    }
    #endregion

    public void Select()
    {
        scheduleController.Select(this);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Select();
    }
}
