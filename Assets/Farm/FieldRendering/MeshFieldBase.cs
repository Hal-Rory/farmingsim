using Farm.Field;
using UnityEngine;
using UnityEngine.Assertions;

public class MeshFieldBase : MonoBehaviour, IFieldRenderer
{
    [SerializeField] private Material UntilledMat;
    [SerializeField] private Material TilledMat;
    [SerializeField] private Material TilledWateredMat;
    [SerializeField] private MeshCropRenderer _cropRenderer;
    private MeshRenderer Renderer;

    private void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
        Assert.IsNotNull(_cropRenderer, "Missing Crop Renderer");
    }

    public ICropRenderer CropRenderer => _cropRenderer;

    public void SetCropState(ICropRender crop)
    {
        CropRenderer.SetCropState(crop);
    }

    public void SetFieldState(FieldState state)
    {
        Material[] mats = new Material[Renderer.sharedMaterials.Length];
        Renderer.sharedMaterials.CopyTo(mats, 0);
        switch (state)
        {
            case FieldState.untilled:
                mats[1] = UntilledMat;
                break;
            case FieldState.tilled_crop:
            case FieldState.tilled:
                mats[1] = TilledMat;
                break;
            case FieldState.watered_crop:
            case FieldState.watered_tilled:
                mats[1] = TilledWateredMat;
                break;
        }
        Renderer.sharedMaterials = mats;
    }
}
