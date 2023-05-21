using Items;
using UnityEngine;
using UnityEngine.Assertions;

public class TreePlot : PropItem, IDamageable
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
    public override void Interact(TOOL_TYPE tool)
    {
        if (tool == ToolNeeded && IsDone && IsAlive)
        {
            //TakeDamage(FarmManager.GetTool(tool).Data.Attack);
            if (!IsAlive)
            {
                GameManager.Instance.AddItem(ItemReturned.Data, ItemReturned.Amount);
            }
        }
    }
    #region TimeListener
    public override void ClockUpdate(int tick)
    {
        if (!IsDone)
        {
            GrowthTick+= tick;
            IsDone = GrowthTick >= GrowthTickMax;
        }
    }
    #endregion
    #region Damageable
    [field:SerializeField] public float Health { get; private set; } = 10;

    public bool IsAlive => Health > 0;

    public void TakeDamage(float amount)
    {
        Health -= amount;
    }

    public void HealDamage(float amount)
    {
        Health += amount;
    }
    #endregion
}
