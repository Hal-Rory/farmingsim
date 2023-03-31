using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemedSelectable : ThemedUI
{
    public Selectable Selectable;

    public override void UpdateTheme()
    {
        Selectable.colors = UIThemeManager.SelectableThemeData.Colors;        
    }
}
