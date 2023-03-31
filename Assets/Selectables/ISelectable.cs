using UnityEngine;

public interface ISelectable
{
    SELECTABLE_TYPE Type { get; }
    public bool Selected { get; }
    public bool Hovered { get; }
    public bool Selectable { get; set; }
    public bool CanReselect { get; }
    GameObject SelectableObject { get; }
    public void OnDeselect();

    public void OnEndHover();

    public void OnSelect();

    public void OnStartHover();

    public void WhileHovering();

    public void WhileSelected();
}