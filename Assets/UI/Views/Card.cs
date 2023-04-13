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
        if (sprite == null)
        {
            Icon.enabled = false;
        }
        else
        {
            Icon.enabled = true;
            Icon.sprite = sprite;
        }
    }
    public void Set(string id)
    {
        ID = id;
    }
    public void Set(string id, string label, Sprite sprite)
    {
        ID = id;
        Label.text = label;
        SetIcon(sprite);
    }
    public virtual void SetEmpty(string label)
    {
        ID = string.Empty;
        SetIcon(null);
        Label.text = label;
    }
}
