using UnityEngine.UI;

public class ThemedScrollRect : ThemedPanel
{
    public ScrollRect Scroll;
    public override void UpdateTheme()
    {
        base.UpdateTheme();
        ScrollRectThemeData t;
        if (Override != null)
        {
            t = Override as ScrollRectThemeData;
        }
        else
        {
            t = UIThemeManager.ScrollRectThemeData;
        }
        Scroll.scrollSensitivity = t.ScrollSensitivity;
        Scroll.movementType = t.MovementType;
        Scroll.inertia = t.Intertia;
        Scroll.decelerationRate = t.DecelerationRate;
        if (Scroll.horizontal)
        {
            Scroll.horizontalScrollbarVisibility = t.HorizontalVisibility;
            Scroll.horizontalScrollbarSpacing = t.HorizontalScrollbarSpacing;
        }
        if (Scroll.vertical)
        {
            Scroll.verticalScrollbarVisibility = t.VerticalVisibility;
            Scroll.verticalScrollbarSpacing = t.VerticalScrollbarSpacing;
        }
    }
}
