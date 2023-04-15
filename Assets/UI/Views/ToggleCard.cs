using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleCard : Card
{
    public Toggle Selectable;
    public bool Interactable { get => Selectable.interactable; set => Selectable.interactable = value; }
    public void Set(string ID, string label, Sprite icon, UnityAction<bool> callback)
    {
        Set(ID, label, icon);
        SetAction(callback);
    }
    public void SetAction(UnityAction<bool> callback)
    {
        Selectable.onValueChanged.RemoveAllListeners();
        Selectable.onValueChanged.AddListener(callback);
    }
    public override void SetEmpty(string label)
    {
        base.SetEmpty(label);
        Interactable = false;
    }
}
