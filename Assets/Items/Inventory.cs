using Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[Serializable]
public class Inventory
{
    private List<Item> Items = new List<Item>();
    public int CurrentIndex = -1;
    public int Count => Items.Count;
    public int Max = 1;
    public delegate void StateChangedHandler(Item item);
    public event StateChangedHandler OnUpdated;
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

    public void Set(int direction)
    {
        if (Items.Count <= 0) CurrentIndex = -1;
        else
        {
            CurrentIndex = (int)Mathf.Repeat(CurrentIndex + direction, Items.Count+1);
            if (CurrentIndex == Items.Count)
                CurrentIndex = -1;
        }
    }
    public Item Get()
    {
        return CurrentIndex != -1 ? this[CurrentIndex] : null;
    }
    public IEnumerable<Item> GetAll()
    {
        return Items;
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
    public bool ModifyAmount(ObjData selection, int amount)
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
    public bool RemoveItem(ObjData selection, int amount)
    {
        Item item = this[selection.ID];
        if (item == null) return false;
        int adjusted = Math.Abs(amount);
        item.Amount -= adjusted;
        if (item.Amount <= 0)
        {
            Items.Remove(item);
        }
        Set(0);
        OnUpdated?.Invoke(item);
        return true;
    }
    /// <summary>
    /// Add item
    /// </summary>
    /// <param name="selection"></param>
    /// <param name="amount">Will take abs of this value</param>
    /// <returns></returns>
    public bool AddItem(ObjData selection, int amount)
    {
        if (Count >= Max) return false;
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

    public bool Contains(ObjData selection)
    {
        return this[selection.ID] != null;
    }

    public bool Contains(ObjData selection, int amount)
    {
        Item item = this[selection.ID];
        return item != null && item.Amount >= amount;
    }
}
