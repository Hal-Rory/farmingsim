using UnityEngine;
using UnityEngine.UI;

public class TimeUI : UIPage
{
    [SerializeField] private ToggleCard Pause;
    [SerializeField] private ToggleCard Play;
    [SerializeField] private ToggleCard Fast;
    [SerializeField] private Text TimeLabel;

    protected override void Start()
    {
        base.Start();
        Pause.Selectable.onValueChanged.AddListener(TryPauseTime);
        Play.Selectable.onValueChanged.AddListener(TryStartTime);
        Fast.Selectable.onValueChanged.AddListener(TrySpeedupTime);
        DoTimeChanged(GameManager.Instance.TimeManager.State);
        GameManager.Instance.OnTimeUpdated += DoTimeChanged;        
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Pause.Selectable.onValueChanged.RemoveListener(TryPauseTime);
        Play.Selectable.onValueChanged.RemoveListener(TryStartTime);
        Fast.Selectable.onValueChanged.RemoveListener(TrySpeedupTime);
    }

    private void DoTimeChanged(ITimeManager.TIME_STATE state)
    {
        Play.Selectable.SetIsOnWithoutNotify(state == ITimeManager.TIME_STATE.playing);
        Pause.Selectable.SetIsOnWithoutNotify(state == ITimeManager.TIME_STATE.paused);
        Fast.Selectable.SetIsOnWithoutNotify(state == ITimeManager.TIME_STATE.fast);
    }

    private void Update()
    {
        TimeLabel.text = GameManager.Instance.TimeManager.DisplayTime();
    }

    private void TryPauseTime(bool pause)
    {
        if (pause)
        {
            GameManager.Instance.PauseTime();
        }
    }
    private void TryStartTime(bool start)
    {
        if (start)
        {
            GameManager.Instance.PlayRegularTime();
        }
    }
    private void TrySpeedupTime(bool speedup)
    {
        if (speedup)
        {
            GameManager.Instance.SpeedUpTime();
        }
    }
}
