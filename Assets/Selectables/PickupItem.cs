using UnityEngine;
namespace Items
{
    public class PickupItem : MonoBehaviour, ISelectable
    {
        public Item Item;
        public GameObject SelectableObject { get => gameObject; }        
        [field: SerializeField] public bool SelectableBySelector { get; private set; } = true;
        [field: SerializeField] public Vector3 HoverPoint { get; private set; }
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.TransformPoint(HoverPoint), .1f);
        }
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