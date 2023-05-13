using Items;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SeedManagementWindow : ObjDataManagementWindowBase<SeedData>
{
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        if (HasOpenInstances<SeedManagementWindow>())
        {
            SeedManagementWindow window = (SeedManagementWindow)GetWindow(typeof(SeedManagementWindow));
            if (window != null)
            {
                Debug.Log("recompiling item management");
                window.Setup();
            }
        }
    }
    private static string SeedPath => "Farm/Crops/Resources/Crops";
    [MenuItem("Management/Seed Management")]
    static void Init()
    {
        SeedManagementWindow window = (SeedManagementWindow)GetWindow(typeof(SeedManagementWindow));
        window.Setup();
        window.Show();
    }
    public override void Setup()
    {        
        AllItems = new List<SeedData>(AssetUtilities.FindAssetsByType<SeedData>());
    }
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        //left side
        GUILayout.BeginVertical();
        ItemScrollView();        
        CreationButton<ItemData>("Create New Seed", SeedPath);
        GUILayout.EndVertical();

        //right side
        GUILayout.BeginVertical();
        ItemDisplay();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}
