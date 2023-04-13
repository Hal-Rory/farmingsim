using Items;
using System;

[Serializable]
public class Item
{
    public ItemData Data;
    public int Amount;

    public Item(ItemData item, int amount)
    {
        Data = item;
        Amount = amount;
    }
    public override bool Equals(object obj)
    {
        if (obj is Item) return ((Item)obj).Data.ID == Data.ID;
        if (obj is string) return Data.ID == ((string)obj);
        return base.Equals(obj);
    }
}
