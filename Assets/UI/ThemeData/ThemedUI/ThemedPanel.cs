using UnityEngine;
using UnityEngine.UI;

public class ThemedPanel : ThemedUI
{
    public Image Background;
    public override void UpdateTheme()
    {
        PanelThemeData t = UIThemeManager.PanelThemeData;
        Background.color = t.BackgroundColor;
    }
}
