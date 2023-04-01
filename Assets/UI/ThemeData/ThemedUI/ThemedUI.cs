using UnityEngine;
using UnityEditor;
public abstract class ThemedUI : MonoBehaviour, IThemed
{
    public bool ManualUpdate;
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (ManualUpdate)
        {
            ManualUpdate= false;
            UIThemeManager.LoadOrCreateTheme(UIThemeManager.CurrentTheme);
            UpdateTheme();
        }
#endif
    }
    public UIThemeData Override;
    public abstract void UpdateTheme();    
}
