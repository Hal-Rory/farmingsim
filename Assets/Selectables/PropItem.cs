using Farm.Field;
using GameTime;
using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class PropItem : MonoBehaviour, ISelectable, ITimeListener
{
    public TOOL_TYPE ToolNeeded = TOOL_TYPE.None;

    protected abstract void ValidateProp();

    #region TimeListener
    public abstract void ClockUpdate(TimeStruct timestamp);
    public virtual void Register()
    {
        ITimeManager.Instance.RegisterListener(this);
    }
    public virtual void Unregister()
    {
        ITimeManager.Instance.UnregisterListener(this);
    }
    #endregion
    #region ISelectable
    public GameObject SelectableObject { get => gameObject; }
    [field: SerializeField] public bool SelectableBySelector { get; private set; } = true;
    public SELECTABLE_TYPE Type => SELECTABLE_TYPE.prop;
    [field: SerializeField] public Vector3 HoverPoint { get; private set; }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.TransformPoint(HoverPoint), .1f);
    }
    public abstract void OnDeselect();

    public abstract void OnEndHover();

    public abstract void OnSelect();

    public abstract void OnStartHover();

    public abstract void WhileHovering();

    public abstract void WhileSelected();
    #endregion
    public abstract void Interact();
}
