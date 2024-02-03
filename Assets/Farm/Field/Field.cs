using GameTime;
using Items;
using UnityEngine;
using UnityEngine.Assertions;

namespace Farm.Field
{
    public enum FieldState { untilled, tilled, tilled_crop, watered_tilled, watered_crop };
    public class Field : PropItem
    {
        [Tooltip("In hours, how long does it take for the till state to decay")]
        [SerializeField] private int TillDecayMax = 1;
        [Tooltip("In days, how long does it take for the watered state to decay")]
        [SerializeField] private float WaterDecayDelta = 1;
        private int TillDecay = 0;
        private int WaterDecay = 0;
        private Crop Current;
        [SerializeField] private ParticleSystem Particles;
        [SerializeField] private IFieldRenderer FieldRenderer; //todo: swap with IFieldRenderer for 2D/3D swapping
        public long TimeNeeded => Current.TimeNeeded;
        public float GrowthLevel
        {
            get
            {
                if (HasCrop())
                    return Current.IsAlive ? Current.Percentage : -1;
                return 0;
            }
        }
        public ObjData CropStats => HasCrop() ? Current.CropStats() : null;

        private void Awake()
        {
            ValidateProp();
            TillDecay = TillDecayMax;
        }
        private void OnEnable()
        {
            Register();
        }
        private void OnDisable()
        {
            Unregister();
        }
        private void Start()
        {
            SetState(FieldState.untilled);
        }
        protected override void ValidateProp()
        {
            FieldRenderer = GetComponentInChildren<IFieldRenderer>();
            Assert.IsNotNull(FieldRenderer, "Missing Field Renderer");
        }
        protected void SetState(FieldState state)
        {
            State = state;
            SetFieldRenderer();
        }
        protected void SetFieldRenderer()
        {            
            FieldRenderer.SetFieldState(State);
        }
        protected void SetCropRenderer(ICropRender render)
        {                        
            FieldRenderer.SetCropState(render);
        }        
        #region ITimeListener
        public override void ClockUpdate(int tick)
        {
            if (WaterDecay > 0)
            {
                float originalDecay = LastWaterLevel / Mathf.CeilToInt(WaterDecayDelta / (TimeUtility.DayLength + 1));
                float currentDecay = WaterDecay / Mathf.CeilToInt(WaterDecayDelta / (TimeUtility.DayLength + 1));
                WaterLevel = currentDecay / originalDecay;
                WaterDecay -= Mathf.CeilToInt(WaterDecayDelta / (TimeUtility.DayLength + 1)) * tick;
            }
            else
            {
                SetWater(false);                
            }
            if (!HasCrop() && State == FieldState.tilled && TillDecay > 0)
            {
                TillDecay-= tick;                
                if(TillDecay <= 0 && WaterDecay == 0)
                {
                    SetState(FieldState.untilled);
                }
            }
            else if (HasCrop())
            {
                if(WaterDecay > 0)
                {                    
                    Current.Water();
                }
                Current.Tick();
                SetCropRenderer(Current.UpdateCropSprite());
                if (Current.CanHarvest() && !Particles.isPlaying)
                {
                    Particles.Play();
                }
            }
        }
        #endregion
        #region Field Related
        public FieldState State { get; private set; }
        private int LastWaterLevel;
        public float WaterLevel { get; private set; }

        public bool HasCrop()
        {
            return Current != null;
        }
        public override void Interact(IFarmToolCollection farmTool)
        {
            TOOL_TYPE tool = farmTool.Data.ToolType;
            switch (tool)
            {
                case TOOL_TYPE.Water:
                    if (State != FieldState.untilled)
                    {
                        SetWater(true);
                    }
                    break;
                case TOOL_TYPE.Dig:
                    if (State == FieldState.untilled)
                    {
                        Till();
                    } else if (HasCrop())
                    {
                        EmptyPlot();
                    }
                    break;
                case TOOL_TYPE.Cut:
                    TryHarvest();
                    break;
                case TOOL_TYPE.Hands:
                    if (!TryHarvest())
                        if(!PlantNewCrop(GameInventory.Instance.Get().Data)){
                            //shake or glow or something
                        }
                    break;
            }            
        }
        #endregion
        #region State Changes Helpers
        /// <summary>
        /// Try to harvest crop and reset plot state
        /// </summary>
        /// <returns></returns>
        private bool TryHarvest()
        {
            if (!(HasCrop() && Current.CanHarvest())) return false;

            Current.Harvest();
            EmptyPlot();
            return true;
        }
        private void EmptyPlot()
        {
            if(Particles.isPlaying)
                Particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            Current = null;
            SetCropRenderer(null);
            TillDecay = TillDecayMax;
            if(State == FieldState.watered_crop) SetState(FieldState.watered_tilled);
            if(State == FieldState.tilled_crop) SetState(FieldState.tilled);
        }
        /// <summary>
        /// Called when we interact with a tilled tile and we have crops to plant.
        /// </summary>
        /// <param name="item"></param>
        public bool PlantNewCrop(ItemData item)
        {
            if (!(State == FieldState.tilled || State == FieldState.watered_tilled) || HasCrop() || item is not SeedData seed)
                return false;
            if (GameInventory.Instance.RemoveItem(item, 1))
            {
                Current = new Crop(seed);
                Current.Plant(seed, ITimeManager.Instance.CurrentTime);
                SetCropRenderer(Current.UpdateCropSprite(0));
                if (State == FieldState.watered_tilled) SetState(FieldState.watered_crop);
                if (State == FieldState.tilled) SetState(FieldState.tilled_crop);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when we interact with a grass tile.
        /// </summary>
        protected void Till()
        {
            if (State == FieldState.untilled)
            {
                SetState(FieldState.tilled);
                TillDecay = TillDecayMax;
            }
        }

        /// <summary>
        /// Called when we interact with a crop tile.
        /// </summary>
        protected void SetWater(bool watered, IFarmToolCollection farmTool = null)
        {
            if (watered)
            {
                if (State == FieldState.tilled_crop)
                {
                    SetState(FieldState.watered_crop);
                }
                else if (State == FieldState.tilled)
                {
                    SetState(FieldState.watered_tilled);
                }
                if (farmTool != null)
                {
                    WaterDecay += Mathf.CeilToInt(Mathf.Clamp(farmTool.Data.Attack, 1, farmTool.Data.Attack));
                    WaterLevel = 1;
                    LastWaterLevel = WaterDecay;                    
                }
            } else
            {
                if (State == FieldState.watered_crop)
                {
                    SetState(FieldState.tilled_crop);
                }
                else if (State == FieldState.watered_tilled)
                {
                    SetState(FieldState.tilled);
                }
                WaterLevel = 0;
            }
        }
        #endregion
    }
}