using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFarmTool
{
    public void Register();
    public void Unregister();

    public void SetToolRender(IFarmToolCollection collection);
    public void SwapTool(TOOL_TYPE tool);
}
