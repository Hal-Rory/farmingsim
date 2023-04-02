using UnityEngine.UI;

public class ThemedSelectable : ThemedUI
{
    public Selectable Selectable;

    public override void UpdateTheme()
    {
        SelectableThemeData t;
        if (Override != null)
        {
            t = Override as SelectableThemeData;
        }
        else
        {
            t = UIThemeManager.SelectableThemeData;
        }
        Selectable.colors = t.Colors;        
    }
}
