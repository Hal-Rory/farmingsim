using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class AssetUtilities
{
    public static IEnumerable<T> FindAssetsByType<T>() where T : Object
    {
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                yield return asset;
            }
        }
    }

    public static T CreateNewScriptableObj<T>(string path, string name)
        where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        AssetDatabase.CreateAsset(asset, Path.Combine("Assets", path, $"{name}.asset"));
        AssetDatabase.SaveAssets();
        return asset;
    }
    public static bool DeleteScriptableObj<T>(T asset)
        where T : ScriptableObject
    {
        string path = AssetDatabase.GetAssetPath(asset);
        return AssetDatabase.DeleteAsset(path);
    }
}
