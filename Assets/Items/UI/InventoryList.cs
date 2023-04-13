using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryList : MonoBehaviour
{
    [SerializeField] private ListView Display;
    [SerializeField] private InfoHeader Header;
    [SerializeField] private GameObject CardPrefab;
    private Dictionary<Item, Card> Cards = new Dictionary<Item, Card>();
    private Action<Card, Item> OnCardSetup;
    public Func<Item, bool> Validation;
    public bool DisplayEmpty;

    private void OnDestroy()
    {
        OnCardSetup = null;
    }
    public void SetCardSetup(Action<Card, Item> action)
    {
        OnCardSetup = action;
    }
    public void SetHeader(string label, string info)
    {
        Header.SetHeader("inventory", label);
        Header.SetInfo(info);
    }
    public void UpdateItem(Item item)
    {
        SetOrAddCard(item);
    }

    public void UpdateInventory(IEnumerable<Item> items)
    {
        HashSet<Item> remove = new HashSet<Item>(Cards.Keys);
        remove.RemoveWhere(x => !items.Contains(x));
        foreach (Item item in remove)
        {
            RemoveCard(item);
        }
        foreach (Item item in items)
        {
            SetOrAddCard(item);
        }
    }
    public void AddCard(Item item)
    {
        if (Validation?.Invoke(item) == false) return;
        Card card = Display.AddCard<Card>(item.Data.ID, CardPrefab);
        card.Set(item.Data.ID);
        SetCard(item, card);        
        Cards.Add(item, card);
    }
    public void RemoveCard(Item item)
    {
        Display.RemoveCard(item.Data.ID);
        Cards.Remove(item);
    }
    public void SetOrAddCard(Item item)
    {
        if (Cards.TryGetValue(item, out Card card))
        {
            SetCard(item, card);
        }
        else
        {
            AddCard(item);
        }
    }

    private void SetCard(Item item, Card card)
    {
        if(item.Amount <= 0 && !DisplayEmpty)
        {
            RemoveCard(item);
            OnCardSetup?.Invoke(null, item);
            return;
        }
        card.SetLabel($"{item.Data.Name}({item.Amount})");
        card.SetIcon(item.Data.Display);        
        OnCardSetup?.Invoke(card, item);
    }
}
