using UnityEngine;
using UnityEngine.EventSystems;

public class DockableUI : DraggableUI
{
    public bool CheckUI;
    public LayerMask Layers;

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (CheckUI)
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (SnapToOrigin) StartLocalPosition = transform.InverseTransformPoint(eventData.pointerCurrentRaycast.gameObject.transform.position);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
            }
        }
        else
        {
            if (GameManager.Instance.Selection.HoverValidated && GameManager.Instance.Selection.Hovered.SelectableObject.TryGetComponent(out IUIDock dock))
            {
                dock.Docked(this);
            }
        }
        base.OnEndDrag(eventData);
    }
}
