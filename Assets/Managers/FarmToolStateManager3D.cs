using Items;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FarmToolStateManager3D : MonoBehaviour, IFarmToolStateManager
{
    [SerializeField] private List<MeshFarmToolCollection> FarmToolCollection = new List<MeshFarmToolCollection>();

    public Action<MeshFarmToolCollection> OnToolSwapped;
    public Action<MeshFarmToolCollection> OnToolUpdated;
    private void Start()
    {
        GameManager.Instance.OnToolUpdated += DoToolUpdated;        
        TrySwapTool(TOOL_TYPE.Hands);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnToolUpdated -= DoToolUpdated;
    }

    private void DoToolUpdated(Item obj)
    {
        if (obj == null) return;
        foreach (var tool in FarmToolCollection)
        {
            if(tool.Data.ToolType == ((ToolData)obj.Data).ToolType)
            {
                OnToolSwapped?.Invoke(tool);
            }
        }        
    }

    public IFarmToolCollection GetCurrentTool()
    {
        Item currentItem = GameManager.Instance.GetToolbelt();
        if (currentItem != null) return null;
        
            foreach (var item in FarmToolCollection)
            {
                if (item.Data.ID == currentItem.Data.ID)
                {
                    return item;
                }
            }
        return null;
    }

    public string GetCurrentToolName()
    {
        IFarmToolCollection tool = GetCurrentTool();
        return tool != null ? tool.Data.Name : string.Empty;
    }
    public TOOL_TYPE GetCurrentToolType()
    {
        IFarmToolCollection tool = GetCurrentTool();
        return tool != null ? tool.Data.ToolType : TOOL_TYPE.None;
    }
    public void TrySwapTool(TOOL_TYPE type)
    {
        HashSet<Item> tools = new HashSet<Item>(GameManager.Instance.GetAllTools());
        foreach (var item in tools)
        {
            if(((ToolData)item.Data).ToolType == type){
                GameManager.Instance.SetToolbelt(item.Data.ID);
                return;
            }
        }
    }
    public TOOL_TYPE NextTool()
    {        
        return ((ToolData)GameManager.Instance.GetNextToolbelt()?.Data).ToolType;
    }
    public void RegisterListener(Action<IFarmToolCollection> listener)
    {
        OnToolSwapped += listener;
    }
    public void UnregisterListener(Action<IFarmToolCollection> listener)
    {
        OnToolSwapped -= listener;
    }

    public IEnumerable<IFarmToolCollection> GetTools()
    {
        HashSet<Item> tools = new HashSet<Item>(GameManager.Instance.GetAllTools());
        foreach (var item in tools)
        {
            yield return FarmToolCollection.Find(x => x.Data.ID == item.Data.ID);
        }        
    }

    protected bool TryGetTool(string toolID, out IFarmToolCollection collection)
    {
        collection = null;
        foreach (var item in FarmToolCollection)
        {
            if (item.Data.ID == toolID)
            {
                collection = item;
                return true;
            }
        }
        return false;
    }
}
