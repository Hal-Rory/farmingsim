using System;
using UnityEngine;

public class Collapsible : MonoBehaviour
{
    public Action<bool> OnExpanded;
    [SerializeField] private bool Expanded;
    [SerializeField] private RectTransform Expandable;
    public void SetExpand(bool expanded)
    {
        Expanded = expanded;
        Expandable.gameObject.SetActive(Expanded);
        OnExpanded?.Invoke(expanded);
    }
}
