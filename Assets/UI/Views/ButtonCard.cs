using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonCard : Card
{
    public Button Selectable;
    public bool Interactable { get => Selectable.interactable; set => Selectable.interactable = value; }

    public void Set(string ID, string label, Sprite icon, UnityAction callback)
    {
        Set(ID, label, icon);
        Selectable.onClick.RemoveAllListeners();
        Selectable.onClick.AddListener(callback);
    }

    public override void SetEmpty(string label)
    {
        base.SetEmpty(label);
        Interactable = false;
    }
}
