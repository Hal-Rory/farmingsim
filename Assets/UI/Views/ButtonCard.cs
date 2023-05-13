using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonCard : Card
{
    [SerializeField] private Button Selectable;
    private Color LabelColorActive;
    public Color LabelColorInactive = Color.white;
    private void Awake()
    {
        if (Label != null)
            LabelColorActive = Label.color;
        Interactable = Selectable.interactable;
    }
    public bool Interactable
    {
        get => Selectable.interactable; set
        {
            Selectable.interactable = value;
            if (Label != null)
                Label.color = value ? LabelColorActive : LabelColorInactive;
        }
    }

    public void Set(string ID, string label, Sprite icon, UnityAction callback)
    {
        Set(ID, label, icon);
        SetAction(callback);
    }

    public void SetAction(UnityAction callback)
    {
        Selectable.onClick.RemoveAllListeners();
        if(callback != null) Selectable.onClick.AddListener(callback);
    }

    public override void SetEmpty(string label = "")
    {
        base.SetEmpty(label);
        Interactable = false;
    }
}
