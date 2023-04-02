using System;
using System.IO;
using UnityEngine;
using Unity.VisualScripting.FullSerializer;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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
    public static bool Valid => _highlightThemeData != null && _labelThemeData != null && _selectableThemeData != null && _panelThemeData!= null && _scrollRectThemeData!= null;
    public static string DefaultThemeName => "Theme";
    private static string ThemePath => Path.Combine("Assets", "UI", "ThemeData", "Themes");
    private static string PrefabPath => Path.Combine("Assets", "UI", "Prefabs");
    public static string CurrentTheme =  DefaultThemeName;
    public static LabelThemeData LabelThemeData { get => _labelThemeData; }
    public static PanelThemeData PanelThemeData { get => _panelThemeData; }
    public static ScrollRectThemeData ScrollRectThemeData { get => _scrollRectThemeData; }
    public static SelectableThemeData SelectableThemeData { get => _selectableThemeData; }
    public static HighlightThemeData HighlightThemeData { get => _highlightThemeData; }

    public static void UpdateTheme()
    {
#if UNITY_EDITOR
        string[] guids = AssetDatabase.FindAssets("", new[] { PrefabPath });

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
                PrefabUtility.SavePrefabAsset(go);
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
    
    public static void SaveAndOverwriteTheme(string themeName)
    {
#if UNITY_EDITOR
        if (_highlightThemeData == null)
        {
            if (TryFind($"{nameof(HighlightThemeData)}_{themeName}", out HighlightThemeData ui))
            {
                _highlightThemeData.Copy(ui);
            }
            else
            {
                _highlightThemeData = ScriptableObject.CreateInstance<HighlightThemeData>();
                AssetDatabase.CreateAsset(_highlightThemeData, $"{ThemePath}/{nameof(HighlightThemeData)}_{themeName}.asset");
            }
        }
        _highlightThemeData.ID = $"{nameof(HighlightThemeData)}_{themeName}";
        EditorUtility.SetDirty(_highlightThemeData);

        if (_labelThemeData == null)
        {
            if (TryFind<LabelThemeData>($"{nameof(LabelThemeData)}_{themeName}", out LabelThemeData ui))
            {
                _labelThemeData.Copy(ui);
            }
            else
            {
                _labelThemeData = ScriptableObject.CreateInstance<LabelThemeData>();
                AssetDatabase.CreateAsset(_labelThemeData, $"{ThemePath}/{nameof(LabelThemeData)}_{themeName}.asset");
            }
        }
        _labelThemeData.ID = $"{nameof(LabelThemeData)}_{themeName}";
        EditorUtility.SetDirty(_labelThemeData);

        if (_panelThemeData == null)
        {
            if (TryFind($"{nameof(PanelThemeData)}_{themeName}", out PanelThemeData ui))
            {
                _panelThemeData.Copy(ui);
            }
            else
            {
                _panelThemeData = ScriptableObject.CreateInstance<PanelThemeData>();
                AssetDatabase.CreateAsset(_panelThemeData, $"{ThemePath}/{nameof(PanelThemeData)}_{themeName}.asset");
            }
        }
        _panelThemeData.ID = $"{nameof(PanelThemeData)}_{themeName}";
        EditorUtility.SetDirty(_panelThemeData);
                
        if (_scrollRectThemeData == null)
        {
            if (TryFind($"{nameof(ScrollRectThemeData)}_{themeName}", out ScrollRectThemeData ui))
            {
                _scrollRectThemeData.Copy(ui);
            }
            else
            {
                _scrollRectThemeData = ScriptableObject.CreateInstance<ScrollRectThemeData>();
                AssetDatabase.CreateAsset(_scrollRectThemeData, $"{ThemePath}/{nameof(ScrollRectThemeData)}_{themeName}.asset");
            }
        }
        _scrollRectThemeData.ID = $"{nameof(ScrollRectThemeData)}_{themeName}";
        EditorUtility.SetDirty(_scrollRectThemeData);
        
        if (_selectableThemeData == null)
        {
            if (TryFind($"{nameof(SelectableThemeData)}_{themeName}", out SelectableThemeData ui))
            {
                _selectableThemeData.Copy(ui);
            }
            else
            {
                _selectableThemeData = ScriptableObject.CreateInstance<SelectableThemeData>();
                AssetDatabase.CreateAsset(_selectableThemeData, $"{ThemePath}/{nameof(SelectableThemeData)}_{themeName}.asset");
            }
        }
        _selectableThemeData.ID = $"{nameof(SelectableThemeData)}_{themeName}";
        EditorUtility.SetDirty(_selectableThemeData);
        AssetDatabase.SaveAssets();
#endif
    }
    public static void LoadOrCreateTheme(string themeName)
    {
#if UNITY_EDITOR
        if (TryFind($"{nameof(HighlightThemeData)}_{themeName}", out HighlightThemeData hl))
        {
            _highlightThemeData = hl;
        }
        else
        {
            _highlightThemeData = ScriptableObject.CreateInstance<HighlightThemeData>();
            _highlightThemeData.ID = $"{nameof(HighlightThemeData)}_{themeName}";
            AssetDatabase.CreateAsset(_highlightThemeData, $"{ThemePath}/{_highlightThemeData.ID}.asset");
            AssetDatabase.SaveAssets();
        }

        if (TryFind($"{nameof(LabelThemeData)}_{themeName}", out LabelThemeData l))
        {
            _labelThemeData = l;
        } else 
        {
            _labelThemeData = ScriptableObject.CreateInstance<LabelThemeData>();
            _labelThemeData.ID = $"{nameof(LabelThemeData)}_{themeName}";
            AssetDatabase.CreateAsset(_labelThemeData, $"{ThemePath}/{_labelThemeData.ID}.asset");
            AssetDatabase.SaveAssets();
        }

        if (TryFind($"{nameof(PanelThemeData)}_{themeName}", out PanelThemeData p))
        {
            _panelThemeData = p;
        }
        else
        {
            _panelThemeData = ScriptableObject.CreateInstance<PanelThemeData>();
            _panelThemeData.ID = $"{nameof(PanelThemeData)}_{themeName}";
            AssetDatabase.CreateAsset(_panelThemeData, $"{ThemePath}/{_panelThemeData.ID}.asset");
            AssetDatabase.SaveAssets();
        }

        if (TryFind($"{nameof(ScrollRectThemeData)}_{themeName}", out ScrollRectThemeData sr))
        {
            _scrollRectThemeData = sr;
        }
        else
        {
            _scrollRectThemeData = ScriptableObject.CreateInstance<ScrollRectThemeData>();
            _scrollRectThemeData.ID = $"{nameof(ScrollRectThemeData)}_{themeName}";
            AssetDatabase.CreateAsset(_scrollRectThemeData, $"{ThemePath}/{_scrollRectThemeData.ID}.asset");
            AssetDatabase.SaveAssets();
        }

        if (TryFind($"{nameof(SelectableThemeData)}_{themeName}", out SelectableThemeData sl))
        {
            _selectableThemeData = sl;
        }
        else
        {
            _selectableThemeData = ScriptableObject.CreateInstance<SelectableThemeData>();
            _selectableThemeData.ID = $"{nameof(SelectableThemeData)}_{themeName}";
            AssetDatabase.CreateAsset(_selectableThemeData, $"{ThemePath}/{_selectableThemeData.ID}.asset");
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
