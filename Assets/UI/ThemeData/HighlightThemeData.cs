using UnityEngine;

public class HighlightThemeData : UIThemeData
{
    public Color HighlightColor = Color.white;

    public override void Copy(UIThemeData other)
    {
        if (other is HighlightThemeData highlightTheme)
        {
            HighlightColor = highlightTheme.HighlightColor;
        }
    }
}
