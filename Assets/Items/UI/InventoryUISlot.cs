using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUISlot : MonoBehaviour
{
    [field: SerializeField] public Card InventoryCard { get; private set; }
    public bool Active => InventoryCard.gameObject.activeSelf;
    public int Index;
    public void SetCardActive(bool active)
    {
        InventoryCard.gameObject.SetActive(active);
    }
}
