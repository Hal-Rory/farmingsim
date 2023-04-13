using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManagerNew : MonoBehaviour, IInputManager
{
    [SerializeField] private Camera Cam;
    [SerializeField] private float RayDist = 100;
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
    private InputAction PointerActionVector;
    [SerializeField] private string PointerActionName = "UI/Point";

    private event Action<bool> OnPrimaryInteraction;
    private event Action<bool> OnSecondaryInteraction;
    private void Start()
    {
        MoveActionVector = Asset.FindAction(MoveActionName);
        MoveActionVector.Enable();
        LookActionVector = Asset.FindAction(LookActionName);
        LookActionVector.Enable();
        PrimaryAction = Asset.FindAction(PrimaryActionName);
        PrimaryAction.Enable();
        SecondaryAction = Asset.FindAction(SecondaryActionName);
        SecondaryAction.Enable();
        PointerActionVector = Asset.FindAction(PointerActionName);
        PointerActionVector.Enable();

        PrimaryAction.performed += OnPrimaryAction;
        SecondaryAction.performed += OnSecondaryAction;
    }
    private void OnDestroy()
    {
        PrimaryAction.performed -= OnPrimaryAction;
        SecondaryAction.performed -= OnSecondaryAction;
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

    public Ray GetPointerWorldPosition()
    {
        return Cam.ScreenPointToRay(PointerActionVector.ReadValue<Vector2>());
    }
    public bool GetPointingAt(LayerMask layers, out GameObject hitGO)
    {
        hitGO = null;
        Ray ray = GetPointerWorldPosition();
        Debug.DrawRay(ray.origin, ray.direction * RayDist);
        if (Physics.Raycast(ray, out RaycastHit hit, RayDist, layers))
        {
            hitGO = hit.collider.gameObject;
            return true;
        }
        return false;
    }

    public void RegisterPrimaryInteractionListener(Action<bool> listener)
    {
        OnPrimaryInteraction += listener;
    }

    public void UnregisterPrimaryInteractionListener(Action<bool> listener)
    {
        OnPrimaryInteraction -= listener;
    }
    public void RegisterSecondaryInteractionListener(Action<bool> listener)
    {
        OnSecondaryInteraction += listener;
    }

    public void UnregisterSecondaryInteractionListener(Action<bool> listener)
    {
        OnSecondaryInteraction -= listener;
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
