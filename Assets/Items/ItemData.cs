using UnityEngine;
namespace Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Create New Item")]
    public class ItemData : ObjData
    {
        public virtual SELECTABLE_TYPE DataType { get; } = SELECTABLE_TYPE.item;
        public int PurchasePrice;
        public int SellPrice;
    }
}