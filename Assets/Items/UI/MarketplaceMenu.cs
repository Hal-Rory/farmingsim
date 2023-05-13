using Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarketplaceMenu : UIPage
{
    [SerializeField] private string MarketName;

    [SerializeField] private AmountWindow AmountWindow;
    [SerializeField] private ProfileInfoPanel HeaderPanel;
    [SerializeField] private Card TooltipCard;
    [SerializeField] private GameObject CardPrefab;
    [SerializeField] private InventoryList<ButtonCard> MarketInventory = new InventoryList<ButtonCard>();
    [SerializeField] private InventoryList<ButtonCard> PlayerInventory = new InventoryList<ButtonCard>();
    private MarketManager MarketManager => GameManager.Instance.MarketManager;
    private bool MarketActive = true;

    private void Awake()
    {
        AmountWindow.OnCancelled = () => SetAmountWindowActive(false);

        MarketManager.OnMarketUpdate += UpdateMarketInventory;
        MarketManager.OnMoneyUpdated += DoMarketWalletUpdated;

        GameManager.Instance.OnItemUpdated += UpdatePlayerInventory;
        GameManager.Instance.OnMoneyUpdated += DoMoneyUpdated;
    }

    private void DoMarketWalletUpdated(string market, int amount)
    {
        if (market != MarketName) return;
        if(MarketActive)
        {
            HeaderPanel.SetHeader(MarketName, $"{MarketName}");
        }
        HeaderPanel.SetInfo($"${amount}");
    }
    private void DoMoneyUpdated(int amount)
    {
         if(!MarketActive) 
        {
            HeaderPanel.SetHeader("player", "Inventory");
        }
        HeaderPanel.SetInfo($"${amount}");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        AmountWindow.OnCancelled = null;
        if (GameManager.Instance)
        {
            MarketManager.OnMarketUpdate -= UpdateMarketInventory;
            MarketManager.OnMoneyUpdated -= DoMarketWalletUpdated;

            GameManager.Instance.OnItemUpdated -= UpdatePlayerInventory;
            GameManager.Instance.OnMoneyUpdated -= DoMoneyUpdated;
        }
    }
    private void OnEnable()
    {
        if (!string.IsNullOrEmpty(MarketName)) ChangeMarket(MarketName);
        OnOpenMarketInventory();
    }

    private void SetAmountWindowActive(bool active)
    {
        AmountWindow.gameObject.SetActive(active);
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
        DoMarketWalletUpdated(MarketName, MarketManager.GetWalletBalance(MarketName));
    }

    public void OnOpenMarketInventory()
    {
        PlayerInventory.SetActive(false);
        UpdateItems(MarketManager.GetItems(), MarketInventory);
        MarketActive = true;
        HeaderPanel.SetHeader(MarketName, $"{MarketName}");
        DoMarketWalletUpdated(MarketName, MarketManager.GetWalletBalance(MarketName));
        MarketInventory.SetActive(true);
    }
    public void OnOpenPlayerInventory()
    {
        MarketInventory.SetActive(false);
        UpdateItems(GameManager.Instance.GetInventory(SELECTABLE_TYPE.item), PlayerInventory);
        MarketActive = false;
        HeaderPanel.SetHeader(MarketName, $"{MarketName}");
        DoMoneyUpdated(GameManager.Instance.WalletBalance);
        PlayerInventory.SetActive(true);
    }

    private void UpdateMarketInventory(Item item)
    {
        UpdateItem(item, MarketInventory);
    }

    private void UpdatePlayerInventory(Item item)
    {
        UpdateItem(item, PlayerInventory);
    }

    private void SetHeader(Item item)
    {
        if (item != null && item.Amount > 0)
        {
            HeaderPanel.SetPanel(item.Data.ID, item.Data.Name, item.Data.Description, item.Data.Display);
        }
        else
        {
            HeaderPanel.SetEmpty();
        }
    }

    private void UpdateItems(IEnumerable<Item> items, InventoryList<ButtonCard> list)
    {
        foreach (Item item in items)
        {
            UpdateItem(item, list);
        }
        HashSet<string> itemIDs = new HashSet<string>(items.Select((a, b) => a.Data.ID));
        list.FindMissing(itemIDs);
    }
    private void UpdateItem(Item item, InventoryList<ButtonCard> list)
    {
        if (!list.UpdateCard(item, SetCard))
        {
            if (item.Data.Sellable) list.AddCard(item, CardPrefab, SetCard);
        }
        else
        {
            if (!item.Data.Sellable) list.RemoveCard(item.Data.ID);
        }
    }

    private void SetCard(Item item, ButtonCard card)
    {        
        void OnCardSelected()
        {
            AmountWindow.Setup(0, item.Amount);
            AmountWindow.OnAmountSet = (int amount) =>
            {
                if (MarketActive)
                {
                    BuyFromMarket(item.Data, amount);
                } else
                {
                    SellToMarket(item.Data, amount);
                }
            };

            SetAmountWindowActive(true);
        }
        card.Set(item.Data.ID,$"{item.Data.Name}({item.Amount})", item.Data.Display, OnCardSelected);
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

    private void BuyFromMarket(ItemData item, int amountPurchased)
    {
        if (MarketManager.CheckItemStock(item, amountPurchased))
        {
            if (GameManager.Instance.CheckBalance(item.SellPrice * amountPurchased) && MarketManager.UpdateWallet(MarketName, item.SellPrice * amountPurchased))
            {
                GameManager.Instance.AddItem(item, amountPurchased);
                GameManager.Instance.ModifyWallet(-item.SellPrice * amountPurchased);
                MarketManager.ModifyItem(item, -amountPurchased);
                SetAmountWindowActive(false);
            }
        }
    }
    private void SellToMarket(ItemData item, int amountSold)
    {
        if (MarketManager.CheckWalletBalance(MarketName, item.SellPrice * amountSold))
        {
            if (MarketManager.UpdateWallet(MarketName, -item.SellPrice * amountSold))
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
        return MarketManager.UpdateWallet(MarketName, amount);
    }
}
