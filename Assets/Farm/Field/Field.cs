using GameTime;
using Items;
using UnityEngine;
using UnityEngine.Assertions;

namespace Farm.Field
{
    public class Field : MonoBehaviour, IField, ITimeListener, ISelectable
    {
        [Tooltip("In hours, how long does it take for the till state to decay")]
        [SerializeField] private int TillDecayMax = 1;
        [Tooltip("In days, how long does it take for the watered state to decay")]
        [SerializeField] private float WaterDecayDelta = 1;
        private int TillDecay = 0;
        private int WaterDecay = 0;
        private Crop Current;        
        [SerializeField] private MeshFieldBase FieldRenderer; //todo: swap with IFieldRenderer for 2D/3D swapping
        public long TimeNeeded => Current.TimeNeeded;
        public float GrowthLevel => HasCrop() ? Current.Percentage : 0;
        public ObjData CropStats => HasCrop() ? Current.CropStats() : null;

        private void Awake()
        {
            Assert.IsNotNull(FieldRenderer, "Missing Field Renderer");
            TillDecay = TillDecayMax;
        }
        private void OnEnable()
        {
            GameManager.Instance.TimeManager.RegisterListener(this);
        }
        private void Start()
        {
            SetState(IField.FieldState.untilled);
        }
        private void OnDisable()
        {
            GameManager.Instance.TimeManager.UnregisterListener(this);
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
        public GameObject SelectableObject { get => gameObject; }
        public bool Selected { get; private set; }
        public bool Hovered { get; private set; }
        private bool _selectable = true;
        public bool Selectable { get => _selectable; set => _selectable = value; }
        public bool CanReselect => true;
        public SELECTABLE_TYPE Type => SELECTABLE_TYPE.field;

        public void OnDeselect()
        {
            print("deselected");
        }

        public void OnEndHover()
        {
            Hovered = false;
        }

        public void OnSelect()
        {
            Interact();
        }

        public void OnStartHover()
        {
            Hovered =true;
        }

        public void WhileHovering()
        {
            if (!Selected)
            {
            }
        }

        public void WhileSelected()
        {
        }
        #endregion
        #region ITimeListener
        public void ClockUpdate(TimeStruct timestamp)
        {
            if (WaterDecay > 0)
            {
                float originalDecay = LastWaterLevel / Mathf.CeilToInt(WaterDecayDelta / (TimeStruct.DayLength + 1));
                float currentDecay = WaterDecay / Mathf.CeilToInt(WaterDecayDelta / (TimeStruct.DayLength + 1));
                WaterLevel = currentDecay / originalDecay;
                WaterDecay -= Mathf.CeilToInt(WaterDecayDelta / (TimeStruct.DayLength + 1));
            }
            else
            {
                SetWater(false);                
            }
            if (!HasCrop() && State == IField.FieldState.tilled && TillDecay > 0)
            {
                TillDecay--;                
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
                if (!Current.IsAlive)
                {
                    Current = null;
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
        public void Interact()
        {
            switch (GameManager.Instance.SelectedTool)
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
                    }
                    break;
                case TOOL_TYPE.Cut:                    
                    EmptyPlot();
                    break;
                case TOOL_TYPE.Hands:
                    if (HasCrop() && Current.CanHarvest())
                    {
                        Current.Harvest();
                        Current = null;
                        EmptyPlot();
                    }
                    break;
            }            
        }
        #endregion

        private void EmptyPlot()
        {
            Current= null;
            SetCropRenderer(null);
            TillDecay = TillDecayMax;
            SetState(IField.FieldState.tilled);
        }
        /// <summary>
        /// Called when we interact with a tilled tile and we have crops to plant.
        /// </summary>
        /// <param name="crop"></param>
        public void PlantNewCrop(CropData crop)
        {
            if (State != IField.FieldState.tilled || HasCrop())
                return;            
            Current = new Crop(crop);
            Current.Plant(crop, GameManager.Instance.TimeManager.CurrentTime);
            SetCropRenderer(Current.UpdateCropSprite(0));
            SetState(IField.FieldState.tilled_crop);
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
                
                if(GameManager.Instance.FarmToolManager.TryGetTool(GameManager.Instance.SelectedTool, out IFarmToolCollection collection))
                {
                    WaterDecay += Mathf.CeilToInt(Mathf.Clamp(collection.Data.Attack, 1, collection.Data.Attack));
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