using UnityEngine;
using UnityEngine.UI;

public class InfoHeader : MonoBehaviour, IFilterable
{
    [SerializeField] protected Text Info;
    [SerializeField] protected Text Header;

    [field:SerializeField] public string ID {get; private set;}

    public void SetHeader(string id, string header)
    {
        ID = id;
        Header.text = header;
    }
    public void SetInfo(string info)
    {
        if (Info == null) return;
        Info.text = info;
    }  
    public virtual void SetEmpty()
    {
        SetHeader(string.Empty, string.Empty);
        SetInfo(string.Empty);        
    }
}
