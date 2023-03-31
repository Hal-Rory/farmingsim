using Items;
using System;
using UnityEngine;

[Serializable]
public class MeshFarmToolCollection : IFarmToolCollection
{
    [field: SerializeField] public ToolData Data { get; private set; }
    public MeshMaterialSet RendererSet;
}