using Items;
using System;
using System.Collections.Generic;

public class MarketManager
{
    public event Action<IEnumerable<Item>> OnMarketSet;
    public event Action<Item> OnMarketUpdate;    
    private Inventory MarketInventory;
    private Dictionary<string, Wallet> MarketWallets = new Dictionary<string, Wallet>();
    public MarketManager()
    {
        MarketInventory = new Inventory();
        MarketInventory.AutoCleanup = false;
    }
    public void SetInventory(List<Item> items)
    {
        foreach (Item item in items)
        {
            MarketInventory.AddItem(item.Data, item.Amount);
        }
        MarketInventorySet();
    }

    public void MarketInventorySet()
    {
        OnMarketSet?.Invoke(MarketInventory.GetAll());
    }
    private void ItemUpdated(Item item)
    {
        OnMarketUpdate?.Invoke(item);
    }
    public IEnumerable<Item> GetItems()
    {
        return MarketInventory.GetAll();
    }
    public bool CreateWallet(string market)
    {
        return MarketWallets.TryAdd(market, new Wallet());
    }
    public bool GetWallet(string market, out Wallet wallet)
    {
        if (!MarketWallets.TryGetValue(market, out wallet)) return false;
        return true;
    }
    public bool UpdateWallet(string market, int amount, out Wallet wallet)
    {        
        if (!GetWallet(market, out wallet)) return false;
        return wallet.Modify(amount);
    }
    public bool CheckWalletBalance(string market, int amount)
    {
        if (!MarketWallets.TryGetValue(market, out Wallet wallet)) return false;
        return wallet.CheckAmount(amount);
    }
    public bool ModifyItem(ItemData item, int amountToModify)
    {
        if(MarketInventory.ModifyAmount(item, amountToModify)){
            ItemUpdated(MarketInventory[item.ID]);
            return true;
        }
        return false;
    }
    public bool CheckItemStock(ItemData item, int amount)
    {
        return MarketInventory.Contains(item, amount);
    }
}
