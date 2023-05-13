using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsMenu : MonoBehaviour
{
    [SerializeField] private ListViewScroll Display;
    private Dictionary<string, Toggle> Toggles = new Dictionary<string, Toggle>();
    void Start()
    {
        InfoHeader[] items = GetComponentsInChildren<InfoHeader>();
        foreach (var item in items)
        {
            InfoHeader header = Display.AddCard<InfoHeader>(item.ID, item.gameObject, false);
            Toggles.Add(header.ID, header.GetComponentInChildren<Toggle>());
        }
    }
    private void OnDisable()
    {
        foreach (var item in Toggles.Values)
        {
            item.isOn = false;
        }
        Display.GoToTop();
    }
}
