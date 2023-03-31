using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class UICrops : MonoBehaviour
{
    [SerializeField] private InfoHeader Header;
    [SerializeField] private Card Display;
    
    void Start()
    {
        Header.SetHeader("crops", "Current:");
        DoCropSet(GameManager.Instance.GetOrSetFirstCrop());
        GameManager.Instance.OnCropSet += DoCropSet;        
    }

    private void DoCropSet(CropData obj)
    {
        Header.SetInfo(obj.Name);
        Display.Set(obj.ID, obj.Name, obj.Display);
    }

    public void OnGetNextCrop()
    {
        GameManager.Instance.NextCrop();
    }
    public void OnGetPrevCrop()
    {
        GameManager.Instance.PreviousCrop();
    }
}
