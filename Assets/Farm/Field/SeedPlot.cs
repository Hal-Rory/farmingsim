using GameTime;
using Items;
using UnityEngine;
using UnityEngine.Assertions;

namespace Farm.Field
{
    public class SeedPlot : PropItem, IField, ITimeListener, ISelectable
    {
        [Tooltip("In hours, how long does it take for the till state to decay")]
        [SerializeField] private int TillDecayMax = 1;
        [Tooltip("In days, how long does it take for the watered state to decay")]
        [SerializeField] private float WaterDecayDelta = 1;
        private int TillDecay = 0;
        private int WaterDecay = 0;
        private Crop Current;
        [SerializeField] private ParticleSystem Particles;
        [SerializeField] private MeshFieldBase FieldRenderer; //todo: swap with IFieldRenderer for 2D/3D swapping
        public long TimeNeeded => Current.TimeNeeded;
        private IFarmToolStateManager FarmManager => GameManager.Instance.FarmToolManager;
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
            SetState(IField.FieldState.untilled);
        }
        protected override void ValidateProp()
        {
            Assert.IsNotNull(FieldRenderer, "Missing Field Renderer");
        }
        protected void SetState(IField.FieldState state)
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
        #region ISelectable
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
            if (!HasCrop() && State == IField.FieldState.tilled && TillDecay > 0)
            {
                TillDecay-= tick;                
                if(TillDecay <= 0 && WaterDecay == 0)
                {
                    SetState(IField.FieldState.untilled);
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
        #region IField       
        public IField.FieldState State { get; private set; }
        private int LastWaterLevel;
        public float WaterLevel { get; private set; }

        public bool HasCrop()
        {
            return Current != null;
        }
        public override void Interact()
        {
            switch (FarmManager.GetCurrentToolType())
            {
                case TOOL_TYPE.Water:
                    if (State != IField.FieldState.untilled)
                    {
                        SetWater(true);
                    }
                    break;
                case TOOL_TYPE.Dig:
                    if (State == IField.FieldState.untilled)
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
                        if(!PlantNewCrop(GameManager.Instance.GetItem().Data)){
                            //shake or glow or something
                        }
                    break;
            }            
        }
        private bool TryHarvest()
        {
            if (!(HasCrop() && Current.CanHarvest())) return false;

            Current.Harvest();
            EmptyPlot();
            return true;
        }
        #endregion

        private void EmptyPlot()
        {
            if(Particles.isPlaying)
                Particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            Current = null;
            SetCropRenderer(null);
            TillDecay = TillDecayMax;
            if(State == IField.FieldState.watered_crop) SetState(IField.FieldState.watered_tilled);
            if(State == IField.FieldState.tilled_crop) SetState(IField.FieldState.tilled);
        }
        /// <summary>
        /// Called when we interact with a tilled tile and we have crops to plant.
        /// </summary>
        /// <param name="item"></param>
        public bool PlantNewCrop(ItemData item)
        {
            if (!(State == IField.FieldState.tilled || State == IField.FieldState.watered_tilled) || HasCrop() || item is not SeedData seed)
                return false;
            if (GameManager.Instance.RemoveItem(item, 1))
            {
                Current = new Crop(seed);
                Current.Plant(seed, ITimeManager.Instance.CurrentTime);
                SetCropRenderer(Current.UpdateCropSprite(0));
                if (State == IField.FieldState.watered_tilled) SetState(IField.FieldState.watered_crop);
                if (State == IField.FieldState.tilled) SetState(IField.FieldState.tilled_crop);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when we interact with a grass tile.
        /// </summary>
        protected void Till()
        {
            if (State == IField.FieldState.untilled)
            {
                SetState(IField.FieldState.tilled);
                TillDecay = TillDecayMax;
            }
        }

        /// <summary>
        /// Called when we interact with a crop tile.
        /// </summary>
        protected void SetWater(bool watered)
        {
            if (watered)
            {
                if (State == IField.FieldState.tilled_crop)
                {
                    SetState(IField.FieldState.watered_crop);
                }
                else if (State == IField.FieldState.tilled)
                {
                    SetState(IField.FieldState.watered_tilled);
                }
                IFarmToolCollection farmTool = FarmManager.GetCurrentTool();
                if (farmTool != null)
                {
                    WaterDecay += Mathf.CeilToInt(Mathf.Clamp(farmTool.Data.Attack, 1, farmTool.Data.Attack));
                    WaterLevel = 1;
                    LastWaterLevel = WaterDecay;                    
                }
            } else
            {
                if (State == IField.FieldState.watered_crop)
                {
                    SetState(IField.FieldState.tilled_crop);
                }
                else if (State == IField.FieldState.watered_tilled)
                {
                    SetState(IField.FieldState.tilled);
                }
                WaterLevel = 0;
            }
        }
    }
}