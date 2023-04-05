using Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FarmToolStateManager3D : MonoBehaviour, IFarmToolStateManager
{
    [SerializeField] private List<MeshFarmToolCollection> FarmToolCollection = new List<MeshFarmToolCollection>();

    public Action<MeshFarmToolCollection> OnToolSwapped;
    private void Start()
    {
        SwapTool(TOOL_TYPE.Hands);
        foreach (var tool in FarmToolCollection) {
            GameManager.Instance.AddItem(tool.Data, 1);
        }
    }
    public string GetCurrentToolName()
    {
        if(TryGetTool(GameManager.Instance.SelectedTool, out IFarmToolCollection tool))
        {
            return tool.Data.Name;
        }
        return string.Empty;
    }
    public void SwapTool(TOOL_TYPE type)
    {
        foreach (var item in FarmToolCollection)
        {
            if(item.Data.ToolType == type)
            {
                GameManager.Instance.SelectedTool = type;
                OnToolSwapped?.Invoke(item);
                return;
            }
        }
    }
    public TOOL_TYPE NextTool()
    {
        int current = FarmToolCollection.FindIndex(x => x.Data.ToolType == GameManager.Instance.SelectedTool);
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
