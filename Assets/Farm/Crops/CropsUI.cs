using Items;
using System.Collections;
using UnityEngine;

public class CropsUI : MonoBehaviour
{
    [SerializeField] private InfoHeader Header;
    [SerializeField] private Card Display;
    [SerializeField] private FadingText ModificationText;
    private static float DisplaySpeed => 1f;
    
    void Start()
    {
        Header.SetHeader("crops", "Money:");
        DoCropSet(GameManager.Instance.GetOrSetFirstCrop());
        GameManager.Instance.OnCropSet += DoCropSet;        
        GameManager.Instance.OnMoneyUpdated += DoMoneyUpdated;        
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnCropSet -= DoCropSet;
        GameManager.Instance.OnMoneyUpdated -= DoMoneyUpdated;
    }
    private void DoCropSet(CropData obj)
    {
        Header.SetInfo($"${GameManager.Instance.Money}");
        Display.Set(obj.ID, obj.Name, obj.Display);
    }
    private void DoMoneyUpdated(int amount)
    {
        StopCoroutine(UpdateMoneyDisplay(amount));
        Header.SetInfo($"${GameManager.Instance.Money}");
        StartCoroutine(UpdateMoneyDisplay(amount));
    }
    private IEnumerator UpdateMoneyDisplay(int amount)
    {
        int current = GameManager.Instance.Money;
        int adjusted = amount == 0 ? 0 : 1;
        while(adjusted <= amount)
        {
            Header.SetInfo($"${current+adjusted}");
            adjusted += 1;
            ModificationText.SetForTime(DisplaySpeed/2, "+1");
            yield return new WaitForSeconds(DisplaySpeed);
        }
        Header.SetInfo($"${GameManager.Instance.Money}");
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
