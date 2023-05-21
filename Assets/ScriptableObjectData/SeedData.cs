using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Crop Data", menuName = "New Crop Data")]    
    public class SeedData : ItemData
    {
        public MeshCropRender[] GrowthProgressRenders; //todo: switch to ICropRender for 3D/2D swapping
        public MeshCropRender ReadyToHarvestRender;
        public MeshCropRender DeadCropRender;        
        public float WaterDelta = .1f;
        public int HoursNeeded;
        public int DaysNeeded;
        public int MonthsNeeded;
        public int YearsNeeded;

    }
}