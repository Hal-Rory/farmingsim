using GameTime;
using Items;
using UnityEngine;
using static ISelectable;

public abstract class PropItem : MonoBehaviour, ISelectable, ITimeListener
{
    public TOOL_TYPE ToolNeeded = TOOL_TYPE.None;

    protected abstract void ValidateProp();
    public abstract void Interact(IFarmToolCollection farmTool);

    #region TimeListener
    public abstract void ClockUpdate(int tick);
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
    public virtual void OnDeselect() { }

    public virtual void OnEndHover() { }

    public virtual void OnSelect() { }

    public virtual void OnStartHover() { 
    }

    public virtual void WhileHovering() { }

    public virtual void WhileSelected() { }
    #endregion

}
