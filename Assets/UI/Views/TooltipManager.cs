using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager _instance;
    public static TooltipManager Instance { get => _instance; }
    private Dictionary<Transform, GameObject> Tooltips= new Dictionary<Transform, GameObject>();
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
        if(Tooltips.TryAdd(parent, tooltip))
        {
            RemoveLast(parent);
            Tooltips[parent] = tooltip;
        }
        tooltip.transform.SetParent(transform);
        RectTransform tooltipRect = (tooltip.transform as RectTransform);
        RectTransform cardRect = (hoverable.transform as RectTransform);

        Vector2 tooltipPivot;
        if ((tooltipRect.parent as RectTransform).anchorMax.x < .5f)
        {
            tooltipPivot.x = 0;
        }
        else
        {
            tooltipPivot.x = 1;
        }
        if ((tooltipRect.parent as RectTransform).anchorMax.y < .5f)
        {
            tooltipPivot.y = 0;
        }
        else
        {
            tooltipPivot.y = 1;
        }
        tooltipRect.pivot = tooltipPivot;

        Vector3 position = cardRect.TransformPoint(cardRect.rect.center);
        tooltipRect.position = position;
    }

    public void RemoveLast(Transform parent)
    {
        if (Tooltips.TryGetValue(parent, out GameObject last))
        {
            last.transform.SetParent(parent);
        }            
            //void PointerExit(Hoverable hoverable)
            //{
            //    TooltipCard.gameObject.SetActive(false);
            //}

            //hoverable.PointerEnter.AddListener(PointerEnter);
            //hoverable.PointerExit.AddListener(PointerExit);
        }
    }
