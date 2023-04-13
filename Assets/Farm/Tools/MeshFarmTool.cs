using Farm.Field;
using Items;
using UnityEngine;

public class MeshFarmTool : MonoBehaviour, IFarmTool
{
    [SerializeField] private MeshRenderer Renderer;
    [SerializeField] private MeshFilter Mesh;
    private Vector3 StartingLocalPosition;
    [SerializeField] private bool UpdateInEditor;
    [SerializeField] private TOOL_TYPE Type;
    [SerializeField] private Animator Animation;
    private bool CanAnimate;
    private IFarmToolStateManager FarmManager => GameManager.Instance.FarmToolManager;

    private void OnValidate()
    {
        if (UpdateInEditor)
        {
            UpdateInEditor= false;
            FarmToolStateManager3D farmToolManager = FindObjectOfType<FarmToolStateManager3D>();
            if(farmToolManager != null)
            {
                if(farmToolManager.TryGetTool(Type, out IFarmToolCollection tool))
                {
                    SetRendererAndMesh(tool as MeshFarmToolCollection);
                }
            }
        }
    }

    private void Awake()
    {
        StartingLocalPosition= transform.localPosition;        
    }

    public void Register()
    {
        FarmManager.RegisterListener(SetToolRender);
        GameManager.Instance.InputManager.RegisterSecondaryInteractionListener(DoSwapTool);
        Field.OnFieldSelected += DoFieldSelected;
        Field.OnHoveredChanged += DoHoveredChanged;
    }

    public void Unregister()
    {
        FarmManager.UnregisterListener(SetToolRender);
        GameManager.Instance.InputManager.UnregisterSecondaryInteractionListener(DoSwapTool);
        Field.OnFieldSelected -= DoFieldSelected;
        Field.OnHoveredChanged -= DoHoveredChanged;
    }

    private void PlayAnimation(bool interact)
    {
        if (interact)
        {
            Animate();
        }
    }
    private void Animate(bool play = true)
    {
        if(!(play && CanAnimate))
        {
            Animation.SetBool("idle", true);
            return;
        }
        Animation.SetBool("idle", false);
        switch (FarmManager.GetCurrentToolType())
        {
            case TOOL_TYPE.Dig:
                Animation.Play("Dig");
                break;
            case TOOL_TYPE.Water:
                Animation.Play("Water");
                break;
            case TOOL_TYPE.Cut:
                Animation.Play("Cut");
                break;
            case TOOL_TYPE.Hands:
                Animation.Play("Hand");
                break;            
        }
    }

    public void InteractWithHovered()
    {
        if (Field.Hovered != null)
        {
            Field.Hovered.Interact();
        }
    }
    private void DoFieldSelected(Field obj)
    {
        PlayAnimation(true);
    }
    private void DoHoveredChanged(Field obj)
    {
        if(obj != Field.Hovered)
        {
            Animate(false);
        }
    }
    private void Start()
    {
        IFarmToolCollection farmTool = FarmManager.GetCurrentTool();
        if (farmTool != null)
        {
            SetToolRender(farmTool);
        }
        Register();
    }
    private void OnDestroy()
    {
        Unregister();
    }
    private void Update()
    {
        if(Field.Hovered != null)
        {
            transform.position = Field.Hovered.transform.position + Vector3.up;
            CanAnimate = true;
        } else if (CanAnimate)
        {
            Animate(false);
            CanAnimate = false;
            transform.localPosition = StartingLocalPosition;
        }        
    }
    private void SetRendererAndMesh(MeshFarmToolCollection farmTool)
    {
        //debug due to odd models
        switch (farmTool.Data.ToolType)
        {
            case TOOL_TYPE.Hands:
            case TOOL_TYPE.Dig:            
                Renderer.transform.localScale = new Vector3(-50, 50, 50);
                break;
            case TOOL_TYPE.Cut:
            case TOOL_TYPE.Water:
                Renderer.transform.localScale = new Vector3(-20, 20, 20);
                break;
        }

        Renderer.sharedMaterials = farmTool.RendererSet.Materials;
        Mesh.sharedMesh = farmTool.RendererSet.Mesh;
    }

    public void SetToolRender(IFarmToolCollection collection)
    {
        Animate(false);
        if (collection is MeshFarmToolCollection farmTool)
        {            
            SetRendererAndMesh(farmTool);
        }
    }
    public void DoSwapTool(bool interact)
    {
        if(interact)
        {            
            SwapTool(FarmManager.NextTool());
        }
    }
    public void SwapTool(TOOL_TYPE tool)
    {
        FarmManager.SwapTool(tool);
    }
}
