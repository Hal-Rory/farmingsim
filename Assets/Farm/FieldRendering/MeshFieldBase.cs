using Farm.Field;
using System;
using System.Collections;
using System.Collections.Generic;
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

    public void SetFieldState(IField.FieldState state)
    {
        Material[] mats = new Material[Renderer.sharedMaterials.Length];
        Renderer.sharedMaterials.CopyTo(mats, 0);
        switch (state)
        {
            case IField.FieldState.untilled:
                mats[1] = UntilledMat;
                break;
            case IField.FieldState.tilled_crop:
            case IField.FieldState.tilled:
                mats[1] = TilledMat;
                break;
            case IField.FieldState.watered_crop:
            case IField.FieldState.watered_tilled:
                mats[1] = TilledWateredMat;
                break;
        }
        Renderer.sharedMaterials = mats;
    }
}
