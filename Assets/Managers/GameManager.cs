using GameTime;
using Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using static ITimeManager;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
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
    public static GameManager Instance;
    public int Money;
    public Dictionary<string, CropData> AllCrops { get; private set; } = new Dictionary<string, CropData>();
    private string SelectedCropToPlant;
    public TOOL_TYPE SelectedTool;
    public event Action<CropData> OnCropSet;
    public event Action<int> OnMoneyUpdated;
    public event Action<TIME_STATE> OnTimeUpdated;    
    [field: SerializeField] public IFarmToolStateManager FarmToolManager { get; private set; }
    [field: SerializeField] public IInputManager InputManager { get; private set; }
    [field: SerializeField] public ITimeManager TimeManager { get; private set; }
    [field: SerializeField] public Selector Selection { get; private set; } = new Selector();
    [SerializeField] private LightManager3D LightManager;
    [SerializeField] private LightTimeListener LightListener;
    [SerializeField] private InventoryUIManager InventoryUI;
    [SerializeField] private Inventory GameInventory;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            List<CropData> crops = new List<CropData>(Resources.LoadAll<CropData>("Crops"));
            foreach (CropData crop in crops)
            {
                AllCrops.TryAdd(crop.ID, crop);
            }
            TimeManager = new TimeManager(1, TimeStruct.Default);
            LightListener = new LightTimeListener();
            FarmToolManager = GetComponentInChildren<IFarmToolStateManager>();
            InputManager = GetComponentInChildren<IInputManager>();
            
            Selection.InputManager = InputManager;
            InputManager.RegisterPrimaryInteractionListener(Selection.Interaction);
            InputManager.SetMouseFocus(true);

            InventoryUI.SetInventory(GameInventory);
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

    public void AddItem(ObjData item, int amount)
    {
        GameInventory.AddItem(item, amount);
    }
    public bool RemoveItem(ObjData item, int amount)
    {
        return GameInventory.RemoveItem(item, amount);
    }

    public bool TryGetCurrentHovered(SELECTABLE_TYPE type, out GameObject selectable)
    {
        selectable = null;
        if(Selection.HoverValidated && Selection.Hovered.Type == type && !InputManager.IsPointerOverUI())
        {
            selectable = Selection.Hovered.SelectableObject;
            return true;
        }
        return false;
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
    public CropData GetOrSetFirstCrop(int modify = 0)
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
    public CropData PreviousCrop()
    {
        return GetOrSetFirstCrop(-1);
    }
    public CropData NextCrop()
    {
        return GetOrSetFirstCrop(1);
    }

    /// <summary>
    /// Called when a crop has been planted.
    /// </summary>
    /// <param name="cop"></param>
    public void DoPlantCrop(CropData cop)
    {
        //monitor crop supply
    }

    /// <summary>
    /// Called when a crop has been harvested.
    /// </summary>
    /// <param name="crop"></param>
    public void DoHarvestCrop(CropData crop)
    {
        //OnMoneyUpdated?.Invoke(crop.SellPrice);
        Money += crop.SellPrice;
        GameInventory.AddItem(crop, 1);
    }

    /// <summary>
    ///  Called when we want to purchase a crop.
    /// </summary>
    /// <param name="crop"></param>
    public void PurchaseCrop(CropData crop)
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
    public void DoBuyCropButton(CropData crop)
    {
    }
}