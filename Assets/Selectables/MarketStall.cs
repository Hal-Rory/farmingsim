using UnityEngine;

public class MarketStall : MonoBehaviour, ISelectable
{
    public MarketplaceMenu MarketMenu;
    public string MarketName;
    public SELECTABLE_TYPE Type => SELECTABLE_TYPE.prop;

    [field: SerializeField] public bool Selectable { get; private set; } = true;


    public GameObject SelectableObject => gameObject;

    public void OnDeselect()
    {
    }

    public void OnEndHover()
    {
    }

    public void OnSelect()
    {
        MarketMenu.ChangeMarket(MarketName);
        MarketMenu.gameObject.SetActive(true);
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
