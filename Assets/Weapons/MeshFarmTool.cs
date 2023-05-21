using Items;
using UnityEngine;

public class MeshFarmTool : MonoBehaviour, IFarmTool
{
    [SerializeField] private MeshRenderer Renderer;
    [SerializeField] private MeshFilter Mesh;
    private Vector3 StartingLocalPosition;
    [SerializeField] private bool UpdateInEditor;
    [SerializeField] private IFarmToolCollection Tool;
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
                //if(farmToolManager.TryGetTool(Type, out IFarmToolCollection tool))
                //{
                //    SetRendererAndMesh(tool as MeshFarmToolCollection);
                //}
            }
        }
    }
    private void Awake()
    {
        StartingLocalPosition = transform.localPosition;
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
    
    #region Event Listeners
    private void DoHoveredChanged(ISelectable prev, ISelectable next)
    {
        if (next == null || prev != next)
        {
            Animate(false);
        }
        if (next != null)
        {
            transform.position = next.SelectableObject.transform.TransformPoint(next.HoverPoint); //todo: make this an axis, not a point?
            CanAnimate = true;
        }
        else if (CanAnimate)
        {
            Animate(false);
            CanAnimate = false;
            transform.localPosition = StartingLocalPosition;
        }
    }

    public void DoSwapTool(bool interact)
    {
        if (interact)
        {
            SwapTool(FarmManager.NextTool());
        }
    }

    private void DoPrimaryInteraction(bool interact)
    {
        if (interact)
        {
            PlayAnimation(true);
        }
    }
    
    private void DoSetToolRender(IFarmToolCollection collection)
    {
        SetToolRender(collection);
    }
    #endregion

    #region Animations and Events
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
        switch (Tool.Data.ToolType)
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
    /// <summary>
    /// Used by animation to interact at appropriate time
    /// </summary>
    public void InteractWithHovered()
    {
        if (GameManager.Instance.Selection.HoverValidated && GameManager.Instance.Selection.Hovered is PropItem prop)
        {
            prop.Interact(Tool.Data.ToolType);
        }
    }
    #endregion

    #region IFarmTool
    public void Register()
    {
        FarmManager.RegisterListener(DoSetToolRender);
        GameManager.Instance.InputManager.RegisterPrimaryInteractionListener(DoPrimaryInteraction);
        GameManager.Instance.InputManager.RegisterSecondaryInteractionListener(DoSwapTool);
        GameManager.Instance.Selection.OnHoveredChanged += DoHoveredChanged;
    }

    public void Unregister()
    {
        FarmManager.UnregisterListener(DoSetToolRender);
        GameManager.Instance.InputManager.UnregisterPrimaryInteractionListener(DoPrimaryInteraction);
        GameManager.Instance.InputManager.UnregisterSecondaryInteractionListener(DoSwapTool);
        GameManager.Instance.Selection.OnHoveredChanged -= DoHoveredChanged;
    }
    public void SetToolRender(IFarmToolCollection collection)
    {
        Animate(false);
        if (collection is MeshFarmToolCollection farmTool)
        {
            Tool = farmTool;
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
    }
    public void SwapTool(TOOL_TYPE tool)
    {
        FarmManager.TrySwapTool(tool);
    }
    #endregion
}
