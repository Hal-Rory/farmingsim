using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScrollRectThemeData_", menuName = "Themes/New ScrollRect Theme")]
public class ScrollRectThemeData : PanelThemeData
{
    public float ScrollSensitivity = 1;
    public ScrollRect.MovementType MovementType;
    public bool Intertia;
    public float DecelerationRate = .135f;
    public ScrollRect.ScrollbarVisibility VerticalVisibility;
    public float VerticalScrollbarSpacing;
    public ScrollRect.ScrollbarVisibility HorizontalVisibility;
    public float HorizontalScrollbarSpacing;
}
