using UnityEngine.UI;

public class ThemedPanel : ThemedUI
{
    public Image Background;
    public override void UpdateTheme()
    {
        PanelThemeData t;
        if (Override != null)
        {
            t = Override as PanelThemeData;
        }
        else
        {
            t = UIThemeManager.PanelThemeData;
        }
        Background.color = t.BackgroundColor;
    }
}
