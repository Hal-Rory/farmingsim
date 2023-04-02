using UnityEngine;
using UnityEngine.EventSystems;

public class DockableUI : DraggableUI
{
    public bool CheckUI;
#if UNITY_EDITOR
    [TagSelector]
#endif
    public string TagFilter= string.Empty;
    public LayerMask Layers;

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (CheckUI)
        {
            if (!string.IsNullOrEmpty(TagFilter) && eventData.pointerCurrentRaycast.gameObject != null 
                && eventData.pointerCurrentRaycast.gameObject.CompareTag(TagFilter))
            {
                if (SnapToOrigin) StartLocalPosition = transform.InverseTransformPoint(eventData.pointerCurrentRaycast.gameObject.transform.position);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(TagFilter) && GameManager.Instance.InputManager.GetPointingAt(Layers, out GameObject hit)
                && hit.CompareTag(TagFilter) && hit.TryGetComponent(out IUIDock dock))
            {
                dock.Docked(this);
            }
        }
        base.OnEndDrag(eventData);
    }
}
