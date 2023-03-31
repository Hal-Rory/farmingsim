using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using static UnityEngine.UI.ScrollRect;

public class ThemeManagementWindow : EditorWindow
{
    LabelThemeData LabelThemeData => UIThemeManager.LabelThemeData;
    SelectableThemeData SelectableThemeData => UIThemeManager.SelectableThemeData;
    ScrollRectThemeData ScrollRectTheme=> UIThemeManager.ScrollRectThemeData;
    PanelThemeData PanelTheme=> UIThemeManager.PanelThemeData;
    HighlightThemeData HighlightTheme => UIThemeManager.HighlightThemeData;
    private Vector2 ScrollPos;

    [MenuItem("Theme Management/Management Window")]
    static void Init()
    {        
        UIThemeManager.LoadOrCreateTheme(UIThemeManager.CurrentTheme);
        ThemeManagementWindow window = (ThemeManagementWindow)GetWindow(typeof(ThemeManagementWindow));
        window.Show();
    }

    private void SetVerticalColorBlock(ref ColorBlock baseBlock)
    {
        GUILayout.BeginVertical();
        baseBlock.normalColor = EditorGUILayout.ColorField("Normal Color", baseBlock.normalColor);
        baseBlock.highlightedColor = EditorGUILayout.ColorField("Highlighted Color", baseBlock.highlightedColor);
        baseBlock.pressedColor = EditorGUILayout.ColorField("Pressed Color", baseBlock.pressedColor);
        baseBlock.selectedColor = EditorGUILayout.ColorField("Selected Color", baseBlock.selectedColor);
        baseBlock.disabledColor = EditorGUILayout.ColorField("Disabled Color", baseBlock.disabledColor);
        baseBlock.colorMultiplier = EditorGUILayout.Slider("Color Multiplier", baseBlock.colorMultiplier, 1, 5);
        GUILayout.EndVertical();
    }


    void OnGUI()
    {
        ScrollPos = GUILayout.BeginScrollView(ScrollPos);
        UIThemeManager.CurrentTheme = EditorGUILayout.TextField("Theme:", UIThemeManager.CurrentTheme);
        EditorGUILayout.Space(10);

        GUILayout.Label("Selectable Color Scheme", EditorStyles.boldLabel);
        GUILayout.Label("Buttons / Toggles", EditorStyles.centeredGreyMiniLabel);
        SetVerticalColorBlock(ref SelectableThemeData.Colors);

        EditorGUILayout.Space(25);

        GUILayout.Label("Label Theme", EditorStyles.boldLabel);
        
        LabelThemeData.Font = EditorGUILayout.ObjectField("Font", LabelThemeData.Font, typeof(Font), false) as Font;

        LabelThemeData.FontColor = EditorGUILayout.ColorField("Font Color", LabelThemeData.FontColor);

        LabelThemeData.FontSize = EditorGUILayout.IntField("Font Size", LabelThemeData.FontSize);               

        EditorGUILayout.Space(25);
        
        EditorGUILayout.Space(25);

        GUILayout.Label("Highlight Theme", EditorStyles.boldLabel);
        HighlightTheme.HighlightColor = EditorGUILayout.ColorField("Highlight Color", HighlightTheme.HighlightColor);

        EditorGUILayout.Space(25);

        GUILayout.Label("Panel Theme", EditorStyles.boldLabel);
        PanelTheme.BackgroundColor = EditorGUILayout.ColorField("Background Color", PanelTheme.BackgroundColor);        

        EditorGUILayout.Space(25);

        GUILayout.Label("ScrollRect Theme", EditorStyles.boldLabel);

        ScrollRectTheme.Intertia = EditorGUILayout.Toggle("Inertia", ScrollRectTheme.Intertia);
        ScrollRectTheme.DecelerationRate = EditorGUILayout.FloatField("Scroll Deceleration Rate", ScrollRectTheme.DecelerationRate);
        ScrollRectTheme.ScrollSensitivity = EditorGUILayout.FloatField("Scroll Sensitivity", ScrollRectTheme.ScrollSensitivity);
        ScrollRectTheme.MovementType= (MovementType)EditorGUILayout.Popup("Movement Type", (int)ScrollRectTheme.MovementType, System.Enum.GetNames(typeof(MovementType)));
        ScrollRectTheme.HorizontalVisibility = (ScrollbarVisibility)EditorGUILayout.Popup("Horizontal Scroll Visibility", (int)ScrollRectTheme.HorizontalVisibility, System.Enum.GetNames(typeof(ScrollbarVisibility)));
        ScrollRectTheme.HorizontalScrollbarSpacing = EditorGUILayout.FloatField("Scroll Horizontal Spacing", ScrollRectTheme.HorizontalScrollbarSpacing);
        ScrollRectTheme.VerticalVisibility = (ScrollbarVisibility)EditorGUILayout.Popup("Vertical Scroll Visibility", (int)ScrollRectTheme.VerticalVisibility, System.Enum.GetNames(typeof(ScrollbarVisibility)));
        ScrollRectTheme.VerticalScrollbarSpacing = EditorGUILayout.FloatField("Scroll Vertical Spacing", ScrollRectTheme.VerticalScrollbarSpacing);

        EditorGUILayout.Space(25);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Update"))
        {
            UIThemeManager.UpdateTheme();
        }
        if (GUILayout.Button("Save"))
        {
            UIThemeManager.UpdateTheme();
            UIThemeManager.SaveAndOverwriteTheme(UIThemeManager.CurrentTheme);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }
}
