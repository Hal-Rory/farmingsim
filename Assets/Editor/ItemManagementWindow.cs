using Items;
using UnityEditor;
using UnityEngine;

public class ItemManagementWindow : ObjDataManagementWindowBase<ItemData>
{
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        if (HasOpenInstances<ItemManagementWindow>())
        {
            ItemManagementWindow window = (ItemManagementWindow)GetWindow(typeof(ItemManagementWindow));
            if (window != null)
            {
                Debug.Log("recompiling item management");
                window.Setup();
            }
        }
    }
    private static string ItemPath => "Items/Resources/Items";
    [MenuItem("Management/Item Management")]
    static void Init()
    {
        ItemManagementWindow window = (ItemManagementWindow)GetWindow(typeof(ItemManagementWindow));
        window.Setup();
        window.Show();
    }
    public override void Setup()
    {
        AllItems.Clear();
        foreach (var item in AssetUtilities.FindAssetsByType<ItemData>())
        {
            if(item is ItemData && item is not WeaponData && item is not SeedData)
            {
                AllItems.Add(item);
            }
        }
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        //left side
        GUILayout.BeginVertical();
        ItemScrollView();
        CreationButton<ItemData>("Create New Item", ItemPath);
        GUILayout.EndVertical();

        //right side
        GUILayout.BeginVertical();
        ItemDisplay();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}
