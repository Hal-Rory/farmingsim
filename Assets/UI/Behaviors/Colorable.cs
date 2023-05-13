using UnityEngine;
using UnityEngine.UI;

public class Colorable : MonoBehaviour
{
    [SerializeField] private Color PrimaryColor;
    [SerializeField] private Color SecondaryColor;
    [SerializeField] private Image Graphic;
    
    public void SetPrimary()
    {
        Graphic.color= PrimaryColor;
    }
    public void SetSecondary()
    {
        Graphic.color= SecondaryColor;
    }
    public void SetColors(Color primary, Color secondary)
    {
        PrimaryColor = primary;
        SecondaryColor = secondary;
    }
}
