using UnityEngine;
using UnityEngine.UI;

public class SelectableThemeData : UIThemeData
{
    public ColorBlock Colors = 
        new ColorBlock() { 
            colorMultiplier = 1, 
            disabledColor = Color.white, 
            highlightedColor = Color.white, 
            normalColor = Color.white, 
            pressedColor = Color.white, 
            selectedColor = Color.white };

    public override void Copy(UIThemeData other)
    {
        if(other is SelectableThemeData selectableTheme)
        {
            Colors = selectableTheme.Colors;
        }
    }
}
