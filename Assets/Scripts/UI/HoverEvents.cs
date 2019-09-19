using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoverEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private UnityEvent HoverEnter;
    [SerializeField]
    private UnityEvent HoverExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverExit.Invoke();
    }
}
