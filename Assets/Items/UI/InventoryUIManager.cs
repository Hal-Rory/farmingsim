using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace Items
{
    public class InventoryUIManager : MonoBehaviour
    {
        [SerializeField] private List<InventoryUISlot> Slots = new List<InventoryUISlot>();
        [SerializeField] private Transform CardParent;
        [SerializeField] private GameObject CardPrefab;
        [SerializeField] private GameObject SlotPrefab;
        [SerializeField] private InfoHeader TooltipCard;
        [SerializeField] private bool ParentOrdered;
        private Inventory Inventory;
        private int LastIndex;
        
        public void SetInventory(GameObject inventoryObj)
        {

        }

        public void SetInventory(Inventory inv)
        {
            Inventory = inv;
            GetInventorySlots();
            if (Inventory.Count == 0)
            {
                Inventory.OnUpdated += DoUpdated;
            }
            else
            {
                for (int i = 0; i < Inventory.Count; i++)
                {
                    DoUpdated(Inventory[i]);
                }
            }
        }
        private void OnDestroy()
        {
            if(Inventory != null) Inventory.OnUpdated -= DoUpdated;
        }

        protected virtual void DoUpdated(Item item)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots[i].Active && Slots[i].InventoryCard.ID == item.Data.ID)
                {
                    if (Inventory.Contains(item.Data))
                    {
                        Slots[i].InventoryCard.SetLabel(item.Amount.ToString());                        
                    } else
                    {
                        InventoryUISlot slot = Slots[i];
                        slot.InventoryCard.SetEmpty(string.Empty);
                        slot.SetCardActive(false);
                        if (ParentOrdered)
                        {
                            Slots.RemoveAt(i);
                            Slots.Insert(LastIndex, slot);
                            slot.transform.SetSiblingIndex(LastIndex);
                        }
                    }
                    return;
                }
                else if(!Slots[i].Active)
                {
                    Slots[i].SetCardActive(true);
                    Slots[i].InventoryCard.Set(item.Data.ID, item.Amount.ToString(), item.Data.Display);
                    LastIndex= Mathf.Max(i+1, Inventory.Count-1);
                    return;
                }
            }
        }
        public Vector3 test;
        protected void SetCard(Card card)
        {
            Hoverable hoverable = card.GetComponent<Hoverable>();
            void PointerEnter(Hoverable hoverable)
            {
                Item item = Inventory[card.ID];
                TooltipCard.SetHeader(item.Data.ID, item.Data.Name);
                TooltipCard.SetInfo(item.Data.Description);
                TooltipCard.gameObject.SetActive(true);
                RectTransform tooltipRect = (TooltipCard.transform as RectTransform);
                RectTransform cardRect = (card.transform.parent as RectTransform);

                Vector2 tooltipPivot;
                if ((tooltipRect.parent as RectTransform).anchorMax.x < .5f)
                {
                    tooltipPivot.x = 0;
                }
                else
                {
                    tooltipPivot.x = 1;
                }
                if ((tooltipRect.parent as RectTransform).anchorMax.y < .5f)
                {
                    tooltipPivot.y = 0;
                }
                else
                {
                    tooltipPivot.y = 1;
                }
                tooltipRect.pivot = tooltipPivot;
                
                Vector3 position = cardRect.position;
                tooltipRect.position = position;
            }
            void PointerExit(Hoverable hoverable)
            {
                TooltipCard.gameObject.SetActive(false);
            }

            hoverable.PointerEnter.AddListener(PointerEnter);
            hoverable.PointerExit.AddListener(PointerExit);
        }

        protected virtual void GetInventorySlots()
        {
            for (int i = 0; i < Inventory.Max; i++)
            {
                InventoryUISlot slot = Instantiate(SlotPrefab, CardParent).GetComponent<InventoryUISlot>();
                SetCard(slot.InventoryCard);
                Slots.Add(slot);
            }
        }
    }
}