using UnityEngine.UI;

public class ThemedText : ThemedUI
{
    public Text Text;

    public override void UpdateTheme()
    {
        LabelThemeData t;
        if(Override != null)
        {
            t = Override as LabelThemeData;
        } else
        {
            t = UIThemeManager.LabelThemeData;
        }
        Text.color = t.FontColor;
        Text.font = t.Font;
        Text.fontSize = t.FontSize;
        if (Text.resizeTextForBestFit)
        {
            Text.resizeTextMaxSize = t.FontSize;
        }
    }
}
