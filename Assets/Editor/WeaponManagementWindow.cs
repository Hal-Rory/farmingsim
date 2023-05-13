using Items;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponManagementWindow : ObjDataManagementWindowBase<WeaponData>
{
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        if (HasOpenInstances<WeaponManagementWindow>()) {
            WeaponManagementWindow window = (WeaponManagementWindow)GetWindow(typeof(WeaponManagementWindow));
            if (window != null)
            {
                Debug.Log("recompiling weapon management");
                window.Setup();
            }
        }
    }

    private static string WeaponPath => "Weapons/Resources/Weapons";
    private static string ToolPath => "Weapons/Resources/Tools";
    [MenuItem("Management/Weapon Management")]
    static void Init()
    {        
        WeaponManagementWindow window = (WeaponManagementWindow)GetWindow(typeof(WeaponManagementWindow));
        window.Setup();
        window.Show();
    }

    public override void Setup()
    {
        AllItems = new List<WeaponData>(AssetUtilities.FindAssetsByType<WeaponData>());
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        //left side
        GUILayout.BeginVertical();
        ItemScrollView();
        CreationButton<WeaponData>("Create New Weapon", WeaponPath);
        CreationButton<ToolData>("Create New Tool", ToolPath);
        GUILayout.EndVertical();
        
        //right side
        GUILayout.BeginVertical();
        ItemDisplay();        
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}
