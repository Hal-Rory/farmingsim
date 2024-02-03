using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropObjectManager : MonoBehaviour
{
    private Dictionary<string, GameObject> CropObjects;
    private static CropObjectManager _instance;
    public static CropObjectManager Instance => _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            GameObject[] objs = Resources.LoadAll<GameObject>("CropModels");
            CropObjects = new Dictionary<string, GameObject>();
            foreach (GameObject obj in objs)
            {
                CropObjects.Add(obj.name, obj);
                print(obj.name);
            }
        } else
        {
            Destroy(gameObject);
        }
    }

    public static GameObject GetCrop(string name)
    {
        return Instance.CropObjects.TryGetValue(name, out GameObject obj) ? obj : null;
    }
}
