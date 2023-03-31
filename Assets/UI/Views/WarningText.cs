using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningText : MonoBehaviour
{
    [SerializeField] private Text Label;
    private void Awake()
    {
        Label.gameObject.SetActive(false);
    }
    public void SetLabel(string text)
    {
        Label.text = text;
    }
    public void SetForTime(float time)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayWarning(time));
    }
    public void SetForTime(float time, string text)
    {
        SetLabel(text);
        SetForTime(time);
    }
    private IEnumerator DisplayWarning(float time)
    {
        Label.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        Label.gameObject.SetActive(false);
    }
}
