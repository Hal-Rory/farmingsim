using UnityEngine.UI;

public class ThemedSelectable : ThemedUI
{
    public Selectable Selectable;

    public override void UpdateTheme()
    {
        Selectable.colors = UIThemeManager.SelectableThemeData.Colors;        
    }
}
