using Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventoryMenu : MonoBehaviour
{
    [SerializeField] private ProfileInfoPanel ItemHeader;
    [SerializeField] private Card TooltipCard;
    [SerializeField] private List<ToggleCard> Slots = new List<ToggleCard>();
    void Start()
    {
        UpdateItems(GameManager.Instance.GetInventory(SELECTABLE_TYPE.item));
        GameManager.Instance.OnItemUpdated += SetOrAddCard;
        GameManager.Instance.OnCurrentItemUpdated += SetHeader;
    }

    private void OnEnable()
    {
        SetHeader(GameManager.Instance.GetItem());
    }
    private void OnDisable()
    {
        ItemHeader.SetEmpty();
    }
    private void SetHeader(Item item)
    {
        if (item != null && item.Amount > 0)
        {
            ItemHeader.SetPanel(item.Data.ID, item.Data.Name, item.Data.Description, item.Data.Display);
        } else
        {
            ItemHeader.SetEmpty();
        }
    }
    private void SetOrAddCard(Item item)
    {
        if (item.Data is WeaponData) return;
        ToggleCard empty = null;
        foreach (var slot in Slots)
        {
            if(slot.ID == item.Data.ID)
            {
                SetItem(item, slot);
                return;
            }
            if(empty == null && string.IsNullOrEmpty(slot.ID))
            {
                empty = slot;
            }
        }
        if(!AddItem(item, empty))
        {
            Debug.LogWarning($"Could not add {item.Data.Name}");
        }
    }
    private void SetItem(Item item, ToggleCard card)
    {
        if (card == null) return;
        if (item.Amount <= 0)
        {
            RemoveItem(card);
            return;
        }
        if (ItemHeader.ID == item.Data.ID)
        {
            if (item.Amount > 0)
            {
                ItemHeader.SetPanel(item.Data.ID, item.Data.Name, item.Data.Description, item.Data.Display);
            }
            else
            {
                ItemHeader.SetEmpty();
            }
        }

        void SetNextItem(bool active)
        {
            if (active)
            {
                GameManager.Instance.SetItem(item.Data.ID);
            } else
            {
                if(ItemHeader.ID == card.ID)
                {
                    GameManager.Instance.SetItem(null);
                }
            }
        }
        card.Set(item.Data.ID, $"{item.Amount}", item.Data.Display, SetNextItem);
        card.Interactable = item.Amount > 0;
        
        if (!card.TryGetComponent(out Hoverable hoverable))
        {
            hoverable = card.gameObject.AddComponent<Hoverable>();
        }
        hoverable.PointerEnter.RemoveAllListeners();
        hoverable.PointerExit.RemoveAllListeners();
        hoverable.PointerEnter.AddListener((hoverable) =>
        {
            TooltipCard.Set(card.ID, item.Data.Name, null);
            TooltipCard.gameObject.SetActive(true);
            TooltipManager.Instance.SetCard(hoverable, TooltipCard.gameObject, transform);
        });
        hoverable.PointerExit.AddListener((hoverable) =>
        {
            TooltipCard.gameObject.SetActive(false);
            TooltipManager.Instance.RemoveLast(transform);
        });
    }
    private bool AddItem(Item item, ToggleCard empty)
    {
        if (empty == null) return false;
        empty.Set(item.Data.ID);
        SetItem(item, empty);
        return true;
    }
    private void RemoveItem(ToggleCard card)
    {
        card.SetEmpty();
    }
    private void UpdateItems(IEnumerable<Item> items)
    {
        HashSet<string> itemIDs = new HashSet<string>(items.Select((a, b) => a.Data.ID));
        ToggleCard[] cards = Slots.ToArray();
        foreach (Item item in items)
        {
            SetOrAddCard(item);
        }
        foreach (ToggleCard card in cards)
        {
            if (!itemIDs.Contains(card.ID))
            {
                RemoveItem(card);
            }
        }                
    }
}
