using System;
using UnityEngine;
[Serializable]
public class MeshCropRender: ICropRender
{
    [SerializeField] private MeshMaterialSet Set;

    public void SetCropRender(ICropRenderer renderer)
    {
        if(renderer is MeshCropRenderer meshCrop)
        {
            meshCrop.SetMesh(Set.Mesh);
            meshCrop.SetMaterials(Set.Materials);
        }
    }
}
