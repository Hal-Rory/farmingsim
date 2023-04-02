using UnityEngine.UI;

public class ThemedHighlight : ThemedUI
{
    public Image Highlight;
    public override void UpdateTheme()
    {
        HighlightThemeData t;
        if (Override != null)
        {
            t = Override as HighlightThemeData;
        }
        else
        {
            t = UIThemeManager.HighlightThemeData;
        }
        Highlight.color = t;
    }
}
