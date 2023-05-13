public class ThemeActivityColorable : ThemedUI
{
    public HighlightThemeData SecondaryOverride;
    public Colorable Colorable;
    public override void UpdateTheme()
    {
        HighlightThemeData p;
        HighlightThemeData s;
        if (Override != null)
        {
            p = Override as HighlightThemeData;
            s = SecondaryOverride;
        }
        else
        {
            p = UIThemeManager.ActiveHighlightThemeData;
            s = UIThemeManager.InactiveHighlightThemeData;
        }
        Colorable.SetColors(p.HighlightColor, s.HighlightColor);
    }
}
