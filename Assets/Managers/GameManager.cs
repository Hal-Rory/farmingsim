using GameTime;
using Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using static ITimeManager;
using static UIManager;
using static UnityEditor.Progress;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{   
    public static GameManager Instance;
    [field: SerializeField] public IFarmToolStateManager FarmToolManager { get; private set; }
    [field: SerializeField] public IInputManager InputManager { get; private set; }
    [field: SerializeField] public MarketManager MarketManager { get; private set; }
    [field: SerializeField] public Selector Selection { get; private set; } = new Selector();
    [field: SerializeField] public PlayerInventoryManager PlayerInventoryManager { get; private set; }
    [field: SerializeField] public UIManager UIManager { get; private set; }

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
                AllCrops.TryAdd(crop.ID, crop);
            }
            PlayerInventoryManager = new PlayerInventoryManager();
            GameInventory.OnUpdated += DoItemsUpdated;
            WeaponInventory.OnUpdated += DoWeaponsUpdated;
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
            TimeManager.RegisterListener(LightListener);
        }        

        GetOrSetFirstCrop();
    }
   
    private void OnDestroy()
    {
        if(Instance == this) InputManager.UnregisterPrimaryInteractionListener(Selection.Interaction);
        if(Instance == this) TimeManager.UnregisterListener(LightListener);
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
    public Dictionary<string, SeedData> AllCrops { get; private set; } = new Dictionary<string, SeedData>();
    private string SelectedCropToPlant;
    public event Action<SeedData> OnCropSet;
    public event Action<Item> OnItemsUpdated;
    public event Action<Item> OnWeaponsUpdated;

    [SerializeField] private Inventory GameInventory = new Inventory();
    [SerializeField] private Inventory WeaponInventory = new Inventory();
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
    public void SetMarketInventory()
    {
        MarketManager.SetInventory(MarketItems);
    }
    private void DoItemsUpdated(Item item)
    {
        OnItemsUpdated?.Invoke(item);
    }
    public void AddItem(ItemData item, int amount)
    {
        if (item is WeaponData)
        {
            AddWeapon(item as WeaponData, amount);
        }
        else
        {
            GameInventory.AddItem(item, amount);            
        }
    }
    public bool RemoveItem(ItemData item, int amount)
    {
        if (item is WeaponData)
        {
            return RemoveWeapon(item as WeaponData, amount);
        }
        else
        {
            return GameInventory.RemoveItem(item, amount);
        }
    }
    public Item GetWeapon()
    {
        return WeaponInventory.Get();
    }
    private void DoWeaponsUpdated(Item item)
    {
        OnWeaponsUpdated?.Invoke(item);
    }
    public void AddWeapon(WeaponData item, int amount)
    {
        WeaponInventory.AddItem(item, amount);
    }
    public bool RemoveWeapon(WeaponData item, int amount)
    {
        return WeaponInventory.RemoveItem(item, amount);
    }
    public bool GetWeapon(string key, out Item item)
    {
        item = WeaponInventory[key];
        return item != null;
    }
    public bool GetWeapon(int key, out Item item)
    {
        item = WeaponInventory[key];
        return item != null;
    }
    public bool SetWeapon(string key)
    {
        return WeaponInventory.Set(key, out Item _);
    }
    public void SelectFromInventory(string ID)
    {
        if (GameInventory.Set(ID, out Item item) && item.Data is SeedData)
        {
            SelectedCropToPlant = item.Data.ID;
        }
    }
    public IEnumerable<Item> GetInventory()
    {
        return GameInventory.GetAll();
    }
    public IEnumerable<Item> GetWeapons()
    {
        return WeaponInventory.GetAll();
    }
    #endregion
    #region Time
    public event Action<TIME_STATE> OnTimeUpdated;
    private class LightTimeListener : ITimeListener
    {
        public LightManager3D LightManager;
        public void ClockUpdate(TimeStruct timestamp)
        {
            float rotation = 360;
            float deltaPerHour = rotation / 24;
            LightManager.SetLightState(deltaPerHour);
        }
    }
    [SerializeField] private float FastFowardDelta = .5f;
    [SerializeField] private float TimeDelta = 1f;

    [field: SerializeField] public ITimeManager TimeManager { get; private set; }
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
    public SeedData GetOrSetFirstCrop(int modify = 0)
    {
        List<string> crops = new List<string>(AllCrops.Keys);
        if (!string.IsNullOrEmpty(SelectedCropToPlant))
        {
            int current = crops.IndexOf(SelectedCropToPlant);
            int next = (int)Mathf.Repeat(current + modify, crops.Count);
            SelectedCropToPlant = crops[next];
            if (modify != 0) OnCropSet?.Invoke(AllCrops[SelectedCropToPlant]);
        }
        else
        {
            SelectedCropToPlant = crops[0];
            OnCropSet?.Invoke(AllCrops[SelectedCropToPlant]);
        }
        return AllCrops[SelectedCropToPlant];
    }
    public SeedData PreviousCrop()
    {
        return GetOrSetFirstCrop(-1);
    }
    public SeedData NextCrop()
    {
        return GetOrSetFirstCrop(1);
    }
    
    /// <summary>
    /// Called when a crop has been planted.
    /// </summary>
    /// <param name="cop"></param>
    public void DoPlantCrop(SeedData cop)
    {
        //monitor crop supply
    }

    /// <summary>
    /// Called when a crop has been harvested.
    /// </summary>
    /// <param name="crop"></param>
    public void DoHarvestCrop(SeedData crop)
    {
        ModifyWallet(crop.SellPrice);
        GameInventory.AddItem(crop, 1);
    }

    /// <summary>
    ///  Called when we want to purchase a crop.
    /// </summary>
    /// <param name="crop"></param>
    public void PurchaseCrop(SeedData crop)
    {
    }

    /// <summary>
    /// Do we have enough crops to plant?
    /// </summary>
    /// <returns></returns>
    public bool CanPlantCrop()
    {
        return true;
    }

    /// <summary>
    /// Called when the buy crop button is pressed.
    /// </summary>
    /// <param name="crop"></param>
    public void DoBuyCropButton(SeedData crop)
    {
    }
    #endregion
}