using UnityEngine.UI;

public static class ColorBlockExtensions
{
    public static ColorBlock Copy(this ColorBlock other)
    {
        ColorBlock colors = new ColorBlock();
        colors.normalColor= other.normalColor;
        colors.highlightedColor= other.highlightedColor;
        colors.colorMultiplier= other.colorMultiplier;
        colors.disabledColor= other.disabledColor;
        colors.selectedColor= other.selectedColor;
        colors.pressedColor= other.pressedColor;
        colors.fadeDuration= other.fadeDuration;

        return colors;
    }
}
