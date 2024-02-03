using GameTime;
using Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Crop
{
    private SeedData Current;
    private float WaterLevel;
    public static event UnityAction<SeedData> OnPlantCrop;
    public static event UnityAction<SeedData> OnHarvestCrop;
    private List<ICropRender> GrowthProgressRenders;
    private ICropRender ReadyToHarvestRender;
    private ICropRender DeadCropRender;
    public long TimeNeeded {get; private set;}
    private long CurrentTime;
    public bool IsAlive { get; private set; }
    private static float WaterMax => 100;

    public float Percentage { get; private set; }

    public Crop(SeedData current)
    {
        Current = current;
        TimeNeeded = Current.HoursNeeded + TimeUtility.DaysToHours(Current.DaysNeeded) + TimeUtility.MonthsToHours(Current.MonthsNeeded) + TimeUtility.YearsToHours(Current.YearsNeeded);
        GrowthProgressRenders = new List<ICropRender>();
        for (int i = 0; i < Current.GrowthProgressRenders.Length; i++)
        {
            ICropRender render = CropObjectManager.GetCrop(Current.GrowthProgressRenders[i]).GetComponent<ICropRender>();
            if (render is not ICropRender)
                return;
            GrowthProgressRenders.Add(render);
        }
    }

    public ObjData CropStats()
    {
        return Current;
    }

    /// <summary>
    /// Called when the crop has been planted for the first time.
    /// </summary>
    /// <param name="crop"></param>
    public void Plant(SeedData crop, TimeStruct time)
    {
        Current = crop;
        IsAlive= true;
        WaterLevel = WaterMax;
        CurrentTime = TimeNeeded;
        OnPlantCrop?.Invoke(crop);
    }

    /// <summary>
    /// Called when time ticks over.
    /// </summary>
    public void Tick()
    {
        if (!CanHarvest() && IsAlive)
        {
            CurrentTime = Math.Clamp(CurrentTime - 1, 0, TimeNeeded);
            WaterLevel = Math.Clamp(WaterLevel - Current.WaterDelta, 0, WaterMax);
            if (WaterLevel == 0 && CurrentTime > 0) 
                IsAlive = false;
        }
    }

    public ICropRender UpdateCropSprite(int index)
    {
        if(Current.GrowthProgressRenders == null)
        {
            Debug.LogWarning("No sprites, returning null");
            return null;
        }
        if (Current.GrowthProgressRenders.Length == 0)
        {
            Debug.LogWarning("No sprites, returning null");
            return null;
        }
        return GrowthProgressRenders[Math.Clamp(index, 0, Current.GrowthProgressRenders.Length - 1)];
    }
    /// <summary>
    /// Called when the crop has progressed.
    /// </summary>
    public ICropRender UpdateCropSprite()
    {
        if(!IsAlive) return DeadCropRender;        
        if (!CanHarvest())
        {
            if (Current.GrowthProgressRenders.Length == 0)
            {
                Debug.LogWarning("No sprites, returning null");
                return null;
            }
            int index = Mathf.FloorToInt(Current.GrowthProgressRenders.Length * Percentage);
            return UpdateCropSprite(index);
        } else 
        {
            return ReadyToHarvestRender;
        }        
    }

    /// <summary>
    /// Can we currently harvest the crop?
    /// </summary>
    /// <returns></returns>
    public bool CanHarvest()
    {
        if (!IsAlive) return false;
        Percentage =  1f - ((CurrentTime * 1f) / (TimeNeeded*1f));
        return CurrentTime == 0;
    }

    /// <summary>
    /// Called when the crop has been watered.
    /// </summary>
    public void Water()
    {
        if (!IsAlive) return;
        WaterLevel = WaterMax;
    }

    /// <summary>
    /// Called when we want to harvest the crop.
    /// </summary>
    public void Harvest()
    {
        if (CanHarvest())
        {
            OnHarvestCrop?.Invoke(Current);            
        }
    }
}
