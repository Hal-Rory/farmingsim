using Items;
using System;
using System.Collections.Generic;

public interface IFarmToolStateManager
{
    public IEnumerable<IFarmToolCollection> GetTools();
    public TOOL_TYPE NextTool();
    public void RegisterListener(Action<IFarmToolCollection> listener);
    public void UnregisterListener(Action<IFarmToolCollection> listener);    
    public IFarmToolCollection GetCurrentTool();
    public void TrySwapTool(TOOL_TYPE type);    
}
