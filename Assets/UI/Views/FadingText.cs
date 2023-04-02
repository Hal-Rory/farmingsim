using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadingText : MonoBehaviour
{
    [SerializeField] private Text Label;
    private void Awake()
    {
        Label.enabled = false;
    }
    public void SetLabel(string text)
    {
        Label.text = text;
    }
    public void SetForTime(float time)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayText(time));
    }
    public void SetForTime(float time, string text)
    {
        StopAllCoroutines();
        SetLabel(text);
        SetForTime(time);
    }
    private IEnumerator DisplayText(float time)
    {
        Label.enabled = true;
        float timeRemaining = time;
        Color current = Label.color;
        while(timeRemaining > 0)
        {
            current.a = timeRemaining / time;
            Label.color= current;
            timeRemaining-= Time.deltaTime;
            yield return null;
        }
        Label.enabled = false;
    }
}
