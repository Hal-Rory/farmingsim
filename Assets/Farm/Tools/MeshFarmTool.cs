using Farm.Field;
using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFarmTool : MonoBehaviour, IFarmTool
{
    private MeshRenderer Renderer;
    private MeshFilter Mesh;
    private Vector3 StartingPosition;

    private void Awake()
    {
        StartingPosition= transform.position;
        Renderer = GetComponent<MeshRenderer>();
        Mesh = GetComponent<MeshFilter>();
    }

    public void Register()
    {
        GameManager.Instance.FarmToolManager.RegisterListener(SetToolRender);
        GameManager.Instance.InputManager.RegisterSecondaryInteractionListener(SwapTool);
    }

    public void Unregister()
    {
        GameManager.Instance.FarmToolManager.UnregisterListener(SetToolRender);
        GameManager.Instance.InputManager.UnregisterSecondaryInteractionListener(SwapTool);
    }

    private void Start()
    {
        Register();
    }
    private void OnDestroy()
    {
        Unregister();
    }
    private void Update()
    {
        if(GameManager.Instance.TryGetCurrentHovered(SELECTABLE_TYPE.field, out GameObject selectable))
        {
            transform.position = selectable.transform.position + Vector3.up;            
        }
        else
        {
            transform.position = StartingPosition;
        }
    }

    public void SetToolRender(IFarmToolCollection collection)
    {
        if (collection is MeshFarmToolCollection farmTool)
        {
            //debug due to odd models
            switch (farmTool.Data.ToolType)
            {
                case Items.TOOL_TYPE.Hands:
                    transform.localScale = new Vector3(-50, 50, 50);
                    transform.localEulerAngles = Vector3.right * 180;
                    break;
                case Items.TOOL_TYPE.Cut:
                case Items.TOOL_TYPE.Water:
                    transform.localScale = new Vector3(-20, 20, 20);
                    transform.localEulerAngles = Vector3.right * 270;
                    break;
                default:
                    transform.localScale = new Vector3(-50, 50, 50);
                    transform.localEulerAngles = Vector3.right * 270;
                    break;
            }

            Renderer.sharedMaterials = farmTool.RendererSet.Materials;
            Mesh.sharedMesh = farmTool.RendererSet.Mesh;
        }
    }
    public void SwapTool(bool interact)
    {
        if(interact)
        {
            SwapTool(GameManager.Instance.FarmToolManager.NextTool());
        }
    }
    public void SwapTool(TOOL_TYPE tool)
    {
        GameManager.Instance.FarmToolManager.SwapTool(tool);
    }
}
