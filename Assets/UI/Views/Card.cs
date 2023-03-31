using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour, IFilterable
{
    public Text Label;
    public Image Icon;

    [field:SerializeField] public string ID { get; private set; }

    public void SetLabel(string label)
    {
        Label.text = label;
    }
    public void SetIcon(Sprite sprite)
    {
        Icon.sprite = sprite;
    }
    public void SetIcon(bool enabled)
    {
        Icon.gameObject.SetActive(enabled);
    }
    public void Set(string id, string label, Sprite sprite)
    {
        ID = id;
        Label.text = label;
        Icon.sprite = sprite;
    }
    public virtual void SetEmpty(string label)
    {
        ID = string.Empty;
        Icon.enabled = false;
        Label.text = label;
    }
}
