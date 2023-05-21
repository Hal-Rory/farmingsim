namespace Farm.Field
{
    public interface IFieldRenderer
    {
        public ICropRenderer CropRenderer { get; }
        public void SetFieldState(FieldState state);
        public void SetCropState(ICropRender crop);
    }
}