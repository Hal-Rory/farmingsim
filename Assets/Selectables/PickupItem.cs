using UnityEngine;
namespace Items
{
    public class PickupItem : MonoBehaviour, ISelectable
    {
        public Item Item;
        public GameObject SelectableObject { get => gameObject; }
        [field: SerializeField] public bool Selectable { get; private set; } = true;

        public SELECTABLE_TYPE Type => Item.Data.DataType;

        public void OnDeselect()
        {
            gameObject.SetActive(false);
        }

        public void OnEndHover()
        {
            print("end hovering");
        }

        public void OnSelect()
        {
            GameManager.Instance.AddItem(Item.Data, Item.Amount);
        }

        public void OnStartHover()
        {
            print("Hover");
        }

        public void WhileHovering()
        {
        }

        public void WhileSelected()
        {
        }
    }

}