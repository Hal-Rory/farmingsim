using UnityEngine;
[CreateAssetMenu(fileName = "PanelThemeData_", menuName = "Themes/New Panel Theme")]
public class PanelThemeData : UIThemeData
{
    public Color BackgroundColor = Color.white;

    public override void Copy(UIThemeData other)
    {
        if(other is PanelThemeData panelTheme)
        {
            BackgroundColor= panelTheme.BackgroundColor;
        }
    }
}
