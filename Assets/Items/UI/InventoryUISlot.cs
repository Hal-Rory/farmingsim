using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    [field: SerializeField] public Card InventoryCard { get; private set; }
    public bool Active => InventoryCard.gameObject.activeSelf;
    [SerializeField] private Colorable Colorable;

    public void SetCardActive(bool active)
    {
        InventoryCard.gameObject.SetActive(active);
    }

    public void SetSlotSelected(bool selected)
    {
        if (selected)
        {
            Colorable.SetPrimary();
        } else
        {
            Colorable.SetSecondary();
        }        
    }

}
