using Items;

public interface IFarmTool
{
    public void Register();
    public void Unregister();

    public void SetToolRender(IFarmToolCollection collection);
    public void SwapTool(TOOL_TYPE tool);
}
