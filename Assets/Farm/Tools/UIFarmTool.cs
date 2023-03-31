using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFarmTool : MonoBehaviour, IFarmTool
{
    [SerializeField] private ListView ListView;
    private Dictionary<TOOL_TYPE, ToggleCard> AllTools = new Dictionary<TOOL_TYPE, ToggleCard>();
    [SerializeField] private InfoHeader Header;
    [SerializeField] private GameObject CardPrefab;
    [SerializeField] private ToggleGroup Group;
    private IFarmToolStateManager ToolManager => GameManager.Instance.FarmToolManager;
    public void Register()
    {
        ToolManager.RegisterListener(SetToolRender);
    }

    public void Unregister()
    {
        ToolManager.UnregisterListener(SetToolRender);
    }
    private void OnDestroy()
    {
        Unregister();
    }
    void Start()
    {
        Header.SetHeader("tools", "Current Tool:");
        Header.SetInfo(ToolManager.GetCurrentToolName());
        foreach (IFarmToolCollection item in ToolManager.GetTools())
        {
            ToggleCard card = ListView.AddCard<ToggleCard>(item.Data.ToolType.ToString(), CardPrefab);
            void SwapToolCard(bool interact)
            {
                if (interact)
                {
                    SwapTool(item.Data.ToolType);
                }
            }
            card.Set(item.Data.ToolType.ToString(), item.Data.Name, item.Data.Display, SwapToolCard); 
            if(item.Data.ToolType == GameManager.Instance.SelectedTool)
            {
                card.Selectable.SetIsOnWithoutNotify(true);
            }
            card.Selectable.group = Group;
            AllTools.Add(item.Data.ToolType, card);
        }
        Register();
    }

    public void SetToolRender(IFarmToolCollection collection)
    {
        Header.SetInfo(collection.Data.Name);
        if(AllTools.TryGetValue(collection.Data.ToolType, out ToggleCard toggle))
        {
            toggle.Selectable.SetIsOnWithoutNotify(true);
        }
    }
    public void SwapTool(TOOL_TYPE tool)
    {
        GameManager.Instance.FarmToolManager.SwapTool(tool);
    }
}
