using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
[ExecuteInEditMode]
public class UIThemeManager
{
    [SerializeField] private static LabelThemeData _labelThemeData;
    [SerializeField] private static PanelThemeData _panelThemeData;
    [SerializeField] private static ScrollRectThemeData _scrollRectThemeData;
    [SerializeField] private static SelectableThemeData _selectableThemeData;
    [SerializeField] private static HighlightThemeData _highlightThemeData;
    [SerializeField] private static HighlightThemeData _activeHighlightThemeData;
    [SerializeField] private static HighlightThemeData _inactiveHighlightThemeData;
    public static bool Valid => _activeHighlightThemeData != null && _inactiveHighlightThemeData != null && _highlightThemeData != null && _labelThemeData != null && _selectableThemeData != null && _panelThemeData!= null && _scrollRectThemeData!= null;
    public static string DefaultThemeName => "Theme";
    private static string ThemePath => Path.Combine("Assets", "UI", "ThemeData", "Themes");
    private static string PrefabPath => Path.Combine("Assets", "UI", "Prefabs");
    public static string CurrentTheme =  DefaultThemeName;
    public static LabelThemeData LabelThemeData { get => _labelThemeData; }
    public static PanelThemeData PanelThemeData { get => _panelThemeData; }
    public static ScrollRectThemeData ScrollRectThemeData { get => _scrollRectThemeData; }
    public static SelectableThemeData SelectableThemeData { get => _selectableThemeData; }
    public static HighlightThemeData HighlightThemeData { get => _highlightThemeData; }
    public static HighlightThemeData ActiveHighlightThemeData { get => _activeHighlightThemeData; }
    public static HighlightThemeData InactiveHighlightThemeData { get => _inactiveHighlightThemeData; }

    public static void UpdateTheme()
    {
#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets("t:Prefab");

        foreach (var guid in guids)
        {            
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if(AssetDatabase.LoadAssetAtPath<GameObject>(path) is GameObject go)
            {
                List<ThemedUI> themes = new List<ThemedUI>(go.GetComponents<ThemedUI>());
                themes.AddRange(go.GetComponentsInChildren<ThemedUI>());
                foreach (var theme in themes)
                {
                    theme.UpdateTheme();
                }
                if (themes.Count > 0)
                {
                    PrefabUtility.SavePrefabAsset(go);
                }
            }
        }
        ThemedUI[] themedGo = UnityEngine.Object.FindObjectsOfType<ThemedUI>();
        foreach (var item in themedGo)
        {
            if (!PrefabUtility.IsPartOfAnyPrefab(item))
            {
                item.UpdateTheme();
            }
        }
#endif
    }

    private static bool TryFind<T>(string other, out T theme)
        where T : UnityEngine.Object
    {
        theme = default;
#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets("", new[] { ThemePath });
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            T found = AssetDatabase.LoadAssetAtPath<T>(path);
            if (found != null && found.Equals(other))
            {
                theme = found;
                return true;
            }
        }
#endif
        return false;
    }
#if UNITY_EDITOR
    private static string ConstructThemeName(string themeType, string themeName)
    {
        return $"{themeType}_{themeName}";
    }
    private static void SetupSave<T>(ref T theme, string themeType, string themeName)
        where T: UIThemeData
    {
        if (theme == null)
        {
            if (TryFind(ConstructThemeName(themeType, themeName), out T ui))
            {
                theme.Copy(ui);
            }
            else
            {
                theme = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(theme, $"{ThemePath}/{ConstructThemeName(themeType, themeName)}.asset");
            }
        }
        theme.ID = ConstructThemeName(themeType, themeName);
        EditorUtility.SetDirty(theme);
    }
#endif
    public static void SaveAndOverwriteTheme(string themeName)
    {
#if UNITY_EDITOR
        SetupSave(ref _highlightThemeData, nameof(_highlightThemeData), themeName);
        SetupSave(ref _inactiveHighlightThemeData, nameof(_inactiveHighlightThemeData), themeName);
        SetupSave(ref _activeHighlightThemeData, nameof(_activeHighlightThemeData), themeName);
        SetupSave(ref _labelThemeData, nameof(_labelThemeData), themeName);
        SetupSave(ref _panelThemeData, nameof(_panelThemeData), themeName);       
        SetupSave(ref _scrollRectThemeData, nameof(_scrollRectThemeData), themeName);       
        SetupSave(ref _selectableThemeData, nameof(_selectableThemeData), themeName);        
        AssetDatabase.SaveAssets();
#endif
    }
#if UNITY_EDITOR
    private static void SetupLoad<T>(ref T theme, string themeType, string themeName)
        where T : UIThemeData
    {
        if (TryFind(ConstructThemeName(themeType, themeName), out T t))
        {
            theme = t;
        }
        else
        {
            theme = ScriptableObject.CreateInstance<T>();
            theme.ID = ConstructThemeName(themeType, themeName);
            AssetDatabase.CreateAsset(theme, $"{ThemePath}/{ConstructThemeName(themeType, themeName)}.asset");
            AssetDatabase.SaveAssets();
        }
    }
#endif
    public static void LoadOrCreateTheme(string themeName)
    {
#if UNITY_EDITOR        
        SetupLoad(ref _highlightThemeData, nameof(_highlightThemeData), themeName);
        SetupLoad(ref _inactiveHighlightThemeData, nameof(_inactiveHighlightThemeData), themeName);
        SetupLoad(ref _activeHighlightThemeData, nameof(_activeHighlightThemeData), themeName);
        SetupLoad(ref _labelThemeData, nameof(_labelThemeData), themeName);
        SetupLoad(ref _panelThemeData, nameof(_panelThemeData), themeName);
        SetupLoad(ref _scrollRectThemeData, nameof(_scrollRectThemeData), themeName);
        SetupLoad(ref _selectableThemeData, nameof(_selectableThemeData), themeName);
#endif
    }
}
