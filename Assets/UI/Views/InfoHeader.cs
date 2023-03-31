using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoHeader : MonoBehaviour, IFilterable
{
    [SerializeField] protected Text Info;
    [SerializeField] protected Text Header;

    public string ID {get; private set;}

    public void SetHeader(string id, string header)
    {
        ID = id;
        Header.text = header;
    }
    public void SetInfo(string info)
    {
        Info.text = info;
    }    
    public virtual void Clear()
    {
        SetHeader(string.Empty, string.Empty);
        SetInfo(string.Empty);
    }
}
