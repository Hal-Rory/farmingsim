using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Items
{
    public class UIFarmToolWheel : UIPage
    {
        private Dictionary<string, InventoryUISlot> Slots = new Dictionary<string, InventoryUISlot>();
        private IFarmToolStateManager FarmToolManager => GameManager.Instance.FarmToolManager;
        public Card CurrentCard;
        public GameObject SlotPrefab;
        [SerializeField]
        private float Radius = 1;
        private InventoryUISlot SelectedSlot;
        protected override void Start()
        {
            base.Start();
            if (FarmToolManager.GetCurrentTool() != null) SetCurrentToolActive(FarmToolManager.GetCurrentTool());
            FarmToolManager.RegisterListener(SetCurrentToolActive);
            GameManager.Instance.InputManager.RegisterEquipmentListener(DoOpenMenu);
            UpdateTools(FarmToolManager.GetTools());
        }        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameManager.Instance.InputManager.UnregisterEquipmentListener(DoOpenMenu);
            if (GameManager.Instance.FarmToolManager != null) GameManager.Instance.FarmToolManager.UnregisterListener(SetCurrentToolActive);
        }
        public void DoOpenMenu(bool interact)
        {
            if (interact)
            {
                if (!gameObject.activeSelf)
                {
                    OpenFocus();
                } else
                {
                    CloseFocus();
                }
            }
        }
        private void SetCurrentToolActive(IFarmToolCollection tool)
        {
            SetItemSelected(tool.Data.ID, true);
            CurrentCard.Set(tool.Data.ID, tool.Data.Name, tool.Data.Display);
        }
        private void OnEnable()
        {
            UpdateTools(FarmToolManager.GetTools());
        }
        protected void UpdateTools(IEnumerable<IFarmToolCollection> tools)
        {
            if (tools == null) return;
            List<IFarmToolCollection> toolList = tools.ToList();
            for (int i = 0; i < tools.Count(); i++)
            {
                float radians = 2 * MathF.PI / tools.Count() * i;

                Vector3 position = (CurrentCard.transform as RectTransform).position + new Vector3(Mathf.Sin(radians), Mathf.Cos(radians)) * Radius; // Radius is just the distance away from the point
                InventoryUISlot slot;
                
                if (!Slots.TryGetValue(toolList[i].Data.ID, out slot))
                {
                    slot = Instantiate(SlotPrefab, transform).GetComponent<InventoryUISlot>();
                    Slots.Add(toolList[i].Data.ID, slot);
                }
                (slot.transform as RectTransform).position = position;
                (slot.transform as RectTransform).sizeDelta = Vector2.one * 100;
            }
        }
        public void SetItemSelected(string itemID, bool selected)
        {
            if (SelectedSlot != null) SelectedSlot.SetSlotSelected(false);
            if (Slots.TryGetValue(itemID, out InventoryUISlot slot))
            {
                slot.SetSlotSelected(selected);
                if(selected) SelectedSlot = slot;
            }
        }
    }
}