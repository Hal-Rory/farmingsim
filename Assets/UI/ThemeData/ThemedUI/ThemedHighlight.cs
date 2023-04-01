using UnityEngine.UI;

public class ThemedHighlight : ThemedUI
{
    public Image Highlight;
    public override void UpdateTheme()
    {
        Highlight.color = UIThemeManager.HighlightThemeData.HighlightColor;
    }
}
