namespace Farm.Field
{
    public interface IFieldRenderer
    {
        public ICropRenderer CropRenderer { get; }
        public void SetFieldState(IField.FieldState state);
        public void SetCropState(ICropRender crop);
    }
}