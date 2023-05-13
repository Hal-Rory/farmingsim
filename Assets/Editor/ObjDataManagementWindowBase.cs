using Items;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public abstract class ObjDataManagementWindowBase<T> : EditorWindow
    where T : ObjData    
{
    protected Vector2 ScrollPos;
    protected List<T> AllItems = new List<T>();
    protected T Current;
    protected string CurrentName;
    protected Editor ObjDataEditor;
    protected bool DeleteConfirmation;
    public abstract void Setup();
    public virtual void ItemScrollView()
    {
        ScrollPos = GUILayout.BeginScrollView(ScrollPos, GUILayout.Width(position.width / 3));
        for (int i = 0; i < AllItems.Count; i++)
        {
            if (GUILayout.Button($"{AllItems[i].Name}", GUILayout.Height(100)))
            {
                Current = AllItems[i];
                CurrentName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(Current));
                ObjDataEditor = Editor.CreateEditor(Current);
            }
        }
        GUILayout.EndScrollView();        
    }
    public virtual void ItemDisplay()
    {
        GUILayout.Label($"Current: {(Current == null ? "None" : Current.Name)}");
        if (Current != null)
        {
            DisplayItemPanel();
            
            EditorGUILayout.Space(25);
            if (GUILayout.Button("Delete"))
            {
                DeleteConfirmation = true;
            }
            if (DeleteConfirmation)
            {
                if (GUILayout.Button("Yes, delete this"))
                {
                    DeleteConfirmation = false;
                    if (AssetUtilities.DeleteScriptableObj(Current))
                    {
                        Current = null;
                        Setup();
                    }
                }
            }
        }
        else
        {
            DeleteConfirmation = false;
        }
    }

    protected virtual void DisplayItemPanel()
    {
        ObjDataEditor.OnInspectorGUI();
    }

    protected void CreationButton<U>(string label, string path)
         where U : ObjData
    {
        if (GUILayout.Button(label, GUILayout.Width(position.width / 3)))
        {
            CreateNew<U>(path);
        }
    }
    private void CreateNew<U>(string path)
        where U : ObjData
    {
        U obj = AssetUtilities.CreateNewScriptableObj<U>(path, $"New Item {AllItems.Count}");
        EditorUtility.FocusProjectWindow();

        Selection.activeObject = obj;
        Setup();
    }
}
