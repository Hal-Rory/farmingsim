using UnityEngine;

[CreateAssetMenu(fileName = "LabelThemeData_", menuName = "Themes/New Label Theme")]
public class LabelThemeData : UIThemeData
{
    public Font Font;
    public Color FontColor = Color.black;
    public int FontSize = 15;

    public override void Copy(UIThemeData other)
    {
        if(other is LabelThemeData labelTheme)
        {
            Font = labelTheme.Font;
            FontColor = labelTheme.FontColor;
            FontSize= labelTheme.FontSize;
        }
    }
}
