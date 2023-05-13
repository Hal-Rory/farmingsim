using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SelectableThemeData_", menuName = "Themes/New Selectable Theme")]
public class SelectableThemeData : UIThemeData
{
    public ColorBlock Colors =
        new ColorBlock()
        {
            colorMultiplier = 1,
            disabledColor = Color.white,
            highlightedColor = Color.white,
            normalColor = Color.white,
            pressedColor = Color.white,
            selectedColor = Color.white
        };
    
    public override void Copy(UIThemeData other)
    {
        if(other is SelectableThemeData selectableTheme)
        {
            Colors = selectableTheme.Colors.Copy();
        }
    }
}
