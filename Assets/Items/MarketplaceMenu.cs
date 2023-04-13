using Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MarketplaceMenu : MonoBehaviour
{
    [SerializeField] private string MarketName;
    [SerializeField] private InventoryList MarketInventory;
    [SerializeField] private InventoryList PlayerInventory;
    [SerializeField] private AmountWindow AmountWindow;
    private MarketManager MarketManager => GameManager.Instance.MarketManager;    
    private int Money;

    void Start()
    {
        Assert.IsFalse(string.IsNullOrEmpty(MarketName));
        ChangeMarket(MarketName);
        AmountWindow.OnCancelled = () => SetAmountWindowActive(false);
        MarketInventory.SetCardSetup(SelectMarketCard);
        MarketManager.OnMarketUpdate += DoMarketUpdate;
        MarketManager.OnMarketSet += DoMarketSet;        
        DoMarketSet(MarketManager.GetItems());
        PlayerInventory.SetCardSetup(SelectPlayerCard);
        PlayerInventory.Validation = ValidateItem;
        GameManager.Instance.PlayerInventoryManager.RegisterPlayerInventory(ref PlayerInventory);
    }

    public void ChangeMarket(string marketName)
    {
        SetAmountWindowActive(false);
        if (string.IsNullOrEmpty(marketName))
        {
            Debug.LogWarning("No market name");
            return;
        }
        MarketName = marketName;
        MarketManager.CreateWallet(MarketName);
        if (MarketManager.GetWallet(MarketName, out Wallet wallet))
        {
            Money = wallet.Balance;
        }
        MarketInventory.SetHeader($"{MarketName}", $"Vender Money: ${Money}");
    }

    private bool ValidateItem(Item item)
    {
        return item.Data is not WeaponData;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
        {
            MarketManager.OnMarketUpdate -= DoMarketUpdate;
            MarketManager.OnMarketSet -= DoMarketSet;
            GameManager.Instance.PlayerInventoryManager.UnregisterPlayerInventory(ref PlayerInventory);
        }
    }

    private void SetAmountWindowActive(bool active)
    {
        AmountWindow.gameObject.SetActive(active);
    }

    private void SelectMarketCard(Card card, Item item)
    {
        if (card == null) return;
        if (!card.TryGetComponent(out Button button))
        {
            button = card.gameObject.AddComponent<Button>();
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            AmountWindow.Setup(0, item.Amount);
            AmountWindow.OnAmountSet = (int amount) => ItemPurchasedFromMarket(item.Data, amount);
            SetAmountWindowActive(true);
        });
        button.interactable = item.Amount > 0;        
    }
    private void SelectPlayerCard(Card card, Item item)
    {
        if (card == null) return;
        if (!card.TryGetComponent(out Button button))
        {
            button = card.gameObject.AddComponent<Button>();
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            AmountWindow.Setup(0, item.Amount);
            AmountWindow.OnAmountSet = (int amount) => ItemSoldFromMarket(item.Data, amount);
            SetAmountWindowActive(true);
        });
        button.interactable = item.Amount > 0;        
    }

    private void DoMarketUpdate(Item item)
    {
        MarketInventory.SetOrAddCard(item);
    }

    private void DoMarketSet(IEnumerable<Item> items)
    {
        MarketInventory.UpdateInventory(items);
    }

    private void ItemPurchasedFromMarket(ItemData item, int amountPurchased)
    {
        if (MarketManager.CheckItemStock(item, amountPurchased))
        {
            if (GameManager.Instance.CheckBalance(item.SellPrice * amountPurchased) && TryUpdateWallet(item.SellPrice * amountPurchased))
            {
                GameManager.Instance.AddItem(item, amountPurchased);
                GameManager.Instance.ModifyWallet(-item.SellPrice * amountPurchased);
                MarketManager.ModifyItem(item, -amountPurchased);
                SetAmountWindowActive(false);
            }
        }
    }
    private void ItemSoldFromMarket(ItemData item, int amountSold)
    {
        if (MarketManager.CheckWalletBalance(MarketName, item.SellPrice * amountSold))
        {
            if (TryUpdateWallet(-item.SellPrice * amountSold))
            {
                MarketManager.ModifyItem(item, amountSold);
                GameManager.Instance.RemoveItem(item, -amountSold);
                GameManager.Instance.ModifyWallet(item.SellPrice * amountSold);
                SetAmountWindowActive(false);
            }
        }
    }
    private bool TryUpdateWallet(int amount)
    {
        if (!MarketManager.UpdateWallet(MarketName, amount, out Wallet wallet)) return false;
        Money = wallet.Balance;
        MarketInventory.SetHeader($"{MarketName}", $"Vender Money: ${Money}");
        return true;
    }
}
