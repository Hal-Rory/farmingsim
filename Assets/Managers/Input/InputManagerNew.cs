using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManagerNew : MonoBehaviour, IInputManager
{
    [SerializeField] private Camera Cam;    
    public Camera ActiveCamera => Cam;
    [SerializeField] private InputActionAsset Asset;

    private InputAction MoveActionVector;
    [SerializeField] private string MoveActionName = "Player/Move";
    private InputAction LookActionVector;
    [SerializeField] private string LookActionName = "Player/Look";
    private InputAction PrimaryAction;
    [SerializeField] private string PrimaryActionName = "Player/InteractP";
    private InputAction SecondaryAction;
    [SerializeField] private string SecondaryActionName= "Player/InteractS";
    private InputAction MenuAction;
    [SerializeField] private string MenuActionName = "Player/InteractMenu";
    private InputAction PointerActionVector;
    [SerializeField] private string PointerActionName = "UI/Point";
    private InputAction EquipmentAction;
    [SerializeField] private string EquipmentActionName = "Player/Equipment";
    private void Start()
    {
        MoveActionVector = Asset.FindAction(MoveActionName);
        MoveActionVector.Enable();
        LookActionVector = Asset.FindAction(LookActionName);
        LookActionVector.Enable();
        PrimaryAction = Asset.FindAction(PrimaryActionName);
        PrimaryAction.Enable();
        EquipmentAction = Asset.FindAction(EquipmentActionName);
        EquipmentAction.Enable();
        SecondaryAction = Asset.FindAction(SecondaryActionName);
        SecondaryAction.Enable();
        MenuAction = Asset.FindAction(MenuActionName);
        MenuAction.Enable();
        PointerActionVector = Asset.FindAction(PointerActionName);
        PointerActionVector.Enable();

        PrimaryAction.performed += OnPrimaryAction;
        SecondaryAction.performed += OnSecondaryAction;
        MenuAction.performed += OnMenuAction;
        EquipmentAction.performed += OnEquipmentAction;
    }
    private void OnDestroy()
    {
        PrimaryAction.performed -= OnPrimaryAction;
        SecondaryAction.performed -= OnSecondaryAction;
        MenuAction.performed -= OnMenuAction;
        EquipmentAction.performed -= OnEquipmentAction;
    }

    private void OnPrimaryAction(InputAction.CallbackContext obj)
    {
        OnPrimaryInteraction?.Invoke(obj.performed);
        if (obj.performed == true)
        {

        }
        else
        {

        }
    }

    private void OnSecondaryAction(InputAction.CallbackContext obj)
    {
        OnSecondaryInteraction?.Invoke(obj.performed);
        if (obj.performed == true)
        {

        }
        else 
        {

        }
    }

    private void OnMenuAction(InputAction.CallbackContext obj)
    {
        OnMenuInteraction?.Invoke(obj.performed);
        if (obj.performed == true)
        {

        }
        else
        {

        }
    }
    private void OnEquipmentAction(InputAction.CallbackContext obj)
    {
        OnEquipmentInteraction?.Invoke(obj.performed);
        if (obj.performed == true)
        {

        }
        else
        {

        }
    }

    public Ray GetPointerWorldPosition()
    {
        return Cam.ScreenPointToRay(PointerActionVector.ReadValue<Vector2>());
    }
    public bool GetPointingAt(LayerMask layers, out GameObject hitGO, out float hitDist)
    {
        hitGO = null;
        hitDist = 0f;
        Ray ray = GetPointerWorldPosition();
        Debug.DrawRay(ray.origin, ray.direction);
        if (Physics.Raycast(ray, out RaycastHit hit, layers))
        {
            hitGO = hit.collider.gameObject;
            hitDist = hit.distance;
            return true;
        }
        return false;
    }
    private event Action<bool> OnPrimaryInteraction;    
    public void RegisterPrimaryInteractionListener(Action<bool> listener)
    {
        OnPrimaryInteraction += listener;
    }

    public void UnregisterPrimaryInteractionListener(Action<bool> listener)
    {
        OnPrimaryInteraction -= listener;
    }
    private event Action<bool> OnSecondaryInteraction;
    public void RegisterSecondaryInteractionListener(Action<bool> listener)
    {
        OnSecondaryInteraction += listener;
    }

    public void UnregisterSecondaryInteractionListener(Action<bool> listener)
    {
        OnSecondaryInteraction -= listener;
    }

    public Action<bool> OnMenuInteraction;
    public void RegisterMenuListener(Action<bool> listener)
    {
        OnMenuInteraction += listener;
    }

    public void UnregisterMenuListener(Action<bool> listener)
    {
        OnMenuInteraction -= listener;
    }
    public Action<bool> OnEquipmentInteraction;
    public void RegisterEquipmentListener(Action<bool> listener)
    {
        OnEquipmentInteraction += listener;
    }

    public void UnregisterEquipmentListener(Action<bool> listener)
    {
        OnEquipmentInteraction -= listener;
    }
    public Vector2 GetPointerDeltaInput()
    {
        return GetLookVector();
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public Vector3 GetMovementVector()
    {
        Vector3 move = MoveActionVector.ReadValue<Vector2>();
        move.z = move.y;
        return move;
    }
    public Vector2 GetLookVector()
    {
        return LookActionVector.ReadValue<Vector2>();
    }

    public void SetMouseFocus(bool focused)
    {
        Cursor.lockState = focused ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
