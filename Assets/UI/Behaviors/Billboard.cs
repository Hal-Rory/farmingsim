using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform Focus;
    private bool Rendered;
   
    private void Start()
    {
        if (!Focus)
        {
            gameObject.SetActive(false);
            Debug.LogWarning($"{name} is missing billboard focus");
        }
    }
    private void OnBecameVisible()
    {
        Rendered = true;
    }
    private void OnBecameInvisible()
    {
        Rendered = false;
    }
    private void LateUpdate()
    {
        //transform.forward = new Vector3(Focus.forward.x, transform.forward.y, Focus.forward.z);
        //transform.forward = -Focus.forward;
        //transform.LookAt(Focus.position, -Vector3.up);
        if (Rendered)
        {
            Vector3 lookPos = Focus.position - transform.position;
            lookPos.y = 0;
            transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }
}
