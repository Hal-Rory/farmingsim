using UnityEngine;
namespace Items
{
    public class PickupItem : MonoBehaviour, ISelectable
    {
        public Item Item;
        public GameObject SelectableObject { get => gameObject; }
        public bool Selected { get; private set; }
        public bool Hovered { get; private set; }
        private bool _selectable = true;
        public bool Selectable { get => _selectable; set => _selectable = value; }
        public bool CanReselect => false;

        private void Start()
        {
        }

        public SELECTABLE_TYPE Type => Item.Data.DataType;

        public Item Pickup()
        {
            gameObject.SetActive(false);
            return Item;
        }

        public void OnDeselect()
        {
        }

        public void OnEndHover()
        {
            Hovered = false;
            print("end hovering");
        }

        public void OnSelect()
        {
        }

        public void OnStartHover()
        {
            Hovered = true;
            print("Hover");
        }

        public void WhileHovering()
        {
            if (!Selected)
            {
            }
        }

        public void WhileSelected()
        {
        }
    }

}