using GameTime;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TimeUI : ITimeListener
{
    [SerializeField] private Text TimeLabel;

    public void ClockUpdate(int tick)
    {
        TimeStruct timestamp = ITimeManager.Instance.CurrentTime;
        TimeLabel.text = $"{timestamp.Day}, {timestamp.Hour}\n{TimeUtility.GetMonthName(timestamp.Month)} {(timestamp.Date):00}, Year {(timestamp.Year)}";
    }

    public virtual void Register()
    {
        ClockUpdate(0);
        ITimeManager.Instance.RegisterListener(this);
    }
    public virtual void Unregister()
    {
        ITimeManager.Instance.UnregisterListener(this);
    }
}
