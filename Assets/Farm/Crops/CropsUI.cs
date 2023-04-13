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
    private void DoCropSet(SeedData obj)
    {
        Header.SetInfo($"${GameManager.Instance.WalletBalance}");
        Display.Set(obj.ID, obj.Name, obj.Display);
    }
    private void DoMoneyUpdated(int amount)
    {
        StopAllCoroutines();
        Header.SetInfo($"${GameManager.Instance.WalletBalance-amount}");
        StartCoroutine(UpdateMoneyDisplay(GameManager.Instance.WalletBalance - amount,amount));
    }
    private IEnumerator UpdateMoneyDisplay(int start, int amount)
    {        
        int adjusted = amount == 0 ? 0 : (int)Mathf.Sign(amount) * 1;
        while(Mathf.Abs(adjusted) <= Mathf.Abs(amount))
        {
            Header.SetInfo($"${start+adjusted}");
            adjusted += (int)Mathf.Sign(amount);
            ModificationText.SetForTime(DisplaySpeed/2, $"{((int)Mathf.Sign(amount) > 0 ? "+" : "-")}1");
            yield return new WaitForSeconds(DisplaySpeed);
        }
        Header.SetInfo($"${GameManager.Instance.WalletBalance}");
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
