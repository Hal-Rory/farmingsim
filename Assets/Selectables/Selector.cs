using System;
using UnityEngine;

public enum SELECTABLE_TYPE { none, item, tool, prop, npc, weapon, currency };
[Serializable]
public class Selector
{
    public ISelectable Hovered { get; protected set; }
    public ISelectable Selected { get; protected set; }
    public bool HoverValidated => Hovered != null && !Hovered.Equals(null);
    public bool SelectedValidated => Selected != null && !Selected.Equals(null);    
    [field:SerializeField] public LayerMask Layers { get; private set; }
    public Action<ISelectable,ISelectable> OnHoveredChanged;
    public IInputManager InputManager;
    public float HoverDistance { get; private set; }

    public void CheckHover()
    {
        if (InputManager.GetPointingAt(Layers, out GameObject hit, out float dist))
        {
            if (hit == null) return;
            ISelectable selectable;
            if (hit.TryGetComponent(out selectable))
            {
                if (Hovered != selectable)
                {
                    HoverDistance = dist;
                    EndPreviousHover(selectable);
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
            EndPreviousHover();
        } else
        {
            EndPreviousHover();
        }
    }
    private void EndPreviousHover(ISelectable newHover = null)
    {
        OnHoveredChanged?.Invoke(Hovered, newHover);
        if (HoverValidated)
        {
            Hovered.OnEndHover();
            Hovered = null;
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
        if (selectable != null && selectable.SelectableBySelector)
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
        EndPreviousHover();
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
