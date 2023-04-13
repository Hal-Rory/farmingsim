using System;
using UnityEngine;

public enum SELECTABLE_TYPE { none, item, tool, prop, npc, weapon };
[Serializable]
public class Selector
{
    public ISelectable Hovered { get; protected set; }
    public ISelectable Selected { get; protected set; }
    public bool HoverValidated => Hovered != null && !Hovered.Equals(null);
    public bool SelectedValidated => Selected != null && !Selected.Equals(null);    
    [field:SerializeField] public LayerMask Layers { get; private set; }

    public IInputManager InputManager;

    public void CheckHover(GameObject go)
    {
        if(go == null) return;
        ISelectable selectable;
        if (go.TryGetComponent(out selectable))
        {
            if (Hovered != selectable)
            {
                if (HoverValidated)
                {
                    Hovered.OnEndHover();
                }
                if (selectable != null)
                {
                    Hovered = selectable;
                    if (HoverValidated && Hovered.SelectableObject.activeSelf)
                    {
                        Hovered.OnStartHover();
                    }
                    else
                    {
                        Hovered = null;
                    }
                }
            }
            return;
        }
        if (HoverValidated)
        {
            Hovered.OnEndHover();
        }
        Hovered = null;
    }

    public void CheckHover()
    {
        if (InputManager.GetPointingAt(Layers, out GameObject hit))
        {
            CheckHover(hit);            
        } else
        {
            if (HoverValidated)
            {
                Hovered.OnEndHover();
                Hovered = null;
            }
        }
    }

    public void Update()
    {                
        CheckHover();
        
        if (HoverValidated)
        {
            Hovered.WhileHovering();
        }
        if (SelectedValidated)
        {            
            Selected.WhileSelected();
            ClearSelection();            
        }
    }

    public void Select(ISelectable selectable)
    {
        if (Selected != null && Selected != selectable)
        {
            Selected = null;
        }
        if (selectable != null && selectable.Selectable)
        {
            Selected = selectable;
            if (SelectedValidated)
            {
                Selected.OnSelect();
            }
            else
            {
                Selected = null;
            }
        }
    }
    public void Interaction(bool interaction)
    {
        if (interaction)
        {
            Select(Hovered);
        }
    }

    public void ClearHovered()
    {
        if (HoverValidated) Hovered.OnEndHover();
        Hovered = null;
    }
    public void ClearSelection()
    {        
        if(SelectedValidated) Selected.OnDeselect();
        Selected = null;
    }
    public bool TryGetCurrentHovered(out ISelectable selectable)
    {
        selectable = null;
        if (HoverValidated && !InputManager.IsPointerOverUI())
        {
            selectable = Hovered;
            return true;
        }
        return false;
    }
}
