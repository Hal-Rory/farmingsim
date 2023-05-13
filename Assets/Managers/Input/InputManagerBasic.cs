using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManagerBasic : MonoBehaviour, IInputManager
{
    [SerializeField] private Camera Cam;
    [SerializeField] private float RayDist = 100;
    public Camera ActiveCamera => Cam;
        
    private Vector3 PreviousMousePosition;

    public Ray GetPointerWorldPosition()
    {
        return Cam.ScreenPointToRay(Input.mousePosition);
    }
    public bool GetPointingAt(LayerMask layers, out GameObject hitGO, out float hitDist)
    {
        hitGO = null;
        hitDist = 0f;
        Ray ray = GetPointerWorldPosition();
        Debug.DrawRay(ray.origin, ray.direction * RayDist);
        if (Physics.Raycast(ray, out RaycastHit hit, RayDist, layers))
        {
            hitGO = hit.collider.gameObject;
            hitDist = hit.distance;
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
        if (Input.GetKeyDown(KeyCode.M))
        {
            OnMenuInteraction?.Invoke(true);
        }
        else if (Input.GetKeyUp(KeyCode.M))
        {
            OnMenuInteraction?.Invoke(false);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnMenuInteraction?.Invoke(true);
        }
        else if (Input.GetKeyUp(KeyCode.T))
        {
            OnMenuInteraction?.Invoke(false);
        }
    }
    private void LateUpdate()
    {
        PreviousMousePosition = Input.mousePosition;
    }
    public Action<bool> OnPrimaryInteraction;    
    public void RegisterPrimaryInteractionListener(Action<bool> listener)
    {
        OnPrimaryInteraction += listener;
    }

    public void UnregisterPrimaryInteractionListener(Action<bool> listener)
    {
        OnPrimaryInteraction -= listener;
    }
    public Action<bool> OnSecondaryInteraction;
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
        return (PreviousMousePosition- Input.mousePosition).normalized;
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public Vector3 GetMovementVector()
    {
        float horiz = Input.GetButton("Horizontal") ? Input.GetAxis("Horizontal") != 0 ? Mathf.Sign(Input.GetAxis("Horizontal")) : 0 : 0;
        float vert = Input.GetButton("Vertical") ? Input.GetAxis("Vertical") != 0 ? Mathf.Sign(Input.GetAxis("Vertical")) : 0 : 0;
        return new Vector3(horiz, vert, vert);
    }
    public Vector2 GetLookVector()
    {        
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public void SetMouseFocus(bool focused)
    {        
        Cursor.lockState = focused ? CursorLockMode.Locked : CursorLockMode.None;        
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
}
