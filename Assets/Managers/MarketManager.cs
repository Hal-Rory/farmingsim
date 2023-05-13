using Items;
using System;
using System.Collections.Generic;
[Serializable]
public class MarketManager
{
    public event Action<Item> OnMarketUpdate;    
    private Inventory MarketInventory;
    private Dictionary<string, Wallet> MarketWallets = new Dictionary<string, Wallet>();
    public event Action<string, int> OnMoneyUpdated;
    public event Action<Item> OnItemsUpdated;
    [Serializable]
    public class Market
    {
        public string ID { get; set; }
        public string MarketName { get; set; }
        public Wallet Wallet { get; set; }
        public Inventory MarketInventory { get; set; }
        public bool BidirectionalMarket { get; set; }
    }
    public MarketManager()
    {
        MarketInventory = new Inventory();
        MarketInventory.AutoCleanup = false;
        MarketInventory.OnUpdated += DoUpdated;
    }

    private void DoUpdated(Item item)
    {
        OnItemsUpdated?.Invoke(item);
    }

    public void SetInventory(List<Item> items)
    {
        foreach (Item item in items)
        {
            MarketInventory.AddItem(item.Data, item.Amount);
            OnMarketUpdate?.Invoke(item);
        }
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
    public bool UpdateWallet(string market, int amount)
    {        
        if (!GetWallet(market, out Wallet wallet)) return false;
        if (wallet.Modify(amount))
        {
            OnMoneyUpdated?.Invoke(market, amount);
            return true;
        }
        return false;
    }
    public int GetWalletBalance(string market)
    {
        if (!MarketWallets.TryGetValue(market, out Wallet wallet)) return 0;
        return wallet.Balance;
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
