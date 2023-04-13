using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    [field: SerializeField] public Card InventoryCard { get; private set; }
    public bool Active => InventoryCard.gameObject.activeSelf;
    public int Index;
    [SerializeField]
    private Image SelectedBackground;
    [SerializeField] private Color SelectedColor = Color.white;
    [SerializeField] private Color DeselectedColor = Color.black;
    private bool Selected;

    public void SetCardActive(bool active)
    {
        InventoryCard.gameObject.SetActive(active);
    }

    public void SetSlotSelected(bool selected)
    {
        Selected = selected;
        if (SelectedBackground == null) return;
        SelectedBackground.color = selected ? SelectedColor : DeselectedColor;
    }

}
