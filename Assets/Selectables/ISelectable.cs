using UnityEngine;
public interface ISelectable
{
    SELECTABLE_TYPE Type { get; }
    public bool Selectable { get;}
    GameObject SelectableObject { get; }
    public void OnDeselect();

    public void OnEndHover();

    public void OnSelect();

    public void OnStartHover();

    public void WhileHovering();

    public void WhileSelected();
}