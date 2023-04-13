using Items;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FarmToolStateManager3D : MonoBehaviour, IFarmToolStateManager
{
    [SerializeField] private List<MeshFarmToolCollection> FarmToolCollection = new List<MeshFarmToolCollection>();

    public Action<MeshFarmToolCollection> OnToolSwapped;
    private void Start()
    {
        foreach (var tool in FarmToolCollection) {
            GameManager.Instance.AddWeapon(tool.Data, 1);
        }
        SwapTool(TOOL_TYPE.Hands);
    }

    public IFarmToolCollection GetCurrentTool()
    {
        Item currentItem = GameManager.Instance.GetWeapon();
        if (currentItem != null && currentItem.Data is ToolData currentTool)
        {
            if (TryGetTool(currentTool.ToolType, out IFarmToolCollection tool))
            {
                return tool;
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
    public void SwapTool(TOOL_TYPE type)
    {
        foreach (var item in FarmToolCollection)
        {
            if(item.Data.ToolType == type)
            {
                if (GameManager.Instance.SetWeapon(item.Data.ID))
                {
                    OnToolSwapped?.Invoke(item);
                    return;
                }
            }
        }
    }
    public TOOL_TYPE NextTool()
    {
        Item currentItem = GameManager.Instance.GetWeapon();
        
        
        int current = FarmToolCollection.FindIndex((x) =>
        {
            return currentItem != null && currentItem.Data is ToolData currentTool && x.Data.ToolType == currentTool.ToolType;            
        });
        int next = (int)Mathf.Repeat(current +1, FarmToolCollection.Count);
        return FarmToolCollection[next].Data.ToolType;
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
        foreach (var item in FarmToolCollection)
        {
            yield return item;
        }
    }

    public bool TryGetTool(TOOL_TYPE tool, out IFarmToolCollection collection)
    {
        collection = null;
        foreach (var item in FarmToolCollection)
        {
            if (item.Data.ToolType == tool)
            {
                collection = item;
                return true;
            }
        }
        return false;
    }
}
