using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButtonUIManager : InventoryUIManager
{
    protected override void SetCard(Card card)
    {
        base.SetCard(card);
        ButtonCard button = card as ButtonCard;
        if (button != null)
        {
            void SetInventory()
            {
                GameManager.Instance.SelectFromInventory(card.ID);
            }
            button.SetAction(SetInventory);
        }
    }
}
