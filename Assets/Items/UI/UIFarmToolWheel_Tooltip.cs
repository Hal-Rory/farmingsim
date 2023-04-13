using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFarmToolWheel_Tooltip : UIFarmToolWheel
{
    [SerializeField] private InfoHeader TooltipCard;
    protected override void SetSlot(InventoryUISlot slot, Item item)
    {
        if (slot.TryGetComponent(out Hoverable hoverable))
        {
            hoverable = slot.gameObject.AddComponent<Hoverable>();
        }
        hoverable.PointerEnter.RemoveAllListeners();
        hoverable.PointerExit.RemoveAllListeners();
        hoverable.PointerEnter.AddListener((hoverable) => 
        {
            TooltipCard.SetHeader(slot.InventoryCard.ID, item.Data.Name);
            TooltipCard.SetInfo(item.Data.Description);
            TooltipCard.gameObject.SetActive(true);
            TooltipManager.Instance.SetCard(hoverable, TooltipCard.gameObject, transform);
        });
        hoverable.PointerExit.AddListener((hoverable) =>
        {
            TooltipCard.gameObject.SetActive(false);
            TooltipManager.Instance.RemoveLast(transform);
        });
        
    }
}
