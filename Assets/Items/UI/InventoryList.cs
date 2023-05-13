using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class InventoryList<T>
    where T : Card
{
    [SerializeField] private ListViewScroll Display;
    private Dictionary<string, T> Cards = new Dictionary<string, T>();
    public bool DisplayEmpty;

    public void SetActive(bool active)
    {
        Display.gameObject.SetActive(active);
    }
    public bool UpdateCard(Item item, Action<Item, T> Setup)
    {
        if (Cards.TryGetValue(item.Data.ID, out T card))
        {
            if (item.Amount <= 0 && !DisplayEmpty)
            {
                RemoveCard(item.Data.ID);
                return true;
            }
            Setup?.Invoke(item, card);
            return true;
        }
        return false;
    }
    public void FindMissing(IEnumerable<string> allIDs)
    {
        string[] cards = Cards.Keys.ToArray();
        foreach (var id in cards)
        {
            if (!allIDs.Contains(id))
            {
                RemoveCard(id);
            }
        }
    }

    public void AddCard(Item item, GameObject prefab, Action<Item, T> Setup)
    {
        T card = Display.AddCard<T>(item.Data.ID, prefab);
        card.Set(item.Data.ID);
        Cards.Add(item.Data.ID, card);
        Setup?.Invoke(item, card);
    }

    public void RemoveCard(string id)
    {
        Display.RemoveCard(id);
        Cards.Remove(id);
    }
}
