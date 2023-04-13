using System;
using UnityEngine;

public interface IInputManager
{    
    public Camera ActiveCamera { get; }
    public bool GetPointingAt(LayerMask layers, out GameObject hitGO);
    public void RegisterPrimaryInteractionListener(Action<bool> listener);
    public void UnregisterPrimaryInteractionListener(Action<bool> listener);
    public void RegisterSecondaryInteractionListener(Action<bool> listener);
    public void UnregisterSecondaryInteractionListener(Action<bool> listener);
    public Vector2 GetPointerDeltaInput();
    public Ray GetPointerWorldPosition();
    public bool IsPointerOverUI();
    public Vector3 GetMovementVector();
    public Vector2 GetLookVector();
    public void SetMouseFocus(bool focused);
}
