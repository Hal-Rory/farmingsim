using Items;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemBelt
{
    public List<Item> Items;
    private int _current;
    public Item GetCurrent()
    {
        return _current != -1 ? Items[_current] : null;
    }
    public void Set()
    {
        _current = Items.Count > 0 ? 0 : -1;
    }
    public ItemBelt(IEnumerable<Item> items)
    {
        Items = new List<Item>(items);
        _current = -1;
    }  
    public Item SetCurrentItem(string id)
    {
        foreach (var item in Items)
        {
            if(item.Data.ID  == id)
            {
                _current = Items.IndexOf(item);
                return GetCurrent();
            }
        }
        return null;
    }
    public void Swap(ItemData oldItem, Item newItem)
    {
        Remove(oldItem.ID);
        Add(newItem);
    }
    public void Add(ItemData item, int amount)
    {
        Items.Add(new Item(item, amount));
    }
    public void Add(Item item)
    {
        Items.Add(item);
    }
    public bool Remove(string itemid)
    {
        List<string> ids = Items.ConvertAll((x) => x.Data.ID);
        for (int i = 0; i < ids.Count; i++)
        {
            if (ids[i] == itemid)
            {
                Items.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    public Item NextItem()
    {
        int next = (int)Mathf.Repeat(_current + 1, Items.Count);
        return Items[next];
    }
    public IEnumerable<Item> GetAll()
    {
        foreach (var item in Items)
        {
            yield return item;
        }
    }
}
