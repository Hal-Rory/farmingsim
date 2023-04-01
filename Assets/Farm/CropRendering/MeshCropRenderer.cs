using UnityEngine;

public class MeshCropRenderer : MonoBehaviour, ICropRenderer
{
    private MeshFilter Mesh;
    private MeshRenderer Renderer;
    private void Start()
    {
        Mesh = GetComponent<MeshFilter>();
        Renderer = GetComponent<MeshRenderer>();
    }
    public void SetCropState(ICropRender crop)
    {
        if (crop != null)
        {
            ((MeshCropRender)crop).SetCropRender(this);
        } else {
            SetMesh(null);
        }
    }
    public void SetMesh(Mesh mesh)
    {
        Mesh.sharedMesh = mesh;
    }
    public void SetMaterials(Material[] mats)
    {
        Renderer.sharedMaterials = mats;
    }
}
