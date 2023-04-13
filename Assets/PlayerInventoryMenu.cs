using Items;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryMenu : MonoBehaviour
{
    [SerializeField] private InventoryList PlayerInventory;
    
    [SerializeField] private ProfileInfoPanel ItemHeader;
    void Start()
    {
        PlayerInventory.SetCardSetup(SetCard);
        PlayerInventory.Validation = ValidateItem;
        GameManager.Instance.PlayerInventoryManager.RegisterPlayerInventory(ref PlayerInventory);
    }
    private bool ValidateItem(Item item)
    {
        return item.Data is not WeaponData;
    }
    protected void SetCard(Card card, Item item)
    {
        if (ItemHeader.ID == item.Data.ID)
        {
            if (item.Amount > 0)
            {
                ItemHeader.SetPanel(item.Data.ID, item.Data.Name, item.Data.Description, item.Data.Display);
            } else
            {
                ItemHeader.SetEmpty();
            }
        }
        if (card == null) return;        
        if (!card.TryGetComponent(out Button button))
        {
            button = card.gameObject.AddComponent<Button>();            
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(()=>
        {
            ItemHeader.SetPanel(item.Data.ID, item.Data.Name, item.Data.Description, item.Data.Display);
            GameManager.Instance.SetItem(item.Data.ID);
        });
        button.interactable = item.Amount > 0;        
    }

    public void SetTool(InventoryUISlot slot)
    {
        if (GameManager.Instance.GetWeapon(slot.InventoryCard.ID, out Item item))
        {
            GameManager.Instance.FarmToolManager.SwapTool((item.Data as ToolData).ToolType);
        }
    }
}
