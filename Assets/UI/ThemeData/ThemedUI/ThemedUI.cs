using UnityEngine;
public abstract class ThemedUI : MonoBehaviour, IThemed
{
    public bool ManualUpdate;
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (ManualUpdate)
        {
            ManualUpdate= false;
            OnManualUpdate();
        }
#endif
    }
    public void OnManualUpdate()
    {        
        UIThemeManager.LoadOrCreateTheme(UIThemeManager.CurrentTheme);
        UpdateTheme();
    }
    public UIThemeData Override;
    public abstract void UpdateTheme();    
}
