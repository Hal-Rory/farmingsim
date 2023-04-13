using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class AmountWindow : MonoBehaviour
{
    [SerializeField] private Text HeaderText;
    [SerializeField] private Text AmountText;
    public Action<int> OnAmountSet;
    public Action OnCancelled;
    [SerializeField] private int Amount;
    public RangeInt ClampAmount = new RangeInt();
    private void Awake()
    {
        Assert.IsNotNull(HeaderText, $"Header not set on {name}");
        Assert.IsNotNull(AmountText, $"Text not set on {name}");
    }
    public void SetHeader(string header = "How Much?")
    {
        HeaderText.text = header;
    }
    public void SetAmount(int amount = 0)
    {
        AmountText.text = amount.ToString();
    }
    public void Setup(int start, int end)
    {
        Amount = start;
        AmountText.text = start.ToString();
        ClampAmount.start = start;
        ClampAmount.length = end;        
    }
    public void OnModifyAmount(int amount = 1)
    {
        Amount += amount;
        Amount = Mathf.Clamp(Amount, ClampAmount.start, ClampAmount.end);
        SetAmount(Amount);
    }
    public void OnSetAmount()
    {
        OnAmountSet?.Invoke(Amount);
    }
    public void OnCancel()
    {
        OnCancelled?.Invoke();
    }
}
