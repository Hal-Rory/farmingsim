using Items;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
    private void OnEnable()
    {
        Item item = GameManager.Instance.GetItem();
        if (item != null && ItemHeader.ID != item.Data.ID)
        {
            if (item.Amount > 0)
            {
                ItemHeader.SetPanel(item.Data.ID, item.Data.Name, item.Data.Description, item.Data.Display);
            }
        }
    }
    private void OnDisable()
    {
        ItemHeader.SetEmpty();
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
            if (ItemHeader.ID != item.Data.ID)
            {
                ItemHeader.SetPanel(item.Data.ID, item.Data.Name, item.Data.Description, item.Data.Display);
                GameManager.Instance.SetItem(item.Data.ID);
            } else
            {
                GameManager.Instance.SetItem(null);
                ItemHeader.SetEmpty();
            }
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
