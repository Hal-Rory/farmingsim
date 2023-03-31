using System;
using UnityEngine.UI;
using static UnityEngine.UI.ScrollRect;
[Serializable]
public class ScrollRectThemeData : PanelThemeData
{
    public float ScrollSensitivity = 1;
    public MovementType MovementType;
    public bool Intertia;
    public float DecelerationRate = .135f;
    public ScrollbarVisibility VerticalVisibility;
    public float VerticalScrollbarSpacing;
    public ScrollbarVisibility HorizontalVisibility;
    public float HorizontalScrollbarSpacing;
}
