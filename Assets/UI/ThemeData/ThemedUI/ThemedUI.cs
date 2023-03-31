using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThemedUI : MonoBehaviour, IThemed
{
    public bool ManualUpdate;
    private void OnValidate()
    {
        if (ManualUpdate)
        {
            ManualUpdate= false;
            UIThemeManager.LoadOrCreateTheme(UIThemeManager.CurrentTheme);
            UpdateTheme();
        }
    }
    public UIThemeData Override;
    public abstract void UpdateTheme();    
}
