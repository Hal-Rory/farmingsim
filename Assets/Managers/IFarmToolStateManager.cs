using Items;
using System;
using System.Collections.Generic;

public interface IFarmToolStateManager
{
    public IEnumerable<IFarmToolCollection> GetTools();
    public void TrySwapTool(TOOL_TYPE type);
    public void RegisterListener(Action<IFarmToolCollection> listener);
    public void UnregisterListener(Action<IFarmToolCollection> listener);
    public TOOL_TYPE NextTool();
    public string GetCurrentToolName();
    public TOOL_TYPE GetCurrentToolType();
    public IFarmToolCollection GetCurrentTool();
}
