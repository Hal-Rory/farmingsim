using GameTime;
using Items;
using UnityEngine;
using UnityEngine.Assertions;

public class BushPlot : PropItem, ITimeListener
{
    [SerializeField] private Item ItemReturned;
    [SerializeField] private int GrowthTickMax;
    private int GrowthTick;
    private bool IsDone;
    private IFarmToolStateManager FarmManager => GameManager.Instance.FarmToolManager;
    private void Start()
    {
        ValidateProp();
    }
    private void OnEnable()
    {
        Register();
    }
    private void OnDisable()
    {
        Unregister();
    }
    protected override void ValidateProp()
    {
        Assert.IsNotNull(ItemReturned.Data, $"No item given to {nameof(BushPlot)}: {name}");
    }
    #region TimeListener
    public override void ClockUpdate(int tick)
    {
        if (!IsDone)
        {
            GrowthTick-= tick;
            IsDone = GrowthTick <= 0;
        }
    }
    #endregion
    #region PropItem
    public override void Interact(TOOL_TYPE tool)
    {
        if(tool == ToolNeeded && IsDone)
        {
            GameManager.Instance.AddItem(ItemReturned.Data, ItemReturned.Amount);
            GrowthTick = GrowthTickMax;
            IsDone= false;
        }
    }
    #endregion
}
