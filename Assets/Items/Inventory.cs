using Items;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    private List<Item> Items = new List<Item>();
    public int CurrentIndex = -1;
    public int Count => Items.Count;
    public int Max = 0;
    public delegate void StateChangedHandler(Item item);
    public event StateChangedHandler OnUpdated;
    public event StateChangedHandler OnCurrentSet;
    public bool AutoCleanup = true;
    public Item this[int key] { get 
        { 
            if(key < Items.Count && key >= 0)
            {
                return Items[key];
            } else
            {
                return null;
            }
        }
    }
    public Item this[string key]
    {
        get
        {
            foreach (var item in Items)
            {
                if(item.Equals(key))
                {
                    return item;
                }
            }
            return null;
        }
    }
    
    public override string ToString()
    {
        return $"{(Get() != null ? Get().Data.Name : "None")}";
    }
    public bool Set(string key, out Item item)
    {
        item = this[key];
        if (item != null) {
            CurrentIndex = Items.IndexOf(item);            
        } else
        {
            CurrentIndex= -1;
        }
        OnCurrentSet?.Invoke(CurrentIndex != -1 ? item : null);
        return CurrentIndex != -1;
    }
    public void Set(int direction)
    {
        if (Items.Count <= 0) CurrentIndex = -1;
        else
        {
            CurrentIndex = (int)Mathf.Repeat(CurrentIndex + direction, Items.Count+1);
            if (CurrentIndex == Items.Count)
                CurrentIndex = -1;
        }        
        OnCurrentSet?.Invoke(CurrentIndex != -1 ? Items[CurrentIndex] : null);
    }
    public Item Get()
    {
        return CurrentIndex != -1 ? this[CurrentIndex] : null;
    }
    public IEnumerable<Item> GetAll()
    {
        return Items;
    }
    public IEnumerable<Item> GetAll(List<SELECTABLE_TYPE> types)
    {        
        foreach (var item in Items)
        {
            if (types.Count == 0 || types.Contains(item.Data.DataType))
                yield return item;
        }
    }
    /// <summary>
    /// Add or remove amount from current item, if valid
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>If adding, returns true. If removing, returns if it was present</returns>
    public bool ModifyAmount(int amount)
    {
        if (Get() != null)
        {
            return ModifyAmount(Get().Data, amount);
        } else
        {
            return false;
        }
    }

    /// <summary>
    /// Add or remove amount from an item
    /// </summary>
    /// <param name="item"></param>
    /// <returns>If adding, returns true. If removing, returns if it was present</returns>
    public bool ModifyAmount(Item item)
    {
        return ModifyAmount(item.Data, item.Amount);
    }

    /// <summary>
    /// Add or remove amount from an item
    /// </summary>
    /// <param name="selection"></param>
    /// <param name="amount"></param>
    /// <returns>If adding, returns true. If removing, returns if it was present</returns>
    public bool ModifyAmount(ItemData selection, int amount)
    {
        if (amount >= 0)
        {
            return AddItem(selection, amount);
        } else
        {
            return RemoveItem(selection, amount);
        }
    }
    /// <summary>
    /// Remove item if it exists
    /// </summary>
    /// <param name="selection"></param>
    /// <param name="amount">Will take -abs of this value</param>
    /// <returns></returns>
    public bool RemoveItem(ItemData selection, int amount)
    {
        Item item = this[selection.ID];
        if (item == null) return false;
        int adjusted = Math.Abs(amount);
        item.Amount -= adjusted;
        if (AutoCleanup && item.Amount <= 0)
        {
            Items.Remove(item);
        }
        Set(0);
        OnUpdated?.Invoke(item);
        return true;
    }
    public void Cleanup()
    {
        Item[] keys = new Item[Items.Count];
        Items.CopyTo(keys, 0);
        foreach (var item in keys)
        {
            if (item.Amount <= 0)
            {
                Items.Remove(item);
            }
        }
    }

    /// <summary>
    /// Add item
    /// </summary>
    /// <param name="selection"></param>
    /// <param name="amount">Will take abs of this value</param>
    /// <returns></returns>
    public bool AddItem(ItemData selection, int amount)
    {
        if (Count >= Max && Max > 0) return false;
        int adjusted = Math.Abs(amount);
        Item item = this[selection.ID];
        if (item != null)
        {
            item.Amount += adjusted;
        }
        else
        {
            item = new Item(selection, adjusted);
            Items.Add(item);
        }
        Set(0);
        OnUpdated?.Invoke(item);
        return true;
    }

    public bool Contains(ItemData selection)
    {
        return this[selection.ID] != null;
    }

    public bool Contains(ItemData selection, int amount)
    {
        Item item = this[selection.ID];
        return item != null && item.Amount >= amount;
    }
}
