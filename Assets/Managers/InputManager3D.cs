using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager3D : MonoBehaviour, IInputManager
{
    [SerializeField] private Camera Cam;
    [SerializeField] private float RayDist = 100;
    public Camera ActiveCamera => Cam;
    public Action<bool> OnPrimaryInteraction;
    public Action<bool> OnSecondaryInteraction;

    public Ray GetPointerWorldPosition()
    {
        return Cam.ScreenPointToRay(Input.mousePosition);
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

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnPrimaryInteraction?.Invoke(true);
        } else if (Input.GetMouseButtonUp(0))
        {
            OnPrimaryInteraction?.Invoke(false);
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnSecondaryInteraction?.Invoke(true);
        } else if (Input.GetMouseButtonUp(1))
        {
            OnSecondaryInteraction?.Invoke(false);
        }
    }
    private void LateUpdate()
    {
        PreviousMousePosition = Input.mousePosition;
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
    //todo: swap with input system asap
    private Vector3 PreviousMousePosition;
    public Vector3 GetPointerInput()
    {
        return (PreviousMousePosition- Input.mousePosition).normalized;
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
