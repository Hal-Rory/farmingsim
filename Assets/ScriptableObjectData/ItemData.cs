using UnityEngine;
using static ISelectable;

namespace Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Create New Item")]
    public class ItemData : ObjData
    {
        [field: SerializeField] public SELECTABLE_TYPE SelectableType { get; protected set; } = SELECTABLE_TYPE.item;

        public int PurchasePrice;
        public int SellPrice;
    }
}