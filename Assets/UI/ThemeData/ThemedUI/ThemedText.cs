using UnityEngine.UI;

public class ThemedText : ThemedUI
{
    public Text Text;

    public override void UpdateTheme()
    {
        Text.color = UIThemeManager.LabelThemeData.FontColor;
        Text.font = UIThemeManager.LabelThemeData.Font;
        Text.fontSize = UIThemeManager.LabelThemeData.FontSize;
        if (Text.resizeTextForBestFit)
        {
            Text.resizeTextMaxSize = UIThemeManager.LabelThemeData.FontSize;
        }
    }
}
