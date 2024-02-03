using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Crop Data", menuName = "New Crop Data")]    
    public class SeedData : ItemData
    {
        public string[] GrowthProgressRenders; //todo: switch to ICropRender for 3D/2D swapping
        public string ReadyToHarvestRender;
        public string DeadCropRender;        
        public float WaterDelta = .1f;
        public int HoursNeeded;
        public int DaysNeeded;
        public int MonthsNeeded;
        public int YearsNeeded;

    }
}