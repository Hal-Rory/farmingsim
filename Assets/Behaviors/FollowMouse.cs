using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] private bool CanZoom;
    private bool IsZooming;
    IInputManager InputManager => GameManager.Instance.InputManager;
    private void Start()
    {
        InputManager.RegisterSecondaryInteractionListener(Zoom);
    }
    private void OnDestroy()
    {
        if (GameManager.Instance) InputManager.UnregisterSecondaryInteractionListener(Zoom);
    }
    void Update()
    {
        if (!CanZoom || (CanZoom && !IsZooming))
        {
            transform.position = InputManager.GetPointerWorldPosition().GetPoint(1).FlattenZ(transform.position.z);
        } else if(IsZooming)
        {
            Vector3 newPos = transform.position;
            newPos.z += InputManager.GetPointerInput().y * Time.deltaTime;
            transform.position = newPos;
        }
    }

    public void Zoom(bool interaction)
    {
        IsZooming = interaction;
    }
}
