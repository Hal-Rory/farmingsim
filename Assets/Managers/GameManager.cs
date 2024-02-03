using GameTime;
using Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using static ISelectable;
using static ITimeManager;
using static UIManager;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{   
    public static GameManager Instance;
    [field: SerializeField] public IFarmToolStateManager FarmToolManager { get; private set; }
    [field: SerializeField] public IInputManager InputManager { get; private set; }
    [field: SerializeField] public MarketManager MarketManager { get; private set; }
    [field: SerializeField] public Selector Selection { get; private set; } = new Selector();
    [field: SerializeField] public UIManager UIManager { get; private set; }
    [field: SerializeField] public CraftingManager CraftingManager { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            
            List<SeedData> crops = new List<SeedData>(Resources.LoadAll<SeedData>("Crops"));
            foreach (SeedData crop in crops)
            {
                Debug.Log("TESTING HERE");
                AddItem(crop, 2);
                AllSeedsTypes.TryAdd(crop.ID, crop);
            }
            CraftingManager = new CraftingManager();
            ToolBelt.Set();
            foreach (var item in ToolBelt.GetAll())
            {
                GameInventory.AddItem(item.Data, item.Amount);
            }
            GameInventory.OnUpdated += DoItemsUpdated;
            GameInventory.OnCurrentSet += DoCurrentItemUpdated;
            MarketManager = new MarketManager();
            TimeManager = new TimeManager(1, TimeStruct.Default);
            LightListener = new LightTimeListener();
            FarmToolManager = GetComponentInChildren<IFarmToolStateManager>();
            InputManager = GetComponentInChildren<IInputManager>();
            GameWallet = new Wallet(100);
            Selection.InputManager = InputManager;
            InputManager.RegisterPrimaryInteractionListener(Selection.Interaction);
            UIManager.OnFocusedChanged += DoFocusChanged;
            UIManager.SetBaseFocus();
            SetMarketInventory();
            LightListener.LightManager = LightManager;
            LightListener.Register();
        }        
    }
   
    private void OnDestroy()
    {
        if(Instance == this) InputManager.UnregisterPrimaryInteractionListener(Selection.Interaction);
        LightListener.Unregister();
    }    
    
    private void Update()
    {
        Selection.Update();
    }

    void OnEnable()
    {
        Crop.OnPlantCrop += DoPlantCrop;
        Crop.OnHarvestCrop += DoHarvestCrop;
        StartTime();
    }
    void OnDisable()
    {
        StopTime();
        Crop.OnPlantCrop -= DoPlantCrop;
        Crop.OnHarvestCrop -= DoHarvestCrop;
    }    

    private void DoFocusChanged(UI_STATE state)
    {
        InputManager.SetMouseFocus(state == UI_STATE.Base);
    }

    #region Items
    [SerializeField] private List<Item> MarketItems = new List<Item>();
    private Dictionary<string, SeedData> AllSeedsTypes = new Dictionary<string, SeedData>();
    public event Action<SeedData> OnCropSet;
    public event Action<Item> OnItemUpdated;
    public event Action<Item> OnCurrentItemUpdated;

    [SerializeField] private GameInventory GameInventory;
    
    public event Action<Item> OnToolUpdated;
    [SerializeField] private ItemBelt ToolBelt = new ItemBelt();
    public Item GetToolbelt()
    {
        return ToolBelt.GetCurrent();
    }
    public Item GetNextToolbelt()
    {
        return ToolBelt.NextItem();
    }
    public IEnumerable<Item> GetAllTools()
    {
        return ToolBelt.GetAll();
    }
    public string GetTool(TOOL_TYPE type)
    {
        foreach (var item in ToolBelt.GetAll())
        {
            if(((ToolData)item.Data).ToolType == type)
            {
                return item.Data.ID;
            }
        }
        return string.Empty;
    }
    public void SetToolbelt(string id) {
        OnToolUpdated?.Invoke(ToolBelt.SetCurrentItem(id));
    }

    public Item GetItem()
    {
        return GameInventory.Get();
    }
    public bool GetItem(string key, out Item item)
    {
        item = GameInventory[key];
        return item != null;
    }
    public bool GetItem(int key, out Item item)
    {
        item = GameInventory[key];
        return item != null;
    }
    public bool SetItem(string key)
    {
        return GameInventory.Set(key, out Item _);
    }
    public bool ContainsItem(ItemData item, int amount)
    {
        return GameInventory.Contains(item, amount);
    }
    public void SetMarketInventory()
    {
        MarketManager.SetInventory(MarketItems);
    }
    private void DoItemsUpdated(Item item)
    {
        OnItemUpdated?.Invoke(item);
    }
    private void DoCurrentItemUpdated(Item item)
    {
        OnCurrentItemUpdated?.Invoke(item);
    }

    public void AddItem(ItemData item, int amount)
    {
        if (item.SelectableType == SELECTABLE_TYPE.currency)
        {
            ModifyWallet(amount);
            return;
        }
        if (item.SelectableType == SELECTABLE_TYPE.tool)
        {
            HashSet<Item> items = new HashSet<Item>(ToolBelt.GetAll());
            foreach (var tool in items)
            {
                if(((ToolData)tool.Data).ToolType == ((ToolData)item).ToolType)
                {
                    ToolBelt.Swap(tool.Data, new Item(item, amount));
                    break;
                }
            }
        }
        GameInventory.AddItem(item, amount);        
    }
    public bool RemoveItem(ItemData item, int amount)
    {
        if (GameInventory.RemoveItem(item, amount))
        {
            if (item.SelectableType == SELECTABLE_TYPE.tool)
            {
                ToolBelt.Remove(item.ID);
            }
            return true;
        }
        return false;
    }
    public IEnumerable<Item> GetInventory(params SELECTABLE_TYPE[] types)
    {
        return GameInventory.GetAll(types);
    }
    #endregion
    #region Time
    public event Action<TIME_STATE> OnTimeUpdated;
    internal class LightTimeListener : ITimeListener
    {
        public LightManager3D LightManager;
        public void ClockUpdate(int tick)
        {
            if (ITimeManager.Instance.TimeDelta > 0)
            {
                float rotation = 360;
                float deltaPerHour = (rotation / 24) / ITimeManager.Instance.TimeDelta;
                LightManager.SetLightState(deltaPerHour);
            }
        }
        public void Register()
        {
            Instance.TimeManager.RegisterListener(this);
        }
        public void Unregister()
        {
            Instance.TimeManager.UnregisterListener(this);
        }
    }
    [SerializeField] private float FastFowardDelta = .5f;
    [SerializeField] private float TimeDelta = 1f;

    [field: SerializeField] private ITimeManager TimeManager;
    [SerializeField] private LightManager3D LightManager;
    [SerializeField] private LightTimeListener LightListener;
    private void StartTime()
    {
        if (!TimeManager.CanTick)
        {
            TimeManager.SetTick(true);
            StartCoroutine(TimeManager.Tick());
        }        
    }
    private void StopTime()
    {
        TimeManager.SetTick(false);
        StopCoroutine(TimeManager.Tick());        
    }
    public void SpeedUpTime()
    {
        StartTime();
        TimeManager.SetTimeDelta(FastFowardDelta, TIME_STATE.fast);
        OnTimeUpdated?.Invoke(TimeManager.State);
    }
    public void PlayRegularTime()
    {        
        StartTime();
        TimeManager.SetTimeDelta(TimeDelta, TIME_STATE.playing);
        OnTimeUpdated?.Invoke(TimeManager.State);
    }
    public void PauseTime()
    {
        StopTime();
        OnTimeUpdated?.Invoke(TimeManager.State);
    }
    #endregion
    #region Wallet
    public event Action<int> OnMoneyUpdated;
    private Wallet GameWallet;
    public int WalletBalance => GameWallet.Balance;
    public void ModifyWallet(int amount)
    {
        GameWallet.Modify(amount);
        OnMoneyUpdated?.Invoke(amount);
    }
    public bool CheckBalance(int amount)
    {
        return GameWallet.CheckAmount(amount);
    }
    #endregion
    #region Crops
    /// <summary>
    /// Called when a crop has been planted.
    /// </summary>
    /// <param name="cop"></param>
    public void DoPlantCrop(SeedData cop)
    {
        //add experience based on crop?
    }

    /// <summary>
    /// Called when a crop has been harvested.
    /// </summary>
    /// <param name="crop"></param>
    public void DoHarvestCrop(SeedData crop)
    {
        //add experience based on crop?
        GameInventory.AddItem(crop, 1);
    }
    #endregion
}