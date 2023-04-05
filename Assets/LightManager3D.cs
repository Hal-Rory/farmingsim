using UnityEngine;

public class LightManager3D : MonoBehaviour
{
    [SerializeField] private Light Source;
    Vector3 Rotation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
    }
    public void SetLightState(float degreeDelta)
    {
        //sun up = 90
        //sundown = 360
        Rotation = Vector3.right * degreeDelta;        
        Source.transform.Rotate(Rotation, Space.World);
    }

}
