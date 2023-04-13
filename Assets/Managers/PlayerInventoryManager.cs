using System;

public class PlayerInventoryManager
{
    private Action<string, string> OnHeaderUpdated;
    public PlayerInventoryManager()
    {
        GameManager.Instance.OnMoneyUpdated += DoMoneyUpdated;
    }
    ~PlayerInventoryManager()
    {
        GameManager.Instance.OnMoneyUpdated -= DoMoneyUpdated;
    }
    public void RegisterPlayerInventory(ref InventoryList inv)
    {
        inv.SetHeader($"Inventory", $"Money: ${GameManager.Instance.WalletBalance}");
        OnHeaderUpdated += inv.SetHeader;
        DoMoneyUpdated();
        inv.UpdateInventory(GameManager.Instance.GetInventory());
        GameManager.Instance.OnItemsUpdated += inv.SetOrAddCard;
        
    }
    public void UnregisterPlayerInventory(ref InventoryList inv)
    {
        GameManager.Instance.OnItemsUpdated -= inv.SetOrAddCard;
        OnHeaderUpdated -= inv.SetHeader;
    }

    private void DoMoneyUpdated(int _ = 0)
    {
        OnHeaderUpdated?.Invoke("Inventory", $"Money: ${GameManager.Instance.WalletBalance}");
    }
}
