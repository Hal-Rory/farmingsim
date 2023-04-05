using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<Hoverable> PointerEnter;
    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter?.Invoke(this);        
    }
    public UnityEvent<Hoverable> PointerExit;
    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit?.Invoke(this);
    }
}
