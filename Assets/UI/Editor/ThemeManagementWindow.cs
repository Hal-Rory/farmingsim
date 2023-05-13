using UnityEngine;
using UnityEditor;

public class ThemeManagementWindow : EditorWindow
{

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        if (HasOpenInstances<ThemeManagementWindow>())
        {
            ThemeManagementWindow window = (ThemeManagementWindow)GetWindow(typeof(ThemeManagementWindow));
            if (window != null)
            {
                Debug.Log("recompiling theme management");
                UIThemeManager.LoadOrCreateTheme(UIThemeManager.CurrentTheme);
            }

        }
    }

    LabelThemeData LabelThemeData => UIThemeManager.LabelThemeData;
    SelectableThemeData SelectableThemeData => UIThemeManager.SelectableThemeData;
    ScrollRectThemeData ScrollRectTheme=> UIThemeManager.ScrollRectThemeData;
    PanelThemeData PanelTheme=> UIThemeManager.PanelThemeData;
    HighlightThemeData HighlightTheme => UIThemeManager.HighlightThemeData;
    HighlightThemeData InactiveHighlightTheme => UIThemeManager.InactiveHighlightThemeData;
    HighlightThemeData ActiveHighlightTheme => UIThemeManager.ActiveHighlightThemeData;
    private Vector2 ScrollPos;

    [MenuItem("Management/Theme Management")]
    static void Init()
    {        
        UIThemeManager.LoadOrCreateTheme(UIThemeManager.CurrentTheme);
        ThemeManagementWindow window = (ThemeManagementWindow)GetWindow(typeof(ThemeManagementWindow));
        window.Show();
    }    

    void OnGUI()
    {        
        ScrollPos = GUILayout.BeginScrollView(ScrollPos);
        UIThemeManager.CurrentTheme = EditorGUILayout.TextField("Theme:", UIThemeManager.CurrentTheme);
        EditorGUILayout.Space(10);

        GUILayout.Label("Selectable Color Scheme", EditorStyles.boldLabel);
        GUILayout.Label("Buttons / Toggles", EditorStyles.centeredGreyMiniLabel);
        Editor.CreateEditor(SelectableThemeData).OnInspectorGUI();
        EditorGUILayout.Space(25);

        GUILayout.Label("Label Theme", EditorStyles.boldLabel);

        Editor.CreateEditor(LabelThemeData).OnInspectorGUI();
                
        EditorGUILayout.Space(25);

        GUILayout.Label("Highlight Theme", EditorStyles.boldLabel);
        Editor.CreateEditor(HighlightTheme).OnInspectorGUI();

        EditorGUILayout.Space(25);

        GUILayout.Label("Activehighlight Theme", EditorStyles.boldLabel);
        Editor.CreateEditor(ActiveHighlightTheme).OnInspectorGUI();

        EditorGUILayout.Space(25);

        GUILayout.Label("Inactivehighlight Theme", EditorStyles.boldLabel);
        Editor.CreateEditor(InactiveHighlightTheme).OnInspectorGUI();

        EditorGUILayout.Space(25);

        GUILayout.Label("Panel Theme", EditorStyles.boldLabel);
        Editor.CreateEditor(PanelTheme).OnInspectorGUI(); 

        EditorGUILayout.Space(25);

        GUILayout.Label("ScrollRect Theme", EditorStyles.boldLabel);
        Editor.CreateEditor(ScrollRectTheme).OnInspectorGUI();

        EditorGUILayout.Space(25);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Update"))
        {
            UIThemeManager.UpdateTheme();
        }
        
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }
}
