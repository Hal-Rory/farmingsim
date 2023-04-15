using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEditor.Progress;

namespace Items
{
    public class UIFarmToolWheel : MonoBehaviour
    {
        [SerializeField] private List<InventoryUISlot> Slots = new List<InventoryUISlot>();

        protected void Start()
        {
            Assert.IsNotNull(Slots, $"No slots for {name}");
            if(GameManager.Instance.FarmToolManager.GetCurrentTool() != null) SetCurrentToolActive(GameManager.Instance.FarmToolManager.GetCurrentTool());
            GameManager.Instance.FarmToolManager.RegisterListener(SetCurrentToolActive);
            IEnumerable<Item> inv = GameManager.Instance.GetWeapons();

            foreach ( var item in inv ) { 
                DoUpdated(item);
            }            
            GameManager.Instance.OnWeaponsUpdated += DoUpdated;
        }
        private void SetCurrentToolActive(IFarmToolCollection tool)
        {
            SetItemSelected(tool.Data.ID, true);
        }

        protected virtual bool ValidateItem(Item item)
        {
            return item != null && item.Data is ToolData;
        }
        protected void OnDestroy()
        {
            if (GameManager.Instance != null) GameManager.Instance.OnWeaponsUpdated -= DoUpdated;
            if (GameManager.Instance.FarmToolManager != null) GameManager.Instance.FarmToolManager.UnregisterListener(SetCurrentToolActive);
        }

        protected virtual void DoUpdated(Item item)
        {
            if (!ValidateItem(item)) return;

            foreach (var slot in Slots)
            {
                if (slot.Active && slot.InventoryCard.ID == item.Data.ID)
                {
                    slot.InventoryCard.SetLabel(item.Amount.ToString());
                    return;
                }
                else if (!slot.Active)
                {
                    slot.SetCardActive(true);
                    slot.InventoryCard.Set(item.Data.ID, item.Amount.ToString(), item.Data.Display);
                    SetSlot(slot, item);
                    return;
                }
            }
            Debug.Log("Not enough slots for inventory");
        }
        protected virtual void SetSlot(InventoryUISlot slot, Item item)
        {
            
        }
        public void SetItemSelected(string itemID, bool selected)
        {
            foreach (var slot in Slots)
            {
                if(!string.IsNullOrEmpty(itemID) && slot.InventoryCard.ID == itemID)
                {
                    slot.SetSlotSelected(selected);
                } else
                {
                    slot.SetSlotSelected(false);
                }
            }
        }
    }
}