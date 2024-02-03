using UnityEngine;
public interface ISelectable
{
    public enum SELECTABLE_TYPE { none, item, tool, prop, npc, weapon, currency };
    SELECTABLE_TYPE Type { get; }
    public bool SelectableBySelector { get;}
    GameObject SelectableObject { get; }
    Vector3 HoverPoint { get; }
    public void OnDeselect();

    public void OnEndHover();

    public void OnSelect();

    public void OnStartHover();

    public void WhileHovering();

    public void WhileSelected();
}