using GameTime;
using Items;
using System;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int Money;
    public Dictionary<string, CropData> AllCrops { get; private set; } = new Dictionary<string, CropData>();
    private string SelectedCropToPlant;
    public TOOL_TYPE SelectedTool;
    public event Action<CropData> OnCropSet;
    [field: SerializeField] public IFarmToolStateManager FarmToolManager { get; private set; }
    [field: SerializeField] public IInputManager InputManager { get; private set; }
    [field: SerializeField] public ITimeManager TimeManager { get; private set; }
    [field: SerializeField] public Selector Selection { get; private set; } = new Selector();
    [field: SerializeField] public AudioSource Player{ get; private set; }

    void Awake()
    {
        // Initialize the singleton.
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
            FarmToolManager = GetComponentInChildren<IFarmToolStateManager>();
            InputManager = GetComponentInChildren<IInputManager>();

            TimeManager.SetTick(true);
            Selection.InputManager = InputManager;
            InputManager.RegisterPrimaryInteractionListener(Selection.Interaction);
        }
        GetOrSetFirstCrop();
    }
    private void OnDestroy()
    {
        if(Instance == this) InputManager.UnregisterPrimaryInteractionListener(Selection.Interaction);
    }

    public bool TryGetCurrentHovered(SELECTABLE_TYPE type, out GameObject selectable)
    {
        selectable = null;
        if(Selection.HoverValidated && Selection.Hovered.Type == type && !InputManager.IsPointerOverUI())
        {
            selectable = GameManager.Instance.Selection.Hovered.SelectableObject;
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
        StartCoroutine(TimeManager.Tick());
    }
    void OnDisable()
    {
        Crop.OnPlantCrop -= DoPlantCrop;
        Crop.OnHarvestCrop -= DoHarvestCrop;
        StopCoroutine(TimeManager.Tick());
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
        Money += crop.SellPrice;
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