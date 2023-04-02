using Items;
using System;
using System.Collections.Generic;

public interface IFarmToolStateManager
{
    public IEnumerable<IFarmToolCollection> GetTools();
    public bool TryGetTool(TOOL_TYPE tool, out IFarmToolCollection collection);
    public void SwapTool(TOOL_TYPE type);
    public void RegisterListener(Action<IFarmToolCollection> listener);
    public void UnregisterListener(Action<IFarmToolCollection> listener);
    public TOOL_TYPE NextTool();
    public string GetCurrentToolName();
}
