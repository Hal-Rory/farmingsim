using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager _instance;
    public static TooltipManager Instance { get => _instance; }
    private Dictionary<Transform, GameObject> Tooltips = new Dictionary<Transform, GameObject>();
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetCard(Hoverable hoverable, GameObject tooltip, Transform parent)
    {
        if (Tooltips.TryAdd(parent, tooltip))
        {
            RemoveLast(parent);
            Tooltips[parent] = tooltip;
        }
        tooltip.transform.SetParent(transform);
        RectTransform tooltipRect = (tooltip.transform as RectTransform);
        RectTransform cardRect = (hoverable.transform as RectTransform);
        Vector3 position = cardRect.TransformPoint(cardRect.rect.center);
        tooltipRect.position = position;
    }

    public void RemoveLast(Transform parent)
    {
        if (Tooltips.TryGetValue(parent, out GameObject last))
        {
            last.transform.SetParent(parent);
        }
    }
}
