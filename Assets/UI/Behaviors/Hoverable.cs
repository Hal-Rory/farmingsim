using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<Hoverable> PointerEnter = new UnityEvent<Hoverable>();
    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter?.Invoke(this);        
    }
    public UnityEvent<Hoverable> PointerExit = new UnityEvent<Hoverable>();
    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit?.Invoke(this);
    }
}
