using UnityEngine;
using UnityEngine.UI;

public class ProfileInfoPanel : InfoHeader
{
    [SerializeField] private Image Bust;    

    public void SetPanel(string id, string header, string info, Sprite bust)
    {
        SetHeader(id, header);
        SetInfo(info);
        SetBust(bust);
    }
    public void SetBust(Sprite bust)
    {
        Bust.sprite = bust;
    }
    public override void Clear()
    {
        base.Clear();
        SetBust(null);
    }
}
