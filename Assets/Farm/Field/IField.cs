namespace Farm.Field
{
    public interface IField
    {
        public enum FieldState { untilled, tilled, tilled_crop, watered_tilled, watered_crop };
        public FieldState State { get; }
        /// <summary>
        /// Contextually nteract with field
        /// </summary>
        public void Interact();
        /// <summary>
        /// Does field contain a crop currently
        /// </summary>
        /// <returns></returns>
        public bool HasCrop();
        public float GrowthLevel { get; }
        public float WaterLevel { get; }
    }
}