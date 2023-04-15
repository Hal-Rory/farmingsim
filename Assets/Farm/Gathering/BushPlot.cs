using GameTime;
using System.Collections;
using System.Collections.Generic;
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
    public override void ClockUpdate(TimeStruct timestamp)
    {
        if (!IsDone)
        {
            GrowthTick--;
            IsDone = GrowthTick <= 0;
        }
    }
    #endregion
    #region Selectable
    public override void Interact()
    {
        if(FarmManager.GetCurrentToolType() == ToolNeeded && IsDone)
        {
            GameManager.Instance.AddItem(ItemReturned.Data, ItemReturned.Amount);
            GrowthTick = GrowthTickMax;
            IsDone= false;
        }
    }

    public override void OnDeselect()
    {
    }

    public override void OnEndHover()
    {
    }

    public override void OnSelect()
    {
        Interact();
    }

    public override void OnStartHover()
    {
    }

    public override void WhileHovering()
    {
    }

    public override void WhileSelected()
    {
    }
    #endregion
}
