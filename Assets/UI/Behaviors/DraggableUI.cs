using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUI : MonoBehaviour, IDragHandler, IPointerDownHandler,IBeginDragHandler, IEndDragHandler, IPointerUpHandler
{
    public Image Item;
    public Vector3 StartLocalPosition;
    [SerializeField] protected bool _snapToOrigin;
    public bool SnapToOrigin { get => _snapToOrigin; set 
        { 
            _snapToOrigin= value;
            if (value)
            {
                StartLocalPosition = transform.localPosition;
            }
        } 
    }
    public event Action OnDrop;

    private void Start()
    {
        StartLocalPosition = transform.localPosition;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        Item.raycastTarget = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if(SnapToOrigin) transform.localPosition = StartLocalPosition;
        Item.raycastTarget = true;
        OnDrop?.Invoke();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (SnapToOrigin) transform.localPosition = StartLocalPosition;
    }
}
