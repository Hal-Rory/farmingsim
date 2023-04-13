using System;
using UnityEngine;
using UnityEngine.UI;

public class DetailedInfoPanel : InfoHeader
{
    [SerializeField] private Text ButtonText;
    [SerializeField] private Button DetailButton;

    public void SetPanel(string id, string header, string info, string buttonText, Action callback)
    {
        SetHeader(id, header);
        SetInfo(info);
        SetButtonText(buttonText);
        SetButton(callback, true);
    }
    public void SetButtonText(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            ButtonText.text = text;
            DetailButton.gameObject.SetActive(true);
        } else
        {
            DetailButton.gameObject.SetActive(false);
        }
    }
    public void SetButton(Action callback, bool clear = false)
    {
        void invoke()
        {
            callback?.Invoke();
        }
        if (clear)
            DetailButton.onClick.RemoveAllListeners();
        if (DetailButton.gameObject.activeSelf)
            DetailButton.onClick.AddListener(invoke);
    }
    public override void SetEmpty()
    {
        base.SetEmpty();
        DetailButton.onClick.RemoveAllListeners();
    }
}
