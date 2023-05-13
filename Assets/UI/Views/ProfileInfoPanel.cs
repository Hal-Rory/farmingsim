using UnityEngine;
using UnityEngine.UI;

public class ProfileInfoPanel : InfoHeader
{
    [SerializeField] private Image Bust;
    [SerializeField] private Sprite Placeholder;

    public void SetPanel(string id, string header, string info, Sprite bust)
    {
        SetHeader(id, header);
        SetInfo(info);
        SetBust(bust);
    }
    public void SetBust(Sprite bust)
    {
        if (Bust == null) return;
        if (bust == null) Bust.sprite = Placeholder;
        else Bust.sprite = bust;
    }
    public override void SetEmpty()
    {
        base.SetEmpty();
        SetBust(Placeholder);
    }
}
