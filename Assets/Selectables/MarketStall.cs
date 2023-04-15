using UnityEngine;

public class MarketStall : MonoBehaviour, ISelectable
{
    public MarketplaceMenu MarketMenu;
    public string MarketName;
    public SELECTABLE_TYPE Type => SELECTABLE_TYPE.prop;

    [field: SerializeField] public bool SelectableBySelector { get; private set; } = true;

    public GameObject SelectableObject => gameObject;
    [field: SerializeField] public Vector3 HoverPoint { get; private set; }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.TransformPoint(HoverPoint), .1f);
    }

    public void OnDeselect()
    {
    }

    public void OnEndHover()
    {
    }

    public void OnSelect()
    {
        MarketMenu.ChangeMarket(MarketName);
        MarketMenu.OpenFocus();
    }

    public void OnStartHover()
    {
    }

    public void WhileHovering()
    {
        
    }

    public void WhileSelected()
    {
    }
}
