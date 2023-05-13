using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleCard : Card
{
    [SerializeField] private Toggle Selectable;
    private Color LabelColorActive;
    public Color LabelColorInactive = Color.white;    
    private void Awake()
    {
        if(Label != null)
        LabelColorActive = Label.color;
        Interactable = Selectable.interactable;
    }
    public bool Interactable { get => Selectable.interactable; set
        {
            Selectable.interactable = value;
            if (Label != null)
                Label.color = value ? LabelColorActive : LabelColorInactive;
        }
    }
    public void SetGroup(ToggleGroup group)
    {
        Selectable.group = group;
    }
    public void SetIsOnWithoutNotify(bool value)
    {
        Selectable.SetIsOnWithoutNotify(value);
    }
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
    public override void SetEmpty(string label = "")
    {
        base.SetEmpty(label);
        Interactable = false;
    }
}
