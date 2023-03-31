using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public bool Transitioning;
    public float Sens = 1f;
    public float Speed = 3f;
    public float Threshold = .1f;
    public float DefaultFOV => 60;
    public float ZoomIn = 30;
    public float ZoomOut = 90;
    public Camera Cam;
    [Range(1, 10)]
    public float Distance;
    Vector3 Previous;

    public void Follow(Transform trans)
    {
        if (!Transitioning)
        {
            transform.position = Vector3.Lerp(transform.position, trans.TransformPoint(0,0,-Distance), Time.deltaTime * Speed);
            GetState();
        }
    }
    public void Move(Transform trans)
    {
        transform.position = trans.TransformPoint(0, 0, -Distance);
        GetState();
    }

    public bool LerpTo(Vector3 position, bool revert = false)
    {
        if (!Transitioning)
        {
            StartCoroutine(CameraPan(position, revert));
            return true;
        }
        return false;
    }
    public bool Zoom(bool zoomIn, bool revert = false)
    {
        if (!Transitioning)
        {
            StartCoroutine(CameraZoom(zoomIn ? ZoomIn : ZoomOut, revert));
            return true;
        }
        return false;
    }

    public IEnumerator CameraZoom(float amount, bool revert = false)
    {
        Transitioning = true;
        while (Mathf.Abs(Cam.fieldOfView - amount) > Threshold)
        {
            Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, amount, Time.deltaTime * Sens);
            yield return null;
        }
        if (revert)
        {
            Cam.fieldOfView = DefaultFOV;
        }
        Transitioning = false;
    }

    public IEnumerator CameraWiggle()
    {
        yield return null;
    }

    public IEnumerator CameraPan(Vector3 position, bool revert = false)
    {
        Transitioning = true;
        while (Vector3.Distance(transform.position, position) > Threshold)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * Sens);
            yield return null;
        }
        if (revert)
        {
            transform.position = Previous;
        }
        Transitioning = false;
    }

    public void GetState()
    {
        Previous = transform.position;
    }

    public void FullRevert()
    {
        transform.position = Previous;
        Cam.fieldOfView = DefaultFOV;
    }
}
